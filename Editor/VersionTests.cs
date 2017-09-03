using NUnit.Framework;
using System.Collections.Generic;
using System;

public class VersionTests
{
    [Test]
    public void testToString()
    {
        string s1 = "0000000001" + "0000100001" + "000000000111";
        Version version = Version.of(Convert.ToUInt32(s1, 2));
        Assert.AreEqual("1.33.7", version.ToString());
    }

    [Test]
    public void testConstructorsAndEquals()
    {
        Version v1 = Version.of("0.0.15");
        Version v2 = Version.of(0xF);
        Assert.AreEqual(v1, v2);
        Version v3 = Version.copyOf(v2);
        Assert.AreEqual(v2, v3);
        HashSet<Version> set = new HashSet<Version>();
        set.Add(v1);
        set.Add(v2);
        set.Add(v3);
        Assert.AreEqual(1, set.Count);
		v1 = Version.of(123456789U);
		Assert.AreEqual(123456789U, v1.Write());
    }

    [Test]
    public void testGettersAndSetters()
    {
        Version v = Version.fresh();
        Assert.AreEqual("1.0.0", v.ToString());
        v.setMajor(13);
        Assert.AreEqual("13.0.0", v.ToString());
        v.setMinor(3);
        Assert.AreEqual("13.3.0", v.ToString());
        v.setPatch(7);
        Assert.AreEqual("13.3.7", v.ToString());
        Assert.AreEqual(13, v.Major());
		Assert.AreEqual(3, v.Minor());
		Assert.AreEqual(7, v.Patch());
	}

    [Test]
    public void testEdgeCases()
    {
		Assert.Throws<ArgumentException>(() => Version.of("1.1.1.1"));
		Assert.Throws<ArgumentException>(() => Version.of("1.1"));
        Assert.Throws<ArgumentException>(() => Version.of("1024.1.1"));
		Assert.Throws<ArgumentException>(() => Version.of("-1.1.1"));
		Assert.Throws<ArgumentException>(() => Version.of("1.1024.1"));
		Assert.Throws<ArgumentException>(() => Version.of("1.-1.1"));
		Assert.Throws<ArgumentException>(() => Version.of("1.1.4096"));
		Assert.Throws<ArgumentException>(() => Version.of("1.1.-1"));
    }
}