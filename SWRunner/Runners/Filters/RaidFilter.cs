using System;
using System.Collections.Generic;
using System.Text;
using SWRunner.Rewards;
using Rune = SWRunner.Rewards.Rune;

namespace SWRunner.Filters
{
    public class RaidFilter : IFilter
    {
        private List<GemStone> AcceptedGemStones { get; set; } = new List<GemStone>();

        public RaidFilter(List<GemStone> acceptedGemStones)
        {
            AcceptedGemStones = acceptedGemStones;
        }

        public bool ShouldGet(Reward reward)
        {
            if (typeof(GemStone).IsAssignableFrom(reward.GetType()))
            {
                return ShouldGetGemStone((GemStone)reward);
            }

            // Get other type
            return true;
        }

        private bool ShouldGetGemStone(GemStone gemStone)
        {
            return AcceptedGemStones.Contains(gemStone);
        }
    }
}
