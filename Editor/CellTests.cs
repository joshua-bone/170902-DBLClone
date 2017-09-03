using NUnit.Framework;
using System.Collections.Generic;
using System;

public class CellTests
{
	[Test]
	public void testToString()
	{
		string s1 = "00000001" + "000000100001" + "00000111" + "000000000000" + "00000010" + "000000000100" + "1000";
		Cell Cell = Cell.of(Convert.ToUInt64(s1, 2));
		Assert.AreEqual("1.33.7.0.2.4.8", Cell.ToString());
	}

	[Test]
	public void testConstructorsAndEquals()
	{
		Cell c1 = Cell.of("0.0.0.0.0.0.15");
		Cell c2 = Cell.of(0xF);
		Assert.AreEqual(c1, c2);
		Cell c3 = Cell.copyOf(c2);
		Assert.AreEqual(c2, c3);
		HashSet<Cell> set = new HashSet<Cell>();
		set.Add(c1);
		set.Add(c2);
		set.Add(c3);
		Assert.AreEqual(1, set.Count);
		c1 = Cell.of(123456789UL);
		Assert.AreEqual(123456789UL, c1.Write());
	}

	[Test]
	public void testGettersAndSetters()
	{
		Cell c = Cell.of("64.32.16.8.4.2.1");
		Assert.AreEqual("64.32.16.8.4.2.1", c.ToString());
        c.setBase(13);
		Assert.AreEqual("13.32.16.8.4.2.1", c.ToString());
		c.setBaseMod(3);
		Assert.AreEqual("13.3.16.8.4.2.1", c.ToString());
        c.setPickup(7);
		Assert.AreEqual("13.3.7.8.4.2.1", c.ToString());
        c.setPickupMod(1);
        Assert.AreEqual("13.3.7.1.4.2.1", c.ToString());
        c.setAction(2);
		Assert.AreEqual("13.3.7.1.2.2.1", c.ToString());
		c.setActionMod(4);
		Assert.AreEqual("13.3.7.1.2.4.1", c.ToString());
        c.setThinWall(8);
		Assert.AreEqual("13.3.7.1.2.4.8", c.ToString());
	}

	[Test]
	public void testEdgeCases()
	{
		Assert.Throws<ArgumentException>(() => Cell.of("1.1.1.1.1.1.1.1"));
		Assert.Throws<ArgumentException>(() => Cell.of("1.1.1.1.1.1"));

		Assert.Throws<ArgumentException>(() => Cell.of("256.1.1.1.1.1.1"));
		Assert.DoesNotThrow(() => Cell.of("255.1.1.1.1.1.1"));
		Assert.Throws<ArgumentException>(() => Cell.of("-1.1.1.1.1.1.1"));

		Assert.Throws<ArgumentException>(() => Cell.of("1.4096.1.1.1.1.1"));
		Assert.DoesNotThrow(() => Cell.of("1.4095.1.1.1.1.1"));
		Assert.Throws<ArgumentException>(() => Cell.of("1.-1.1.1.1.1.1"));

		Assert.Throws<ArgumentException>(() => Cell.of("1.1.256.1.1.1.1"));
		Assert.DoesNotThrow(() => Cell.of("1.1.255.1.1.1.1"));
		Assert.Throws<ArgumentException>(() => Cell.of("1.1.-1.1.1.1.1"));

		Assert.Throws<ArgumentException>(() => Cell.of("1.1.1.4096.1.1.1"));
		Assert.DoesNotThrow(() => Cell.of("1.1.1.4095.1.1.1"));
		Assert.Throws<ArgumentException>(() => Cell.of("1.1.1.-1.1.1.1"));

		Assert.Throws<ArgumentException>(() => Cell.of("1.1.1.1.256.1.1"));
		Assert.DoesNotThrow(() => Cell.of("1.1.1.1.255.1.1"));
		Assert.Throws<ArgumentException>(() => Cell.of("1.1.1.1.-1.1.1"));

		Assert.Throws<ArgumentException>(() => Cell.of("1.1.1.1.1.4096.1"));
		Assert.DoesNotThrow(() => Cell.of("1.1.1.1.1.4095.1"));
		Assert.Throws<ArgumentException>(() => Cell.of("1.1.1.1.1.-1.1"));

		Assert.Throws<ArgumentException>(() => Cell.of("1.1.1.1.1.1.16"));
		Assert.DoesNotThrow(() => Cell.of("1.1.1.1.1.1.15"));
		Assert.Throws<ArgumentException>(() => Cell.of("1.1.1.1.1.1.-1"));
	}
}