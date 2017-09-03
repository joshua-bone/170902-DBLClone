using System;

public static class BitUtils
{
    public static uint getSlice(uint input, int startIndex, int width)
    {
        verifyArePositive(startIndex, width);
        uint mask = width >= 32 ? ~0U << startIndex : ((1U << width) - 1U) << startIndex;
        return (input & mask) >> startIndex;
    }

    public static ulong getSlice(ulong input, int startIndex, int width)
    {
        verifyArePositive(startIndex, width);
        ulong mask = width >= 64 ? ~0UL << startIndex : ((1UL << width) - 1UL) << startIndex;
        return (input & mask) >> startIndex;
    }

    public static uint setSlice(uint bitVector, int value, int startIndex, int width)
    {
        return setSlice(bitVector, (uint)value, startIndex, width);
    }

    public static uint setSlice(uint bitVector, uint value, int startIndex, int width)
    {
        verifyArePositive(startIndex, width);
        uint mask = width >= 32 ? ~0U << startIndex : ((1U << width) - 1U) << startIndex;
        uint chunk = mask & (value << startIndex);
        return chunk + (bitVector & (~mask));
    }

    public static ulong setSlice(ulong bitVector, int value, int startIndex, int width)
    {
        return setSlice(bitVector, (ulong)value, startIndex, width);
    }

    public static ulong setSlice(ulong bitVector, uint value, int startIndex, int width)
    {
        return setSlice(bitVector, (ulong)value, startIndex, width);
    }

    public static ulong setSlice(ulong bitVector, ulong value, int startIndex, int width)
    {
        verifyArePositive(startIndex, width);
        ulong mask = width >= 64 ? ~0UL << startIndex : ((1UL << width) - 1UL) << startIndex;
        ulong chunk = mask & (value << startIndex);
        return chunk + (bitVector & (~mask));
    }

    public static void verifyArePositive(int n1, int n2)
    {
        if (n1 < 0 || n2 < 0)
            throw new ArgumentOutOfRangeException();
    }
}

