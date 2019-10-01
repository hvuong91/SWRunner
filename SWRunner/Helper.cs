using CsvHelper;
using SWEmulator;
using SWRunner.Rewards;
using SWRunner.Runners;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace SWRunner
{
    public static class Helper
    {
        public static RunResult GetRunResult(string runsPath)
        {
            RunResult result = null;

            using (var reader = new StreamReader(runsPath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.PrepareHeaderForMatch = (header, index) => header.ToLower();
                var records = csv.GetRecords<RunResult>();
                foreach (var record in records)
                {
                    result = (RunResult)record;
                }
            }

            return result;
        }

        public static void UpdateRunConfig<T>(AbstractEmulator emulator, T runConfig) where T : RunnerConfig
        {
            FieldInfo[] fields = runConfig.GetType().GetFields();

            int configW = runConfig.Width;
            int configH = runConfig.Height;

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(Point))
                {
                    Point point = (Point)field.GetValue(runConfig);
                    point.X = emulator.Width * point.X / configW;
                    point.Y = emulator.Height * point.Y / configH;
                    field.SetValue(runConfig, point);
                }
            }
        }

        public static Reward GetReward(RunResult runResult)
        {
            Reward reward = null;

            RewardType type = GetRewardType(runResult.Drop);

            switch (type)
            {
                case RewardType.RUNE:
                    reward = new Rune.RuneBuilder().Grade(runResult.Grade).Set(runResult.Set).Slot(runResult.Slot).
                        Rarity(runResult.Rarity).MainStat(runResult.MainStat).PrefixStat(runResult.PrefixStat).
                        SubStat1(runResult.SubStat1).SubStat2(runResult.SubStat2).SubStat3(runResult.SubStat3).
                        SubStat4(runResult.SubStat4).Build();
                    break;
                case RewardType.GRINDSTONE:
                    // TODO
                case RewardType.ENCHANTED_GEM:
                    // TODO
                case RewardType.OTHER:
                    throw new NotImplementedException();
            }

            return reward;
        }
        
        public static float CompareImageDifference(Bitmap img1, Bitmap img2)
        {
            if (img1.Size != img2.Size)
            {
                Console.Error.WriteLine("Images are of different sizes");
                return 100;
            }

            float diff = 0;

            for (int y = 0; y < img1.Height; y++)
            {
                for (int x = 0; x < img1.Width; x++)
                {
                    Color pixel1 = img1.GetPixel(x, y);
                    Color pixel2 = img2.GetPixel(x, y);

                    diff += Math.Abs(pixel1.R - pixel2.R);
                    diff += Math.Abs(pixel1.G - pixel2.G);
                    diff += Math.Abs(pixel1.B - pixel2.B);
                }
            }

            return 100 * (diff / 255) / (img1.Width * img1.Height * 3);
        }

        private static RewardType GetRewardType(string dropItem)
        {
            RewardType type = RewardType.OTHER;

            if (dropItem.Contains("Rune"))
            {
                type = RewardType.RUNE;
            }
            else if (dropItem.Contains("Grindstone"))
            {
                type = RewardType.GRINDSTONE;
            }
            else if (dropItem.Contains("Enchanted Gem"))
            {
                type = RewardType.ENCHANTED_GEM;
            }

            return type;
        }

    }
}
