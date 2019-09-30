using NUnit.Framework;
using SWEmulator;
using SWRunner.Rewards;
using System.Drawing;

namespace SWRunnerTest
{
    [TestFixture]
    class RewardTest
    {
        [Test]
        public void Reward_GivenRuneDrop_SetRuneTypeQuantity1()
        {
            Rune reward = new Rune("Rune");

            Assert.AreEqual(RewardType.RUNE, reward.Type);
            Assert.AreEqual(1, reward.Quantity);
        }

        [Test]
        public void Reward_GivenDropWithQuantity_SetRuneTypeOtherWithQuantity()
        {
            Reward reward = new Reward("Drop Item Type x20");

            Assert.AreEqual(RewardType.OTHER, reward.Type);
            Assert.AreEqual(20, reward.Quantity);
        }

        [Test]
        public void Reward_GivenUnknownDrop_SetRuneTypeOtherWithQuantity1()
        {
            Reward reward = new Reward("Some random drop");

            Assert.AreEqual(RewardType.OTHER, reward.Type);
            Assert.AreEqual(1, reward.Quantity);
        }

        [Test]
        public void GetRunResult_GivenCSV_GetLastRun()
        {
            string runsPath = @"C:\Users\Administrator\Desktop\Rune\test.csv";
            RunResult result = RunParser.GetRunResult(runsPath);
            Assert.IsNotNull(result);

            Assert.AreEqual(RewardType.RUNE, result.GetReward().Type);
        }

        [Test]
        public void Test()
        {
            NoxEmulator emulator = new NoxEmulator("Nox");
            emulator.Click(new Point(1006, 594));
        }
    }
}
