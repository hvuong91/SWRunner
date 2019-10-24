using NUnit.Framework;
using SWEmulator;
using SWRunner;
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
        public void TestNeedRefill()
        {
            CairosRunner test = new CairosRunner(
                new CairosFilter(), testLogFile, @"C:\Users\Administrator\Desktop\Rune\full_log.txt", new CairosRunnerConfig(), new NoxEmulator(), new RunnerLogger());

            Assert.True(test.NeedRefill());
        }

        [Test]
        public void TestIsFailedCariosRun()
        {
            CairosRunner test = new CairosRunner(
                new CairosFilter(), testLogFile, @"C:\Users\Administrator\Desktop\Rune\full_log.txt", new CairosRunnerConfig(), new NoxEmulator(), new RunnerLogger());

            Assert.True(test.IsFailed());
        }

        [Test]
        public void TestIsFailedToaRun()
        {
            ToARunner test = new ToARunner("", "", new ToaRunnerConfig(), new NoxEmulator(), new RunnerLogger());

            Assert.True(test.IsFailed());
        }

        [Test]
        public void TestKeySend()
        {
            NoxEmulator emulator = new NoxEmulator();
            emulator.PressEsc();
        }

        [Test]
        public void EmulatorTestQuiz()
        {
            NoxEmulator emulator = new NoxEmulator();
            emulator.PrintWindow();
        }

    }
}
