using System;
using System.IO;

public class Coordinates
{
    public enum Field { Z, Y, X }; //static by definition
    private static int[] WIDTHS = new int[] { 6, 13, 13 };

    public uint State { get; private set; }

    //CONSTRUCTORS AND FACTORIES
    private Coordinates(uint state)
    {
        this.State = state;
    }

    public static Coordinates of(uint state)
    {
        return new Coordinates(state);
    }

    public static Coordinates of(string state)
    {
        return Coordinates.of(Parse(state));
    }

    public static Coordinates of(int x, int y, int z)
    {
        return new Coordinates(0).setX(x).setY(y).setZ(z);
    }

    public static Coordinates copyOf(Coordinates other)
    {
        return new Coordinates(other.State);
    }

	public static Coordinates read(BinaryReader reader)
	{
		return Coordinates.of(reader.ReadUInt32());
	}

    //PUBLIC API
    public void Write(BinaryWriter writer)
    {
        writer.Write(this.State);
    }

    public uint X()
    {
        return getSlice(Field.X);
    }

    public uint Y()
    {
        return getSlice(Field.Y);
    }

    public uint Z()
    {
        return getSlice(Field.Z);
    }

    public Coordinates setX(int value)
    {
        ValidateInputSize(Field.X, value);     
        this.State = BitUtils.setSlice(this.State, value, getOffsetOf(Field.X), getWidthOf(Field.X));
        return this;
    }

    public Coordinates setY(int value)
    {
        ValidateInputSize(Field.Y, value);
        this.State = BitUtils.setSlice(this.State, value, getOffsetOf(Field.Y), getWidthOf(Field.Y));
        return this;
    }

    public Coordinates setZ(int value)
    {
        ValidateInputSize(Field.Z, value);
        this.State = BitUtils.setSlice(this.State, value, getOffsetOf(Field.Z), getWidthOf(Field.Z));
        return this;
    }

    //OVERRIDE
    public override string ToString()
    {
        return string.Format("{0}.{1}.{2}", X(), Y(), Z());
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        Coordinates v = obj as Coordinates;
        return Equals(v);
    }

    public bool Equals(Coordinates other)
    {
        return this.State == other.State;
    }

    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }

    //PRIVATE
    private uint getSlice(Field field)
    {
        return BitUtils.getSlice(this.State, getOffsetOf(field), WIDTHS[(int)field]);
    }

    private static int getWidthOf(Field field)
    {
        return WIDTHS[(int)field];
    }

    private static int getOffsetOf(Field thisField)
    {
        int offset = 0;
        foreach (Field field in Enum.GetValues(typeof(Field)))
        {
            if (field == thisField)
                break;
            offset += getWidthOf(field);
        }
        return offset;
    }

    private static uint Parse(string state)
    {
        string[] partStrings = state.Split('.');
        if (partStrings.Length != 3)
        {
            throw new ArgumentException("Invalid Coordinates String Format");
        }
        int[] parts = new int[3];
        for (int i = 0; i < 3; i++)
        {
            parts[i] = Int32.Parse(partStrings[i]);
        }
        Coordinates Coordinates = new Coordinates(0);
        Coordinates.setX(parts[0]);
        Coordinates.setY(parts[1]);
        Coordinates.setZ(parts[2]);
        return Coordinates.State;
    }

    private static void ValidateInputSize(Field field, int value)
    {
        if (value >= DBLMath.Pow(2, getWidthOf(field)) || value < 0)
        {
            throw new ArgumentException("Value " + value + " does not fit " +
                                        "into field of width " + getWidthOf(field));
        }
    }
}