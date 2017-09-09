using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.IO;

public class WorldTests
{
	[Test]
	public static void TestSaveAndLoad()
	{
        string path = "SavedMaps/____test_world____";
        World world = World.DefaultInstance();
        world.Map.Add(Coordinates.of(1, 2, 3), Cell.of(1337UL));
        world.DefaultCells.Add(1, Cell.of(1337UL));
        world.Write(path);
        World world2 = World.Read(path);
        File.Delete(path);
        Assert.AreEqual(world, world2);
	}

    [Test]
    public static void TestMapToAndFromInverse()
    {
        Dictionary<Coordinates, Cell> map = new Dictionary<Coordinates, Cell>();
		for (int x = 4096 - 20; x < 4096 + 20; x++)
		{
			for (int y = 4096 - 20; y < 4096 + 20; y++)
			{
				map.Add(Coordinates.of(x, y, 0), Cell.of(x - 4000, 0, 0, 0, 0, 0, 0));
			}
		}
        Dictionary<Cell, HashSet<Coordinates>> inverseMap = World.MapToInverse(map);
        Assert.AreEqual(map, World.MapFromInverse(inverseMap));
        Assert.AreEqual(40, inverseMap.Count);
        for (int x = 76; x < 116; x++){
            Assert.AreEqual(inverseMap[Cell.of(x, 0, 0, 0, 0, 0, 0)].Count, 40);
        }
    }
}
