using NUnit.Framework;
using SWEmulator;
using SWRunner;
using SWRunner.Rewards;
using System.Drawing;
using static SWRunner.Rewards.Rune;

namespace SWRunnerTest
{
    [TestFixture]
    class RewardTest
    {
        [Test]
        public void Reward_GivenRuneDrop_SetRuneTypeQuantity1()
        {
            //Rune reward = new Rune("Rune");

            //Assert.AreEqual(RewardType.RUNE, reward.Type);
            //Assert.AreEqual(1, reward.Quantity);
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
            RunResult result = Helper.GetRunResult(runsPath);
            Assert.IsNotNull(result);

            Assert.AreEqual(RewardType.RUNE, result.GetReward().Type);
        }
        
        [Test]
        public void GetRune_GivenRuneInfo_ReturnRune()
        {
            Rune rune = new Rune.RuneBuilder().Set("Violent").Rarity("Hero").Build();

            Assert.AreEqual(rune.Type, RewardType.RUNE);
            Assert.AreEqual(rune.Set, RUNESET.VIOLENT);
            Assert.AreEqual(rune.Rarity, RARITY.HERO);
        }

        [Test]
        public void GetRune_GivenInvalidRuneSet_ReturnUnknownRune()
        {
            Rune rune = new Rune.RuneBuilder().Set("Some invalid set").Rarity("blah").Build();

            Assert.AreEqual(rune.Type, RewardType.RUNE);
            Assert.AreEqual(rune.Set, RUNESET.UNKNOWN);
            Assert.AreEqual(rune.Rarity, RARITY.UNKNOWN);
        }

        [Test]
        public void Test()
        {
            NoxEmulator emulator = new NoxEmulator("Nox");
            emulator.Click(new Point(1006, 594)); // Settings
        }

    }
}
