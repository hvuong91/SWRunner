using NUnit.Framework;
using SWRunner.Filters;
using SWRunner.Rewards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using static SWRunner.Rewards.Rune;

namespace SWRunnerTest
{
    [TestFixture]
    class StoneGemFilterTest
    {
        private List<GemStone> acceptedGemStones;
        private RiftFilter riftFilter;

        [SetUp]
        public void init()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GemStoneFilter), new XmlRootAttribute("GemStoneFilter"));

            // Declare an object variable of the type to be deserialized.
            string gemStoneFilterXml = @"TestData/GemStoneFilter.xml";

            using (Stream reader = new FileStream(gemStoneFilterXml, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                acceptedGemStones = ((GemStoneFilter)serializer.Deserialize(reader)).GemStoneList;
            }

            riftFilter = new RiftFilter(acceptedGemStones);
        }

        [Test]
        public void TestViolentGrindStoneFilter()
        {
            // Accept HP%
            GrindStone rareHPGrindStone = new GrindStone(RUNESET.VIOLENT, "HP%", RARITY.RARE);
            GrindStone magicHPGrindStone = new GrindStone(RUNESET.VIOLENT, "HP%", RARITY.MAGIC);
            GrindStone heroHPGrindStone = new GrindStone(RUNESET.VIOLENT, "HP%", RARITY.HERO);
            GrindStone legendaryHPGrindStone = new GrindStone(RUNESET.VIOLENT, "HP%", RARITY.LEGENDARY);
            Assert.IsTrue(riftFilter.ShouldGet(magicHPGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(rareHPGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(heroHPGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(legendaryHPGrindStone));

            // Accept DEF%
            GrindStone rareDEFGrindStone = new GrindStone(RUNESET.VIOLENT, "DEF%", RARITY.RARE);
            GrindStone magicDEFGrindStone = new GrindStone(RUNESET.VIOLENT, "DEF%", RARITY.MAGIC);
            GrindStone heroDEFGrindStone = new GrindStone(RUNESET.VIOLENT, "DEF%", RARITY.HERO);
            GrindStone legendaryDEFGrindStone = new GrindStone(RUNESET.VIOLENT, "DEF%", RARITY.LEGENDARY);
            Assert.IsTrue(riftFilter.ShouldGet(rareDEFGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(magicDEFGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(heroDEFGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(legendaryDEFGrindStone));

            // Accept SPD
            GrindStone rareSPDGrindStone = new GrindStone(RUNESET.VIOLENT, "SPD", RARITY.RARE);
            GrindStone magicSPDGrindStone = new GrindStone(RUNESET.VIOLENT, "SPD", RARITY.MAGIC);
            GrindStone heroSPDGrindStone = new GrindStone(RUNESET.VIOLENT, "SPD", RARITY.HERO);
            GrindStone legendarySPDGrindStone = new GrindStone(RUNESET.VIOLENT, "SPD", RARITY.LEGENDARY);
            Assert.IsTrue(riftFilter.ShouldGet(rareSPDGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(magicSPDGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(heroSPDGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(legendarySPDGrindStone));

            // Accept other hero/legendary grindstone
            GrindStone heroFlatHPGrindStone = new GrindStone(RUNESET.VIOLENT, "HP flat", RARITY.HERO);
            GrindStone legendaryFlatHPGrindStone = new GrindStone(RUNESET.VIOLENT, "DEF flat", RARITY.LEGENDARY);
            Assert.IsTrue(riftFilter.ShouldGet(heroFlatHPGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(legendaryFlatHPGrindStone));

            GrindStone magicFlatHPGrindStone = new GrindStone(RUNESET.VIOLENT, "HP flat", RARITY.MAGIC);
            Assert.IsFalse(riftFilter.ShouldGet(magicFlatHPGrindStone));
        }

        [Test]
        public void TestViolentEnchantedGemFilter()
        {
            // Take rare HP%, DEF%, SPD
            EnchantedGem rareHPEnchantedGem = new EnchantedGem(RUNESET.VIOLENT, "HP%", RARITY.RARE);
            EnchantedGem rareDEFEnchantedGem = new EnchantedGem(RUNESET.VIOLENT, "DEF%", RARITY.RARE);
            EnchantedGem rareSPDEnchantedGem = new EnchantedGem(RUNESET.VIOLENT, "SPD", RARITY.RARE);
            Assert.IsTrue(riftFilter.ShouldGet(rareHPEnchantedGem));
            Assert.IsTrue(riftFilter.ShouldGet(rareDEFEnchantedGem));
            Assert.IsTrue(riftFilter.ShouldGet(rareSPDEnchantedGem));

            // Take hero and legend for all
            EnchantedGem heroCRateEnchantedGem = new EnchantedGem(RUNESET.VIOLENT, "CRate", RARITY.HERO);
            EnchantedGem lendaryCDmgEnchantedGem = new EnchantedGem(RUNESET.VIOLENT, "CDmg", RARITY.LEGENDARY);
            EnchantedGem heroAccEnchantedGem = new EnchantedGem(RUNESET.VIOLENT, "ACC", RARITY.HERO);
            Assert.IsTrue(riftFilter.ShouldGet(heroCRateEnchantedGem));
            Assert.IsTrue(riftFilter.ShouldGet(lendaryCDmgEnchantedGem));
            Assert.IsTrue(riftFilter.ShouldGet(heroAccEnchantedGem));

            // Ignore trash stuffs
            EnchantedGem rareRESEnchantedGem = new EnchantedGem(RUNESET.VIOLENT, "RES", RARITY.RARE);
            EnchantedGem rareHPFlatEnchantedGem = new EnchantedGem(RUNESET.VIOLENT, "HP flat", RARITY.RARE);
            Assert.IsFalse(riftFilter.ShouldGet(rareRESEnchantedGem));
            Assert.IsFalse(riftFilter.ShouldGet(rareHPFlatEnchantedGem));
        }

        [Test]
        public void TestWillGrindStoneFilter()
        {
            // Accept HP%
            GrindStone rareWillHPGrindStone = new GrindStone(RUNESET.WILL, "HP%", RARITY.RARE);
            GrindStone magicWillHPGrindStone = new GrindStone(RUNESET.WILL, "HP%", RARITY.MAGIC);
            GrindStone heroHPGrindStone = new GrindStone(RUNESET.WILL, "HP%", RARITY.HERO);
            GrindStone legendaryHPGrindStone = new GrindStone(RUNESET.WILL, "HP%", RARITY.LEGENDARY);
            Assert.IsTrue(riftFilter.ShouldGet(magicWillHPGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(rareWillHPGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(heroHPGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(legendaryHPGrindStone));

            // Accept DEF%
            GrindStone rareDEFGrindStone = new GrindStone(RUNESET.WILL, "DEF%", RARITY.RARE);
            GrindStone magicDEFGrindStone = new GrindStone(RUNESET.WILL, "DEF%", RARITY.MAGIC);
            GrindStone heroDEFGrindStone = new GrindStone(RUNESET.WILL, "DEF%", RARITY.HERO);
            GrindStone legendaryDEFGrindStone = new GrindStone(RUNESET.WILL, "DEF%", RARITY.LEGENDARY);
            Assert.IsTrue(riftFilter.ShouldGet(rareDEFGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(magicDEFGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(heroDEFGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(legendaryDEFGrindStone));

            // Accept SPD
            GrindStone rareSPDGrindStone = new GrindStone(RUNESET.WILL, "SPD", RARITY.RARE);
            GrindStone magicSPDGrindStone = new GrindStone(RUNESET.WILL, "SPD", RARITY.MAGIC);
            GrindStone heroSPDGrindStone = new GrindStone(RUNESET.WILL, "SPD", RARITY.HERO);
            GrindStone legendarySPDGrindStone = new GrindStone(RUNESET.WILL, "SPD", RARITY.LEGENDARY);
            Assert.IsTrue(riftFilter.ShouldGet(rareSPDGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(magicSPDGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(heroSPDGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(legendarySPDGrindStone));

            // Accept other hero/legendary grindstone
            GrindStone heroFlatHPGrindStone = new GrindStone(RUNESET.WILL, "HP flat", RARITY.HERO);
            GrindStone legendaryFlatHPGrindStone = new GrindStone(RUNESET.WILL, "DEF flat", RARITY.LEGENDARY);
            Assert.IsTrue(riftFilter.ShouldGet(heroFlatHPGrindStone));
            Assert.IsTrue(riftFilter.ShouldGet(legendaryFlatHPGrindStone));

            GrindStone magicFlatHPGrindStone = new GrindStone(RUNESET.WILL, "HP flat", RARITY.MAGIC);
            Assert.IsFalse(riftFilter.ShouldGet(magicFlatHPGrindStone));
        }

        [Test]
        public void TestWillEnchantedGemFilter()
        {
            // Take rare HP%, DEF%, SPD
            EnchantedGem rareHPEnchantedGem = new EnchantedGem(RUNESET.WILL, "HP%", RARITY.RARE);
            EnchantedGem rareDEFEnchantedGem = new EnchantedGem(RUNESET.WILL, "DEF%", RARITY.RARE);
            EnchantedGem rareSPDEnchantedGem = new EnchantedGem(RUNESET.WILL, "SPD", RARITY.RARE);
            Assert.IsTrue(riftFilter.ShouldGet(rareHPEnchantedGem));
            Assert.IsTrue(riftFilter.ShouldGet(rareDEFEnchantedGem));
            Assert.IsTrue(riftFilter.ShouldGet(rareSPDEnchantedGem));

            // Take hero and legend for all
            EnchantedGem heroCRateEnchantedGem = new EnchantedGem(RUNESET.WILL, "CRate", RARITY.HERO);
            EnchantedGem lendaryCDmgEnchantedGem = new EnchantedGem(RUNESET.WILL, "CDmg", RARITY.LEGENDARY);
            EnchantedGem heroAccEnchantedGem = new EnchantedGem(RUNESET.WILL, "ACC", RARITY.HERO);
            Assert.IsTrue(riftFilter.ShouldGet(heroCRateEnchantedGem));
            Assert.IsTrue(riftFilter.ShouldGet(lendaryCDmgEnchantedGem));
            Assert.IsTrue(riftFilter.ShouldGet(heroAccEnchantedGem));

            // Ignore trash stuffs
            EnchantedGem rareRESEnchantedGem = new EnchantedGem(RUNESET.WILL, "RES", RARITY.RARE);
            EnchantedGem rareHPFlatEnchantedGem = new EnchantedGem(RUNESET.WILL, "HP flat", RARITY.RARE);
            Assert.IsFalse(riftFilter.ShouldGet(rareRESEnchantedGem));
            Assert.IsFalse(riftFilter.ShouldGet(rareHPFlatEnchantedGem));
        }

        [Test]
        public void TestGetOther()
        {
            GrindStone heroDespairHPGrindStone = new GrindStone(RUNESET.DESPAIR, "HP%", RARITY.HERO);
            Assert.IsTrue(riftFilter.ShouldGet(heroDespairHPGrindStone));

            GrindStone legendaryEnergyDEFFlatGrindStone = new GrindStone(RUNESET.ENERGY, "DEF flat", RARITY.LEGENDARY);
            Assert.IsTrue(riftFilter.ShouldGet(legendaryEnergyDEFFlatGrindStone));

        }
    }
}
