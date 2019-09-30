using CsvHelper;
using System.Collections.Generic;
using System.IO;

namespace SWRunner.Rewards
{
    public static class RunParser
    {
        private static IEnumerable<RunResult> records;

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
