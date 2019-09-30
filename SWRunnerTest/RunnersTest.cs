using NUnit.Framework;
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
            CairosRunner test = new CairosRunner(null, testLogFile);

            File.WriteAllText(testLogFile, "Test");

            Assert.IsTrue(test.IsEnd());
        }
    }
}
