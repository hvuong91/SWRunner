using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Rewards
{
    public class RunResult
    {
        private Reward reward;

        public Reward GetReward()
        {
            if (reward == null)
            {
                // Construct reward
                reward = new Reward(Drop);
            }

            return reward;
        }

        public DateTime Date { get; set; }

        [BooleanTrueValues("win")]
        [BooleanFalseValues("lose")]
        public bool Result { get; set; }

        public DateTime Time { get; set; }

        public string Drop { get; set; }

        public string Grade { get; set; }

        public string Set { get; set; }

        public string Efficiency { get; set; }

        public string Slot { get; set; }

        public string Rarity { get; set; }

        [Index(14)]
        public string MainStat { get; set; }

        [Index(15)]
        public string PrefixStat { get; set; }

        [Index(16)]
        public string SubStat1 { get; set; }

        [Index(17)]
        public string SubStat2 { get; set; }

        [Index(18)]
        public string SubStat3 { get; set; }

        [Index(19)]
        public string SubStat4 { get; set; }

    }

}
