using CsvHelper;
using SWEmulator;
using SWRunner.Runners;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace SWRunner.Rewards
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

        public static void UpdateRunConfig<T>(AbstractEmulator emulator, T runConfig) where T : RunConfig
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

        public static Reward GetReward(string path)
        {
            return null;
        }

        public static Reward GetReward(string path, int row)
        {
            return null;
        }

    }
}
