using SWRunner.Rewards;
using System.Collections.Generic;

namespace SWRunner.Filters
{
    public class RiftFilter : IFilter
    {
        private List<GemStone> AcceptedGemStones { get; set; } = new List<GemStone>();

        public RiftFilter(List<GemStone> acceptedGemStones)
        {
            AcceptedGemStones = acceptedGemStones;
        }

        public bool ShouldGet(Reward reward)
        {
            if (reward.GetType() == typeof(Rune))
            {
                return ShouldGetRune((Rune)reward);
            }
            else if (typeof(GemStone).IsAssignableFrom(reward.GetType()))
            {
                return ShouldGetGemStone((GemStone)reward);
            }

            // Get other type
            return true;
        }

        private bool ShouldGetRune(Rune rune)
        {
            // only get legendary rune
            if (!IsLegendary(rune))
            {
                return false;
            }

            // Ignore flat rune
            if (IsFlat246(rune) && !IsSlot2Speed(rune))
            {
                return false;
            }

            return true;
        }

        private bool ShouldGetGemStone(GemStone gemStone)
        {
            return AcceptedGemStones.Contains(gemStone);
        }

        private bool IsLegendary(Rune rune)
        {
            return rune.Rarity == Rune.RARITY.LEGENDARY;
        }

        private bool IsFlat246(Rune rune)
        {
            return Is246(rune) && !rune.MainStat.Contains("%");
        }

        private static bool Is246(Rune rune)
        {
            return rune.Slot.Equals("2") || rune.Slot.Equals("4") || rune.Slot.Equals("6");
        }

        private bool IsSlot2Speed(Rune rune)
        {
            return rune.Slot.Equals("2") && rune.MainStat.Contains("SPD");
        }
    }
}
