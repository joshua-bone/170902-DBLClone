using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    public ulong Id { get; private set; }
    public string Name { get; private set; }
    public Version WorldVersion { get; private set; }
    public Version GameVersion { get; private set; }
    public ulong AuthorId { get; private set; }
    public Coordinates EditorCameraStart { get; private set; }
    public Dictionary<int, Cell> DefaultCells { get; private set; }
    public Dictionary<Coordinates, Cell> Map { get; private set; }

    private World() { }
    public static World DefaultInstance()
    {
        World w = new World();
        w.Id = DBLMath.GenerateRandomInt64();
        w.Name = "Default World";
        w.WorldVersion = Version.fresh();
        w.GameVersion = Version.fresh();
        w.AuthorId = 0;
        w.EditorCameraStart = Coordinates.of("4096.4096.0");
        w.DefaultCells = new Dictionary<int, Cell>();
        w.Map = new Dictionary<Coordinates, Cell>();
        for (int x = 4096 - 20; x < 4096 + 20; x++)
        {
            for (int y = 4096 - 20; y < 4096 + 20; y++)
            {
                w.Map.Add(Coordinates.of(x, y, 0), Cell.of("0.1.0.0.0.0.0")); //TODO replace with Floor.toCell() or something
            }
        }
        return w;
    }

    public void Write(string fileName)
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
        {
            writer.Write(this.Id);
            writer.Write(this.Name); //length-prefixed with uint
            WorldVersion.Write(writer);
            GameVersion.Write(writer);
            writer.Write(this.AuthorId);

            writer.Write(this.DefaultCells.Count); //numberOfDefaultCells
            foreach (KeyValuePair<int, Cell> entry in this.DefaultCells){
                writer.Write(entry.Key);
                entry.Value.Write(writer);
            }
            Dictionary<Cell, HashSet<Coordinates>> inverseMap = MapToInverse(this.Map);

            writer.Write(inverseMap.Count);
            foreach (KeyValuePair<Cell, HashSet<Coordinates>> entry in inverseMap)
            {
                entry.Key.Write(writer);
                writer.Write(entry.Value.Count);
                foreach (Coordinates coordinates in entry.Value)
                {
                    coordinates.Write(writer);
                }
            }
        }
    }

    public static World Read(string fileName)
    {
        using (BinaryReader reader = new BinaryReader(File.OpenRead(fileName)))
        {
            World world = new World();
            world.Id = reader.ReadUInt64();
            world.Name = reader.ReadString();
            world.WorldVersion = Version.read(reader);
            world.GameVersion = Version.read(reader);
            world.AuthorId = reader.ReadUInt64();

            int numberOfDefaultCells = reader.ReadInt32();
            world.DefaultCells = new Dictionary<int, Cell>();
            for (int i = 0; i < numberOfDefaultCells; i++){
                world.DefaultCells.Add(reader.ReadInt32(), Cell.read(reader));
            }

            Dictionary<Cell, HashSet<Coordinates>> inverseMap = new Dictionary<Cell, HashSet<Coordinates>>();
            int numberOfMapEntries = reader.ReadInt32();
            for (int i = 0; i < numberOfMapEntries; i++)
            {
                Cell cell = Cell.read(reader);
                inverseMap[cell] = new HashSet<Coordinates>();

                int numberOfCoordinates = reader.ReadInt32();
                for (int j = 0; j < numberOfCoordinates; j++)
                {
                    inverseMap[cell].Add(Coordinates.read(reader));
                }
            }
            world.Map = MapFromInverse(inverseMap);
            return world;
        }
    }

    public static Dictionary<Coordinates, Cell> MapFromInverse(Dictionary<Cell, HashSet<Coordinates>> inverseMap)
    {
        Dictionary<Coordinates, Cell> map = new Dictionary<Coordinates, Cell>();
        foreach (KeyValuePair<Cell, HashSet<Coordinates>> kvp in inverseMap)
        {
            foreach (Coordinates c in kvp.Value)
            {
                map.Add(c, kvp.Key);
            }
        }
        return map;
    }

    public static Dictionary<Cell, HashSet<Coordinates>> MapToInverse(Dictionary<Coordinates, Cell> map)
    {
        Dictionary<Cell, HashSet<Coordinates>> inverseMap = new Dictionary<Cell, HashSet<Coordinates>>();
        foreach (KeyValuePair<Coordinates, Cell> kvp in map)
        {
            if (!inverseMap.ContainsKey(kvp.Value))
            {
                inverseMap[kvp.Value] = new HashSet<Coordinates>();
            }
            inverseMap[kvp.Value].Add(kvp.Key);
        }
        return inverseMap;
    }

    //OVERRIDE
    public override string ToString()
    {
        return string.Format("World<{0}>", this.Name);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        World w = obj as World;
        return Equals(w);
    }

    public bool Equals(World other)
    {
        bool eq = Id == other.Id
                          && Name.Equals(other.Name)
                          && WorldVersion.Equals(other.WorldVersion)
                          && GameVersion.Equals(other.GameVersion)
                             && AuthorId == other.AuthorId;

        if (eq){
            foreach(KeyValuePair<int, Cell> entry in this.DefaultCells){
                if (!(other.DefaultCells.ContainsKey(entry.Key)
                      && other.DefaultCells[entry.Key].Equals(entry.Value)))
				{
                    Debug.Log("Default Cells " + entry);
					return false;
				}
            }
        }

        if (eq)
        {
            foreach (KeyValuePair<Coordinates, Cell> entry in this.Map)
            {
                if (!(other.Map.ContainsKey(entry.Key)
                      && other.Map[entry.Key].Equals(entry.Value)))
                {
                    Debug.Log("Map " + entry);
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (int)this.Id; //probably never have a problem with this
    }
}