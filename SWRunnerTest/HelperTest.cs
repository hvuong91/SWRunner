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

            runConfig.StartPoint.X = 0.841f;
            runConfig.StartPoint.Y = 0.710f;

            runConfig.ReplayPoint.X = 0.323f;
            runConfig.ReplayPoint.Y = 0.533f;

            runConfig.GetRunePoint.X = 0.584f;
            runConfig.GetRunePoint.Y = 0.803f;

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
            emulator.Width = 1144;
            emulator.Height = 644;

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

            Assert.AreEqual((int)runConfig.ReplayPoint.X, 369);
            Assert.AreEqual((int)runConfig.ReplayPoint.Y, 343);
        }

        [Test]
        public void SolveQuizTest()
        {
            // Fire
            string firePattern = @"unit_icon_(.)*_1_\d.png";
            Assert.AreEqual(2, Helper.SolveQuiz(firePattern));

            // Wind
            string windPattern = @"unit_icon_(.)*_2_\d.png";
            Assert.AreEqual(5, Helper.SolveQuiz(windPattern));

            // Water
            string waterPattern = @"unit_icon_(.)*_3_\d.png";
            Assert.AreEqual(1, Helper.SolveQuiz(waterPattern));
        }
    }
}
