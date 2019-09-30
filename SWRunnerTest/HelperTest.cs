using NUnit.Framework;
using SWEmulator;
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
            Foo foo = new Foo { A = 1, B = 2 };

            FieldInfo[] fields = foo.GetType().GetFields();
            fields[0].SetValue(foo, 10);

            Assert.AreEqual(10, foo.A);

            DungeonRunConfig runConfig = new DungeonRunConfig();
            runConfig.Width = 120;
            runConfig.Height = 100;

            runConfig.StartPoint.X = 50;
            runConfig.StartPoint.Y = 60;

            runConfig.ReplayPoint.X = 70;
            runConfig.ReplayPoint.Y = 80;


            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(RunConfig));

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

            XmlSerializer serializer = new XmlSerializer(typeof(DungeonRunConfig), new XmlRootAttribute("RunConfig"));

            // Declare an object variable of the type to be deserialized.
            DungeonRunConfig runConfig;

            string testConfigXml = @"TestData/TestConfig.xml";

            using (Stream reader = new FileStream(testConfigXml, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                runConfig = (DungeonRunConfig)serializer.Deserialize(reader);
            }

            Helper.UpdateRunConfig(emulator, runConfig);

            Assert.IsNotNull(runConfig);
        }

        public class Foo
        {
            public int A;
            public int B;
        }
    }
}
