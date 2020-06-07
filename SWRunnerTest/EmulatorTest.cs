using NUnit.Framework;
using SWEmulator;
using SWRunner;
using SWRunner.Runners;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunnerTest
{
    [TestFixture]
    class EmulatorTest
    {
        [Test]
        public void BlueStack_PrintScreen()
        {
            BlueStacksEmulator emulator = new BlueStacksEmulator();
            emulator.PrintWindow();
        }

        [Test]
        public void Nox_PrintScreen()
        {
            NoxEmulator emulator = new NoxEmulator();
            emulator.PrintWindow();
        }

        [Test]
        public void BlueStacks_Carios_Cancel()
        {
            AbstractEmulator emulator = new BlueStacksEmulator();

            CairosRunnerConfig config = new CairosRunnerConfig();
            config.SellRunePoint = new System.Drawing.PointF(0.817f, 0.807f);

            Helper.UpdateRunConfig(emulator, config);
            emulator.Click(config.SellRunePoint);
        }

        [Test]
        public void BlueStacks_Esc()
        {
            BlueStacksEmulator emulator = new BlueStacksEmulator();
            emulator.PressEsc(); 
            emulator.PressEsc(); 
            emulator.PressEsc(); 
        }

        [Test]
        public void Nox_Esc()
        {
            NoxEmulator emulator = new NoxEmulator();
            emulator.PressEsc();
        }
    }
}
