using AForge.Imaging;
using AForge.Imaging.Filters;
using CsvHelper;
using SWEmulator;
using SWRunner.Rewards;
using SWRunner.Runners;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace SWRunner
{
    public static class Helper
    {

        // Define quiz answer position
        private static List<Rectangle> answersPos = new List<Rectangle>()
        {
            new Rectangle(100, 100, 80, 80),
            new Rectangle(200, 100, 80, 80),
            new Rectangle(300, 100, 80, 80),
            new Rectangle(400, 100, 80, 80),
            new Rectangle(100, 200, 80, 80),
            new Rectangle(200, 200, 80, 80),
            new Rectangle(300, 200, 80, 80),
            new Rectangle(400, 200, 80, 80)
        };

        // Define quiz answer patterns
        private static Dictionary<string, string> answerPatterns = new Dictionary<string, string>()
        {
            {"fire only", "pattern" }
        };

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

        public static void UpdateRunConfig<T>(AbstractEmulator emulator, T runConfig) where T : AbstractRunnerConfig
        {
            FieldInfo[] fields = runConfig.GetType().GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(PointF))
                {
                    PointF point = (PointF)field.GetValue(runConfig);
                    point.X = emulator.Width * point.X;
                    point.Y = emulator.Height * point.Y;
                    field.SetValue(runConfig, point);
                }
            }
        }

        public static Reward GetReward(RunResult runResult)
        {
            Reward reward = null;

            REWARDTYPE type = GetRewardType(runResult.Drop);

            switch (type)
            {
                case REWARDTYPE.RUNE:
                    reward = new Rune.RuneBuilder().Grade(runResult.Grade).Set(runResult.Set).Slot(runResult.Slot).
                        Rarity(runResult.Rarity).MainStat(runResult.MainStat).PrefixStat(runResult.PrefixStat).
                        SubStat1(runResult.SubStat1).SubStat2(runResult.SubStat2).SubStat3(runResult.SubStat3).
                        SubStat4(runResult.SubStat4).Build();
                    break;
                case REWARDTYPE.GRINDSTONE:
                    reward = new Grindstone(runResult.Set, runResult.MainStat, runResult.SubStat1, runResult.SubStat2);
                    break;
                case REWARDTYPE.ENCHANTEDGEM:
                    reward = new EnchantedGem(runResult.Set, runResult.MainStat, runResult.SubStat1, runResult.SubStat2);
                    break;
                default:
                    //throw new NotImplementedException();
                    reward = new Reward(runResult.Drop, type);
                    break;
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

        public static int SolveQuiz(string pattern)
        {

            int matchedCounts = 0;

            string captchaDirectory = @"Resources\captcha\";

            DirectoryInfo d = new DirectoryInfo(captchaDirectory);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            // Create regex for certain question type
            //var regex = @"unit_icon_0032_1_1.png";

            
            for (int i = 1; i <= 8; i++)
            {
                Bitmap sourceImage = new Bitmap(@"C:\Test\sample" + i + ".png");
                sourceImage = ConvertToFormat(sourceImage, PixelFormat.Format24bppRgb);
                sourceImage = new ResizeBicubic((int)(sourceImage.Width * 0.4), (int)(sourceImage.Height * 0.4)).Apply(sourceImage);

                foreach (FileInfo file in Files)
                {
                    Match match = Regex.Match(file.Name, pattern, RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        continue;
                    }

                    //Bitmap sourceImage = new Bitmap(@"C:\Test\quiz_1920_1080.png");
                    Bitmap template = new Bitmap(file.FullName);

                    template = ConvertToFormat(template, PixelFormat.Format24bppRgb);
                    template = new ResizeBicubic((int)(template.Width * 0.4), (int)(template.Height * 0.4)).Apply(template);
                    ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.90f);

                    TemplateMatch[] matchings = tm.ProcessImage(sourceImage, template);

                    if (matchings.Length > 0)
                    {
                        matchedCounts += matchings.Length;
                    }
                }
                Debug.WriteLine(i);
            }

            return matchedCounts;
        }

        public static List<Bitmap> GetQuizImages(Bitmap screen)
        {
            List<Bitmap> answers = new List<Bitmap>();

            // Get each slot
            int i = 1;
            foreach (Rectangle cropRect in answersPos)
            {
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(screen, new Rectangle(0, 0, target.Width, target.Height),
                                     cropRect,
                                     GraphicsUnit.Pixel);
                }
                answers.Add(target);

                // Test
                target.Save("C:\\TestWin32\\" + i++ + ".png", ImageFormat.Png);
            }

            return answers;
        }

        public static string GetQuiz(string fullLogFile)
        {
            string result = "";
            string line = "";
            string temp = "";
            StreamReader file = new StreamReader(fullLogFile);
            while ((temp = file.ReadLine()) != null)
            {
                if (temp.Contains("Result"))
                {
                    line = temp;
                }
            }

            // TODO: Change pattern to get quiz question
            string pattern = "(.*wizard_energy\":)(\\d *)(.*)";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(line);
            if (match.Success)
            {
                Group group = match.Groups[2];
                result = group.Value;
            }

            file.Close();
            return result;
        }

        public static bool CheckRunFail()
        {
            Bitmap sourceImage = new Bitmap(@"C:\Test\sampleDefeated.png");

            using (Graphics gr = Graphics.FromImage(sourceImage)) // SourceImage is a Bitmap object
            {
                var gray_matrix = new float[][] {
                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                new float[] { 0,      0,      0,      1, 0 },
                new float[] { 0,      0,      0,      0, 1 }
            };

                var ia = new System.Drawing.Imaging.ImageAttributes();
                ia.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(gray_matrix));
                ia.SetThreshold(0.8f); // Change this threshold as needed
                var rc = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
                gr.DrawImage(sourceImage, rc, 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, ia);
            }

            return false;
        }

        public static void Sleep(int min, int max)
        {
            int sleepTime = new Random().Next(min, max);
            Thread.Sleep(sleepTime);
        }

        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private static Bitmap ConvertToFormat(Bitmap image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }

        private static REWARDTYPE GetRewardType(string dropItem)
        {
            REWARDTYPE type = REWARDTYPE.OTHER;

            if (dropItem.Contains("Rune") && !dropItem.Contains("Rune Piece"))
            {
                type = REWARDTYPE.RUNE;
            }
            else if (dropItem.Contains("Grindstone"))
            {
                type = REWARDTYPE.GRINDSTONE;
            }
            else if (dropItem.Contains("Enchanted Gem"))
            {
                type = REWARDTYPE.ENCHANTEDGEM;
            }
            else if (dropItem.Contains("Summoning Stone"))
            {
                type = REWARDTYPE.SUMMONSTONE;
            }
            else if (dropItem.Contains("Mystical Scroll"))
            {
                type = REWARDTYPE.MYSTICALSCROLL;
            }

            return type;
        }

    }
}
