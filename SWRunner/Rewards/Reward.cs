using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SWRunner.Rewards
{
    public class Reward
    {
        public int Quantity { get; private set; }
        public RewardType Type { get; private set; }

        public string Drop { get; private set; }

        public Reward(string dropItem)
        {
            Drop = dropItem;
            SetType(dropItem);
            SetQuantity(dropItem);
        }

        private void SetType(string dropItem)
        {
            if (dropItem.Contains("Rune"))
            {
                Type = RewardType.RUNE;
            }
            else if (dropItem.Contains("Grindstone"))
            {
                Type = RewardType.GRINDSTONE;
            }
            else if (dropItem.Contains("Enchanted Gem"))
            {
                Type = RewardType.ENCHANTED_GEM;
            }
            else
            {
                Type = RewardType.OTHER;
            }
            
        }

        private void SetQuantity(string dropItem)
        {
            Quantity = 1;
            if (Type == RewardType.OTHER)
            {
                Match match = Regex.Match(dropItem, @"(.*\s)(x)(\d*)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Quantity = Int32.Parse(match.Groups[3].Value);
                }
            }
        }
    }

    public enum RewardType { RUNE, GRINDSTONE, ENCHANTED_GEM, OTHER}
}
