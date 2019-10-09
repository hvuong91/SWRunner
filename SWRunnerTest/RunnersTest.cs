using NUnit.Framework;
using SWEmulator;
using SWRunner.Filters;
using SWRunner.Runners;
using System.IO;

namespace SWRunnerTest
{
    [TestFixture]
    class RunnersTest
    {
        private const string testLogFile = @"TestData/testLogFile.txt"; 

        [Test]
        public void IsEnd_GivenLogFileBeingModified_ReturnTrue()
        {
            CairosRunner test = new CairosRunner(
                new CairosFilter(), testLogFile, @"C:\Users\Administrator\Desktop\Rune\full_log - Copy.txt", new CairosRunnerConfig(), new NoxEmulator());

            File.WriteAllText(testLogFile, "Test");

            Assert.IsTrue(test.IsEnd());

            test.GetCurrentEnergy();
        }

    }
}
