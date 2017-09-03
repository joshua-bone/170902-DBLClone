using NUnit.Framework;

public class DBLMathTests
{
	[Test]
	public void testPow()
	{
        Assert.AreEqual(256, DBLMath.Pow(2, 8));
	}
}
