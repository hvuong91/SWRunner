using NUnit.Framework;
using SWEmulator;
using SWRunner;
using SWRunner.Rewards;
using SWRunnerApp;
using System;
using System.Drawing;
using static SWRunner.Rewards.Rune;

namespace SWRunnerTest
{
    [TestFixture]
    class RewardTest
    {
        public object QuizSolve { get; private set; }

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

            Assert.AreEqual(REWARDTYPE.OTHER, reward.Type);
            Assert.AreEqual(20, reward.Quantity);
        }

        [Test]
        public void Reward_GivenUnknownDrop_SetRuneTypeOtherWithQuantity1()
        {
            Reward reward = new Reward("Some random drop");

            Assert.AreEqual(REWARDTYPE.OTHER, reward.Type);
            Assert.AreEqual(1, reward.Quantity);
        }

        [Test]
        public void TestGrindStoneCreation()
        {
            GrindStone grindStone = new GrindStone("Violent", "%ATK", "5", "10");
            Assert.AreEqual(RUNESET.VIOLENT, grindStone.Set);
            Assert.AreEqual(RARITY.LEGENDARY, grindStone.Rarity);
        }

        [Test]
        public void GetRunResult_GivenCSV_GetLastRun()
        {
            string runsPath = @"C:\Users\Administrator\Desktop\Rune\test.csv";
            RunResult result = Helper.GetRunResult(runsPath);
            Assert.IsNotNull(result);

            Assert.AreEqual(REWARDTYPE.RUNE, result.GetReward().Type);
        }
        
        [Test]
        public void GetRune_GivenRuneInfo_ReturnRune()
        {
            Rune rune = new Rune.RuneBuilder().Set("Violent").Rarity("Hero").Build();

            Assert.AreEqual(rune.Type, REWARDTYPE.RUNE);
            Assert.AreEqual(rune.Set, RUNESET.VIOLENT);
            Assert.AreEqual(rune.Rarity, RARITY.HERO);
        }

        [Test]
        public void GetRune_GivenInvalidRuneSet_ReturnUnknownRune()
        {
            Rune rune = new Rune.RuneBuilder().Set("Some invalid set").Rarity("blah").Build();

            Assert.AreEqual(rune.Type, REWARDTYPE.RUNE);
            Assert.AreEqual(rune.Set, RUNESET.UNKNOWN);
            Assert.AreEqual(rune.Rarity, RARITY.UNKNOWN);
        }

        [Test]
        public void Test()
        {
            NoxEmulator emulator = new NoxEmulator();
            emulator.Click(new Point(1006, 594)); // Settings
        }
        
        [Test]
        public void TestPrintWindow()
        {
            NoxEmulator emulator = new NoxEmulator();

            IntPtr parent = AbstractEmulator.FindWindow("Qt5QWindowIcon", "Nox");
            IntPtr subWindow = AbstractEmulator.FindWindowEx(parent, IntPtr.Zero, "Qt5QWindowIcon", "ScreenBoardClassWindow");
            IntPtr mainWindow = AbstractEmulator.FindWindowEx(subWindow, IntPtr.Zero, "Qt5QWindowIcon", "QWidgetClassWindow");
            IntPtr sub = AbstractEmulator.FindWindowEx(mainWindow, IntPtr.Zero, "subWin", "sub");

            emulator.PrintWindow(parent);
        }

        [Test]
        public void TestMatchImage()
        {
            NoxEmulator emulator = new NoxEmulator();

            IntPtr parent = AbstractEmulator.FindWindow("Qt5QWindowIcon", "Nox");
            Bitmap source = emulator.PrintWindow(parent);

            Bitmap crop = BitmapUtils.CropImage(source, new Rectangle(800, 550, 400, 200));


            string test1 = @"C:\Users\Administrator\Desktop\1\dungeonEnergy.png";
            string test2 = @"E:\SWRunner\Resources\general\gift_box.png";
            Assert.AreEqual(1, QuizSolver.FindMatchImage(crop, new Bitmap(test2)));
        }
    }
}
