using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SWRunner.Rewards
{
    public class Reward
    {
        [XmlIgnore]
        public int Quantity { get; private set; }
        public REWARDTYPE Type { get; set; }

        [XmlIgnore]
        public string Drop { get; private set; }

        public Reward(string dropItem, REWARDTYPE type)
        {
            Drop = dropItem;
            Type = type;
            SetQuantity(dropItem);
        }

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
                Type = REWARDTYPE.RUNE;
            }
            else if (dropItem.Contains("Grindstone"))
            {
                Type = REWARDTYPE.GRINDSTONE;
            }
            else if (dropItem.Contains("Enchanted Gem"))
            {
                Type = REWARDTYPE.ENCHANTEDGEM;
            }
            else
            {
                Type = REWARDTYPE.OTHER;
            }
            
        }

        private void SetQuantity(string dropItem)
        {
            Quantity = 1;
            if (Type == REWARDTYPE.OTHER)
            {
                Match match = Regex.Match(dropItem, @"(.*\s)(x)(\d*)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Quantity = Int32.Parse(match.Groups[3].Value);
                }
            }
        }

        public override string ToString()
        {
            return Drop;
        }
    }

    public enum REWARDTYPE { RUNE, GRINDSTONE, ENCHANTEDGEM, SUMMONSTONE, MYSTICALSCROLL, OTHER}
}
