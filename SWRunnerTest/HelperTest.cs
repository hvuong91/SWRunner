using NUnit.Framework;
using SWEmulator;
using SWRunner;
using SWRunner.Rewards;
using SWRunner.Runners;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace SWRunnerTest
{
    [TestFixture]
    public class HelperTest
    {
        [Test]
        public void Helper_GivenRunConfigAndEmulator_ProcessAllClickPoints()
        {

            CairosRunnerConfig runConfig = new CairosRunnerConfig();
            runConfig.Width = 120;
            runConfig.Height = 100;

            runConfig.StartPoint.X = 50;
            runConfig.StartPoint.Y = 60;

            runConfig.ReplayPoint.X = 70;
            runConfig.ReplayPoint.Y = 80;

            runConfig.GetRune.X = 100;
            runConfig.GetRune.Y = 200;

            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(RunnerConfig));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//TestConfig.xml";
            System.IO.FileStream file = System.IO.File.Create(path);

            writer.Serialize(file, runConfig);
            file.Close();
        }

        [Test]
        public void Test()
        {
            NoxEmulator emulator = new NoxEmulator("Nox");
            emulator.Width = 20;
            emulator.Height = 50;

            XmlSerializer serializer = new XmlSerializer(typeof(CairosRunnerConfig), new XmlRootAttribute("RunConfig"));

            // Declare an object variable of the type to be deserialized.
            CairosRunnerConfig runConfig;

            string testConfigXml = @"TestData/TestConfig.xml";

            using (Stream reader = new FileStream(testConfigXml, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                runConfig = (CairosRunnerConfig)serializer.Deserialize(reader);
            }

            Helper.UpdateRunConfig(emulator, runConfig);

            Assert.IsNotNull(runConfig);
        }

    }
}
