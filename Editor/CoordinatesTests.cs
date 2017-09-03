using NUnit.Framework;
using System.Collections.Generic;
using System;

public class CoordinatesTests
{
	[Test]
	public void testToString()
	{
		string s1 = "0000000000001" + "100001" + "0000000000111";
		Coordinates Coordinates = Coordinates.of(Convert.ToUInt32(s1, 2));
		Assert.AreEqual("1.33.7", Coordinates.ToString());
	}

	[Test]
	public void testConstructorsAndEquals()
	{
        Coordinates c1 = Coordinates.of("0.0.15");
        Coordinates c2 = Coordinates.of(0xF);
		Assert.AreEqual(c1, c2);
        Coordinates c3 = Coordinates.copyOf(c2);
		Assert.AreEqual(c2, c3);
		HashSet<Coordinates> set = new HashSet<Coordinates>();
		set.Add(c1);
		set.Add(c2);
		set.Add(c3);
		Assert.AreEqual(1, set.Count);
		c1 = Coordinates.of(123456789U);
		Assert.AreEqual(123456789U, c1.Write());
	}

	[Test]
	public void testGettersAndSetters()
	{
        Coordinates c = Coordinates.of("1.0.0");
		Assert.AreEqual("1.0.0", c.ToString());
		c.setX(13);
		Assert.AreEqual("13.0.0", c.ToString());
		c.setY(3);
		Assert.AreEqual("13.3.0", c.ToString());
		c.setZ(7);
		Assert.AreEqual("13.3.7", c.ToString());
		Assert.AreEqual(13, c.X());
		Assert.AreEqual(3, c.Y());
		Assert.AreEqual(7, c.Z());
	}

	[Test]
	public void testEdgeCases()
	{
		Assert.Throws<ArgumentException>(() => Coordinates.of("1.1.1.1"));
		Assert.Throws<ArgumentException>(() => Coordinates.of("1.1"));
		Assert.Throws<ArgumentException>(() => Coordinates.of("8192.1.1"));
        Assert.DoesNotThrow(() => Coordinates.of("8191.1.1"));
		Assert.Throws<ArgumentException>(() => Coordinates.of("-1.1.1"));
		Assert.Throws<ArgumentException>(() => Coordinates.of("1.64.1"));
        Assert.DoesNotThrow(() => Coordinates.of("1.63.1"));
		Assert.Throws<ArgumentException>(() => Coordinates.of("1.-1.1"));
		Assert.Throws<ArgumentException>(() => Coordinates.of("1.1.8192"));
        Assert.DoesNotThrow(() => Coordinates.of("1.1.8191"));
		Assert.Throws<ArgumentException>(() => Coordinates.of("1.1.-1"));
	}
}