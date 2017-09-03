using System;

public class Cell
{
	public enum Field { ThinWall, ActionMod, Action, PickupMod, Pickup, BaseMod, Base }; //static by definition
	private static int[] WIDTHS = new int[] { 4, 12, 8, 12, 8, 12, 8 };

	private ulong state;

	//CONSTRUCTORS AND FACTORIES
	private Cell(ulong state)
	{
		this.state = state;
	}

	public static Cell of(ulong state)
	{
		return new Cell(state);
	}

	public static Cell of(string state)
	{
		return Cell.of(Parse(state));
	}

	public static Cell copyOf(Cell other)
	{
		return new Cell(other.Write());
	}

	//PUBLIC API
	public ulong Write()
	{
		return this.state;
	}

	public uint Base()
	{
        return Convert.ToUInt32(getSlice(Field.Base));
	}

	public uint BaseMod()
	{
		return Convert.ToUInt32(getSlice(Field.BaseMod));
	}

	public uint Pickup()
	{
        return Convert.ToUInt32(getSlice(Field.Pickup));
	}

	public uint PickupMod()
	{
        return Convert.ToUInt32(getSlice(Field.PickupMod));
	}

	public uint Action()
	{
        return Convert.ToUInt32(getSlice(Field.Action));
	}

	public uint ActionMod()
	{
        return Convert.ToUInt32(getSlice(Field.ActionMod));
	}

	public uint ThinWall()
	{
        return Convert.ToUInt32(getSlice(Field.ThinWall));
	}

	public void setBase(int value)
	{
        ValidateInputSize(Field.Base, value);
		state = BitUtils.setSlice(state, value, getOffsetOf(Field.Base), getWidthOf(Field.Base));
	}

	public void setBaseMod(int value)
	{
		ValidateInputSize(Field.BaseMod, value);
		state = BitUtils.setSlice(state, value, getOffsetOf(Field.BaseMod), getWidthOf(Field.BaseMod));
	}

	public void setPickup(int value)
	{
        ValidateInputSize(Field.Pickup, value);
        state = BitUtils.setSlice(state, value, getOffsetOf(Field.Pickup), getWidthOf(Field.Pickup));
	}

	public void setPickupMod(int value)
	{
        ValidateInputSize(Field.PickupMod, value);
        state = BitUtils.setSlice(state, value, getOffsetOf(Field.PickupMod), getWidthOf(Field.PickupMod));
	}

	public void setAction(int value)
	{
        ValidateInputSize(Field.Action, value);
        state = BitUtils.setSlice(state, value, getOffsetOf(Field.Action), getWidthOf(Field.Action));
	}

	public void setActionMod(int value)
	{
        ValidateInputSize(Field.ActionMod, value);
        state = BitUtils.setSlice(state, value, getOffsetOf(Field.ActionMod), getWidthOf(Field.ActionMod));
	}

	public void setThinWall(int value)
	{
        ValidateInputSize(Field.ThinWall, value);
        state = BitUtils.setSlice(state, value, getOffsetOf(Field.ThinWall), getWidthOf(Field.ThinWall));
	}

	//OVERRIDE
	public override string ToString()
	{
        return string.Format("{0}.{1}.{2}.{3}.{4}.{5}.{6}", Base(), BaseMod(), Pickup(), PickupMod(), Action(), ActionMod(), ThinWall());
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
			return false;
		Cell v = obj as Cell;
		return Equals(v);
	}

	public bool Equals(Cell other)
	{
		return state == other.Write();
	}

	public override int GetHashCode()
	{
		return this.ToString().GetHashCode();
	}

	//PRIVATE
	private ulong getSlice(Field field)
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

	private static ulong Parse(string state)
	{
		string[] partStrings = state.Split('.');
		if (partStrings.Length != 7)
		{
			throw new ArgumentException("Invalid Cell String Format");
		}
		int[] parts = new int[7];
		for (int i = 0; i < 7; i++)
		{
			parts[i] = Int32.Parse(partStrings[i]);
		}
		Cell Cell = new Cell(0);
        Cell.setBase(parts[0]);
		Cell.setBaseMod(parts[1]);
		Cell.setPickup(parts[2]);
		Cell.setPickupMod(parts[3]);
        Cell.setAction(parts[4]);
        Cell.setActionMod(parts[5]);
        Cell.setThinWall(parts[6]);
		return Cell.Write();
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