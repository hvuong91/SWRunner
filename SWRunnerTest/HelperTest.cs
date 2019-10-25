using NUnit.Framework;
using SWEmulator;
using SWRunner;
using SWRunner.Filters;
using SWRunner.Rewards;
using SWRunner.Runners;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace SWRunnerTest
{
    [TestFixture]
    public class HelperTest
    {
        [Test]
        public void Helper_GivenRunConfigAndEmulator_ProcessAllClickPoints()
        {

            CairosRunnerConfig runConfig = new CairosRunnerConfig();

            // Basic
            runConfig.StartPoint = new PointF(0.841f, 0.710f);
            runConfig.ReplayPoint = new PointF(0.323f, 0.533f);
            runConfig.NoRevivePoint = new PointF(0.651f, 0.666f);

            // Refill
            runConfig.OpenShopPoint = new PointF(0.401f, 0.615f);
            runConfig.BuyEnergyWithCrystalPoint = new PointF(0.423f, 0.525f);
            runConfig.ConfirmBuyPoint = new PointF(0.418f, 0.606f);
            runConfig.BuyOKPoint = new PointF(0.502f, 0.599f);
            runConfig.CloseShopPoint = new PointF(0.499f, 0.868f);

            // TODO: Captcha

            // Cairos config
            runConfig.GetRunePoint = new PointF(0.584f, 0.803f);
            runConfig.SellRunePoint = new PointF(0.417f, 0.807f);
            runConfig.ConfirmSellRunePoint = new PointF(0.408f, 0.595f);

            runConfig.GetMysticalScrollPoint = new PointF(0.499f, 0.767f);
            runConfig.GetOtherPoint = new PointF(0.499f, 0.829f);

            XmlSerializer writer =
            new XmlSerializer(typeof(AbstractRunnerConfig));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//TestConfig.xml";
            FileStream file = File.Create(path);

            writer.Serialize(file, runConfig);
            file.Close();
        }

        [Test]
        public void TestGemStoneListToXml()
        {
            //List<GemStone> items = new List<GemStone>();
            //items.Add(new Grindstone("Violent", "%HP", "4", "7"));
            //items.Add(new Grindstone("Blade", "%HP", "4", "7"));
            //items.Add(new Grindstone("Rage", "%HP", "4", "7"));
            //items.Add(new Grindstone("Endure", "Flat HP", "100", "200"));
            //items.Add(new Grindstone("Destroy", "SPD", "4", "5"));
            //items.Add(new Grindstone("Rage", "%DEF", "5", "10"));

            GemStoneFilter filter = new GemStoneFilter();

            filter.GemStoneList.Add(new GrindStone("Violent", "%HP", "4", "7"));
            filter.GemStoneList.Add(new GrindStone("Blade", "%HP", "4", "7"));
            filter.GemStoneList.Add(new GrindStone("Rage", "%HP", "4", "7"));
            filter.GemStoneList.Add(new GrindStone("Endure", "Flat HP", "100", "200"));
            filter.GemStoneList.Add(new GrindStone("Destroy", "SPD", "4", "5"));
            filter.GemStoneList.Add(new GrindStone("Rage", "%DEF", "5", "10"));

            XmlSerializer writer =
           new XmlSerializer(typeof(GemStoneFilter));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//GemStoneFilter.xml";
            FileStream file = File.Create(path);

            writer.Serialize(file, filter);
            file.Close();

        }

        [Test]
        public void Test()
        {
            NoxEmulator emulator = new NoxEmulator();
            emulator.Width = 1144;
            emulator.Height = 644;

            XmlSerializer serializer = new XmlSerializer(typeof(CairosRunnerConfig), new XmlRootAttribute("RunConfig"));

            // Declare an object variable of the type to be deserialized.
            CairosRunnerConfig runConfig;

            string testConfigXml = @"TestData/TestConfig.xml";

            using (Stream reader = new FileStream(testConfigXml, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                runConfig = (CairosRunnerConfig)serializer.Deserialize(reader);
            }

            Helper.UpdateRunConfig(emulator, runConfig);

            Assert.IsNotNull(runConfig);

            Assert.AreEqual((int)runConfig.ReplayPoint.X, 369);
            Assert.AreEqual((int)runConfig.ReplayPoint.Y, 343);
        }

        [Test]
        public void SolveQuizTest()
        {
            // Fire
            string firePattern = @"unit_icon_(.)*_1_\d.png";
            Assert.AreEqual(2, Helper.SolveQuiz(firePattern));

            // Wind
            string windPattern = @"unit_icon_(.)*_2_\d.png";
            Assert.AreEqual(5, Helper.SolveQuiz(windPattern));

            // Water
            string waterPattern = @"unit_icon_(.)*_3_\d.png";
            Assert.AreEqual(1, Helper.SolveQuiz(waterPattern));
        }

        [Test]
        public void TestPrintScreen()
        {
            NoxEmulator emulator = new NoxEmulator();
            IntPtr paintHwnd = AbstractEmulator.FindWindow(null, "Untitled - Paint");

            Bitmap src = emulator.PrintWindow(paintHwnd);

            Rectangle cropRect = new Rectangle(100, 100, 80, 80);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }
            target.Save("C:\\TestWin32\\test1.png", ImageFormat.Png);

            Helper.GetQuizImages(src);
        }
    }
}
