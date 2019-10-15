using SWEmulator;
using SWRunner.Rewards;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace SWRunner.Runners
{
    public abstract class AbstractRunner<T> : IRunner where T : AbstractRunnerConfig
    {
        public string LogFile { get; private set; }
        public T RunnerConfig { get; private set; }
        public AbstractEmulator Emulator { get; private set; }
        public DateTime ModifiedTime { get; protected set; }

        public int MinEnergyRequired { get; protected set; }
        public TimeSpan MaxRunTime { get; set; }

        public string FullLogFile { get; private set; }

        public bool Stop { get; set; } = false;

        public RunnerLogger Logger {get; private set;}

        public AbstractRunner(string logFile, string fullLogFile, T runnerConfig, AbstractEmulator emulator, RunnerLogger logger)
        {
            LogFile = logFile;
            FullLogFile = fullLogFile;
            Emulator = emulator;
            RunnerConfig = runnerConfig;
            Helper.UpdateRunConfig(emulator, runnerConfig);
            Logger = logger;
        }

        public void CheckRefill()
        {
            Debug.WriteLine("Checking for refill ...");
            bool needRefill = false;
            // TODO: check this 3 times for now
            for (int i = 0; i < 3; i++)
            {
                needRefill = NeedRefill();
                if (needRefill)
                {
                    break;
                }
                Thread.Sleep(1000);
                Debug.WriteLine("Checking for refill " + i + " ...");
            }

            if (NeedRefill())
            {
                Debug.WriteLine("Perform refill");
                Emulator.Click(RunnerConfig.OpenShopPoint);
                Helper.Sleep(2000, 3000);

                Emulator.Click(RunnerConfig.BuyEnergyWithCrystalPoint);
                Helper.Sleep(1000, 2000);

                Emulator.Click(RunnerConfig.ConfirmBuyPoint);
                Helper.Sleep(8000, 10000);

                Emulator.Click(RunnerConfig.BuyOKPoint);
                Helper.Sleep(1500, 2000);

                Emulator.Click(RunnerConfig.CloseShopPoint);
                Helper.Sleep(1500, 2000);

                Emulator.Click(RunnerConfig.ReplayPoint);
            }
            Debug.WriteLine("No need to refill");
        }

        public abstract void Collect();

        public bool IsEnd()
        {
            // Check last modification timestamp of log file
            DateTime lastModifiedTime = File.GetLastWriteTime(LogFile);
            if (lastModifiedTime > ModifiedTime)
            {
                ModifiedTime = lastModifiedTime;
                return true;
            }
            return false;
        }

        public bool IsFailed()
        {
            // Check last modification timestamp of log file
            //DateTime lastModifiedTime = File.GetLastWriteTime(LogFile);
            //return (DateTime.Now - ModifiedTime) > MaxRunTime;

            Bitmap screenShot = Emulator.PrintWindow();
            Bitmap crop = BitmapUtils.CropImage(screenShot, new Rectangle(500 * Emulator.Width / 1920, 550 * Emulator.Height / 1080,
                500 * Emulator.Width / 1920, 250 * Emulator.Height / 1080));
            return BitmapUtils.FindMatchImage(crop, new Bitmap(@"Resources\general\defeatCrystal.PNG"), 0.80f);
        }

        public abstract void Run();

        public void SkipRevive()
        {
            Thread.Sleep(1500);
            Debug.WriteLine("Skip revive");
            Emulator.Click(RunnerConfig.NoRevivePoint);

            Thread.Sleep(1000);
            Emulator.RandomClick();

            Thread.Sleep(1000);
        }

        public virtual void StartNewRun()
        {
            Thread.Sleep(3000);
            RandomSleep();
            Emulator.Click(RunnerConfig.ReplayPoint);

            Thread.Sleep(3500); // ensure refill window is pop up
            Debug.WriteLine("Checking for refill ...");
            CheckRefill();

            RandomSleep();
            Emulator.Click(RunnerConfig.StartPoint);
        }

        public bool NeedRefill()
        {
            Bitmap screenShot = Emulator.PrintWindow();
            Bitmap crop = BitmapUtils.CropImage(screenShot, new Rectangle(500 * Emulator.Width / 1920, 550 * Emulator.Height / 1080, 
                500 * Emulator.Width / 1920, 300 * Emulator.Height / 1080));
            return BitmapUtils.FindMatchImage(crop, new Bitmap(@"Resources\general\shop.PNG"), 0.78f);
            //return GetCurrentEnergy() < MinEnergyRequired;
        }

        public int GetCurrentEnergy()
        {
            int result = -1;
            string line = "";
            string temp = "";
            StreamReader file = new StreamReader(FullLogFile);
            while ((temp = file.ReadLine()) != null)
            {
                if (temp.Contains("Result"))
                {
                    line = temp;
                }
            }

            string pattern = @"(.*wizard_energy" + "\"" +":)" + @"(\d*)(.*)";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(line);
            if (match.Success)
            {
                Group group = match.Groups[2];
                result = Int32.Parse(group.Value);
            }

            file.Close();
            return result;
        }

        protected void RandomSleep()
        {
            int randomWaitTime = new Random().Next(200, 1000);
            Thread.Sleep(randomWaitTime);
        }

        public void StopRunner()
        {
            Stop = true;
        }

        public void ReadyRunner()
        {
            Stop = false;
        }
    }
}
