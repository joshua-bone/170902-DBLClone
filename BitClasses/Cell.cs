using System;
using System.IO;

public class Cell
{
    public enum Field { ThinWall, ActionMod, Action, PickupMod, Pickup, BaseMod, Base }; //static by definition
    private static int[] WIDTHS = new int[] { 4, 12, 8, 12, 8, 12, 8 };

    public ulong State { get; private set; }

    //CONSTRUCTORS AND FACTORIES
    private Cell(ulong state)
    {
        this.State = state;
    }

    public static Cell of(ulong state)
    {
        return new Cell(state);
    }

    public static Cell of(string state)
    {
        return Cell.of(Parse(state));
    }

    public static Cell read(BinaryReader reader) {
        return Cell.of(reader.ReadUInt64());
    }

    public static Cell of(int _base, 
                          int baseMod, 
                          int pickup, 
                          int pickupMod, 
                          int action, 
                          int actionMod, 
                          int thinWall)
    {
        return Cell.of(0)
                   .setBase(_base)
                   .setBaseMod(baseMod)
                   .setPickup(pickup)
                   .setPickupMod(pickupMod)
                   .setAction(action)
                   .setActionMod(actionMod)
                   .setThinWall(thinWall);
    }

    public static Cell copyOf(Cell other)
    {
        return new Cell(other.State);
    }

    //PUBLIC API
    public void Write(BinaryWriter writer)
    {
        writer.Write(this.State);
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

    public Cell setBase(int value)
    {
        return setField(Field.Base, value);
    }

    public Cell setBaseMod(int value)
    {
        return setField(Field.BaseMod, value);
    }

    public Cell setPickup(int value)
    {
        return setField(Field.Pickup, value);
    }

    public Cell setPickupMod(int value)
    {
        return setField(Field.PickupMod, value);
    }

    public Cell setAction(int value)
    {
        return setField(Field.Action, value);
    }

    public Cell setActionMod(int value)
    {
        return setField(Field.ActionMod, value);
    }

    public Cell setThinWall(int value)
    {
        return setField(Field.ThinWall, value);
    }

    public Cell setField(Field field, int value){
        ValidateInputSize(field, value);
        this.State = BitUtils.setSlice(this.State, value, getOffsetOf(field), getWidthOf(field));
        return this;
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
        return this.State == other.State;
    }

    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }

    //PRIVATE
    private ulong getSlice(Field field)
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

    private static ulong Parse(string state)
    {
        string[] partStrings = state.Split('.');
        if (partStrings.Length != 7)
        {
            throw new ArgumentException("Invalid Cell String Format");
        }
        int[] p = new int[7];
        for (int i = 0; i < 7; i++)
        {
            p[i] = Int32.Parse(partStrings[i]);
        }
        Cell Cell = Cell.of(p[0], p[1], p[2], p[3], p[4], p[5], p[6]);
        return Cell.State;
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