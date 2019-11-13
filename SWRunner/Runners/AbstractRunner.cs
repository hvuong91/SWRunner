using SWEmulator;
using SWRunner.Rewards;
using SWRunnerApp;
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
            if (NeedRefill())
            {
                Debug.WriteLine("Perform refill");
                Emulator.Click(RunnerConfig.OpenShopPoint);
                Helper.Sleep(2000, 3000);

                Emulator.Click(RunnerConfig.BuyEnergyWithCrystalPoint);
                Helper.Sleep(1000, 2000);

                string pattern = TryGetQuizPattern();
                if (!string.IsNullOrEmpty(pattern))
                {
                    while (!string.IsNullOrEmpty(pattern))
                    {
                        Debug.Write("Try to solve: " + pattern);
                        QuizSolver.SolveQuiz(Emulator);
                        Thread.Sleep(3000);
                        Emulator.PressEsc();
                        Thread.Sleep(1000);
                        pattern = TryGetQuizPattern();
                    }
                }


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

        private string TryGetQuizPattern()
        {
            string pattern = string.Empty;
            for (int i = 0; i < 3; i++)
            {
                pattern = QuizSolver.GetQuizPattern(Emulator.PrintWindow(), Emulator.Width, Emulator.Height); 
                if (!string.IsNullOrEmpty(pattern))
                {
                    break;
                }
            }

            return pattern;
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

        public virtual bool IsFailed()
        {
            // Check last modification timestamp of log file
            //DateTime lastModifiedTime = File.GetLastWriteTime(LogFile);
            //return (DateTime.Now - ModifiedTime) > MaxRunTime;

            bool failed = false;
            for (int i = 0; i < 3; i++)
            {
                Bitmap screenShot = Emulator.PrintWindow();
                Bitmap crop = BitmapUtils.CropImage(screenShot, new Rectangle(500 * Emulator.Width / 1920, 550 * Emulator.Height / 1080,
                    500 * Emulator.Width / 1920, 300 * Emulator.Height / 1080));
                failed = BitmapUtils.FindMatchImage(crop, new Bitmap(@"Resources\general\defeatCrystal2.PNG"), 0.82f);
                Thread.Sleep(300);
                if (failed)
                {
                    break;
                }
            }
            return failed;
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
            Thread.Sleep(2000);
            RandomSleep();
            Emulator.Click(RunnerConfig.ReplayPoint);

            Thread.Sleep(3500); // ensure refill window is pop up
            CheckRefill();

            RandomSleep();
            Emulator.Click(RunnerConfig.StartPoint);
        }

        public bool NeedRefill()
        {
            bool needRefill = false;

            for (int i = 0; i < 3; i++)
            {
                Bitmap screenShot = Emulator.PrintWindow();
                Bitmap crop = BitmapUtils.CropImage(screenShot, new Rectangle(500 * Emulator.Width / 1920, 550 * Emulator.Height / 1080,
                    700 * Emulator.Width / 1920, 400 * Emulator.Height / 1080));
                needRefill = BitmapUtils.FindMatchImage(crop, new Bitmap(@"Resources\general\shop.PNG"), 0.82f);
                if (needRefill)
                {
                    break;
                }
                Thread.Sleep(500);
            }

            return needRefill;
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
