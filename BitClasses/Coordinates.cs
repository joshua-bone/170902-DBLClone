using System;

public class Coordinates
{
    public enum Field { Z, Y, X }; //static by definition
	private static int[] WIDTHS = new int[] { 13, 6, 13 };

	private uint state;

	//CONSTRUCTORS AND FACTORIES
	private Coordinates(uint state)
	{
		this.state = state;
	}

	public static Coordinates of(uint state)
	{
		return new Coordinates(state);
	}

	public static Coordinates of(string state)
	{
		return Coordinates.of(Parse(state));
	}

	public static Coordinates copyOf(Coordinates other)
	{
		return new Coordinates(other.Write());
	}

	//PUBLIC API
	public uint Write()
	{
		return this.state;
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

	public void setX(int value)
	{
        ValidateInputSize(Field.X, value);
		state = BitUtils.setSlice(state, value, getOffsetOf(Field.X), getWidthOf(Field.X));
	}

	public void setY(int value)
	{
        ValidateInputSize(Field.Y, value);
		state = BitUtils.setSlice(state, value, getOffsetOf(Field.Y), getWidthOf(Field.Y));
	}

	public void setZ(int value)
	{
        ValidateInputSize(Field.Z, value);
		state = BitUtils.setSlice(state, value, getOffsetOf(Field.Z), getWidthOf(Field.Z));
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
		return state == other.Write();
	}

	public override int GetHashCode()
	{
		return this.ToString().GetHashCode();
	}

	//PRIVATE
	private uint getSlice(Field field)
	{
		return BitUtils.getSlice(this.state, getOffsetOf(field), WIDTHS[(int)field]);
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
		return Coordinates.Write();
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