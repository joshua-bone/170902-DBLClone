using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.IO;

public class ExperimentTests
{
    [Test]
    public static void Experiment()
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open("george.dbl", FileMode.Create)))
        {
            writer.Write(1.250F);
            writer.Write(@"c:\Temp");
            writer.Write(10);
            writer.Write(true);
        }

        float aspectRatio;
        string tempDirectory;
        int autoSaveTime;
        bool showStatusBar;

        Assert.IsTrue(File.Exists("george.dbl"));

        using (BinaryReader reader = new BinaryReader(File.Open("george.dbl", FileMode.Open)))
        {
            aspectRatio = reader.ReadSingle();
            tempDirectory = reader.ReadString();
            autoSaveTime = reader.ReadInt32();
            showStatusBar = reader.ReadBoolean();
        }

        Assert.AreEqual(1.250F, aspectRatio);
        Assert.AreEqual("c:\\Temp", tempDirectory);
        Assert.AreEqual(10, autoSaveTime);
        Assert.AreEqual(true, showStatusBar);
	}
}
