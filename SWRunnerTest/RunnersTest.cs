using NUnit.Framework;
using SWEmulator;
using SWRunner;
using SWRunner.Filters;
using SWRunner.Rewards;
using SWRunner.Runners;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using static SWRunner.Rewards.Rune;

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
        public void TestFoundOkButton()
        {
            CairosRunner test = new CairosRunner(
                new CairosFilter(), testLogFile, @"C:\Users\Administrator\Desktop\Rune\full_log.txt", new CairosRunnerConfig(), new NoxEmulator(), new RunnerLogger());

            Assert.True(test.FoundOkButton());
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
            BlueStacksEmulator emulator = new BlueStacksEmulator();
            emulator.PrintWindow();
        }

        [Test]
        public void NoxEmulatorTestQuiz()
        {
            NoxEmulator emulator = new NoxEmulator();
            emulator.PrintWindow();
        }

        [Test]
        public void NoxEmulatorTestSellRuneInRift()
        {
            NoxEmulator emulator = new NoxEmulator();

            RiftRunnerConfig config = new RiftRunnerConfig();
            config.SellRunePoint = new System.Drawing.PointF(0.417f, 0.750f);

            Helper.UpdateRunConfig(emulator, config);

            emulator.Click(config.SellRunePoint);
        }

    }
}
