using System;
using System.IO;

public class Version
{
    public enum Field { Patch, Minor, Major }; //static by definition
    private static int[] WIDTHS = new int[] { 12, 10, 10 };
    public uint State { get; private set; }

    //CONSTRUCTORS AND FACTORIES
    private Version(uint state)
    {
        this.State = state;
    }

    public static Version fresh()
    {
        return of("1.0.0");
    }

    public static Version of(uint state)
    {
        return new Version(state);
    }

    public static Version of(string state)
    {
        return of(Parse(state));
    }

    public static Version copyOf(Version other)
    {
        return new Version(other.State);
    }

	public static Version read(BinaryReader reader)
	{
		return Version.of(reader.ReadUInt32());
	}

    //PUBLIC API
    public void Write(BinaryWriter writer)
    {
        writer.Write(this.State);
    }

    public uint Major()
    {
        return getSlice(Field.Major);
    }

    public uint Minor()
    {
        return getSlice(Field.Minor);
    }

    public uint Patch()
    {
        return getSlice(Field.Patch);
    }

    public void setMajor(int value)
    {
        ValidateInputSize(Field.Major, value);
        this.State = BitUtils.setSlice(this.State, value, getOffsetOf(Field.Major), getWidthOf(Field.Major));
    }

    public void setMinor(int value)
    {
		ValidateInputSize(Field.Minor, value);
		this.State = BitUtils.setSlice(this.State, value, getOffsetOf(Field.Minor), getWidthOf(Field.Minor));
    }

    public void setPatch(int value)
    {
        ValidateInputSize(Field.Patch, value);
		this.State = BitUtils.setSlice(this.State, value, getOffsetOf(Field.Patch), getWidthOf(Field.Patch));
    }

    //OVERRIDE
    public override string ToString()
    {
        return string.Format("{0}.{1}.{2}", Major(), Minor(), Patch());
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        Version v = obj as Version;
        return Equals(v);
    }

    public bool Equals(Version other)
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
            throw new ArgumentException("Invalid Version String Format");
        }
        int[] parts = new int[3];
        for (int i = 0; i < 3; i++)
        {
            parts[i] = Int32.Parse(partStrings[i]);
        }
        Version version = new Version(0);
        version.setMajor(parts[0]);
        version.setMinor(parts[1]);
        version.setPatch(parts[2]);
        return version.State;
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