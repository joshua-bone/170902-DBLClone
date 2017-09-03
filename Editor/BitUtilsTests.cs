using System;
using NUnit.Framework;

public class BitUtilsTests
{
    public const string string32 = "11110000111111110000111100001111";
    public const string string64 = "1111000011110000111100001111000011110000111111110000111100001111";

    [Test]
    public void testGetSlice()
    {
        uint n1 = Convert.ToUInt32(string32, 2);
        Assert.AreEqual(0x0F, BitUtils.getSlice(n1, 0, 8));
        Assert.AreEqual(0xF0FF0F0F, BitUtils.getSlice(n1, 0, 32));
        Assert.AreEqual(0xFF, BitUtils.getSlice(n1, 16, 8));
        Assert.AreEqual(1U, BitUtils.getSlice(n1, 31, 100000));
        ulong n2 = Convert.ToUInt64(string64, 2);
        Assert.AreEqual(0xF0F0F0F0F0FF0F0F, BitUtils.getSlice(n2, 0, 10000));
        Assert.AreEqual(3UL, BitUtils.getSlice(n2, 0, 2));
        Assert.AreEqual(0xF, BitUtils.getSlice(n2, 60, 4));
        Assert.AreEqual(0x0F, BitUtils.getSlice(n2, 52, 8));
    }

    [Test]
    public void testSetSlice()
    {
        uint n1 = Convert.ToUInt32(string32, 2);
        Assert.AreEqual(0xF0FF050F, BitUtils.setSlice(n1, 5, 8, 4));
        Assert.AreEqual(0xFFFFFFFF, BitUtils.setSlice(n1, -1, 0, 32));
        Assert.AreEqual(0x00000F0F, BitUtils.setSlice(n1, 0, 16, 500));
        ulong n2 = Convert.ToUInt64(string64, 2);
        Assert.AreEqual(0xF0F0F0F0F0FF1337, BitUtils.setSlice(n2, 4919, 0, 16));
        Assert.AreEqual(0x50F0F0F0F0FF0F0F, BitUtils.setSlice(n2, 5, 60, 4));
        Assert.AreEqual(0x50F0F0F0F0FF0F0F, BitUtils.setSlice(n2, 5, 60, 100000));

    }
}