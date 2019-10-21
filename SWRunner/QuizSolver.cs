using AForge.Imaging;
using AForge.Imaging.Filters;
using SWEmulator;
using SWRunner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SWRunnerApp
{
    /// <summary>
    /// This class contains the logic for solving the quiz.
    /// </summary>
    public static class QuizSolver
    {
        private static readonly string CAPTCHA_DIR = @"Resources\captcha\";
        private static readonly string CAPTCHA_POPUP_DIR = @"Resources\captchaPopup";

        private static readonly int BASE_WIDTH = 1920;
        private static readonly int BASE_HEIGHT = 1080;

        // Quiz pattern
        public static readonly string ALL_WATER = @"unit_icon_(.)*_0_\d.png";
        public static readonly string EXCLUDE_WATER = "exclude water";

        public static readonly string ALL_FIRE = @"unit_icon_(.)*_1_\d.png";
        public static readonly string EXCLUDE_FIRE = "^(?!(" + ALL_FIRE + ")).*";

        public static readonly string ALL_WIND = @"unit_icon_(.)*_2_\d.png";
        public static readonly string EXCLUDE_WIND = "exclude wind";

        public static readonly string ANGELMON = @"(unit_icon_0016_\d_4.*)|(unit_icon_0019_\d_0.*)|(unit_icon_0030_.*)";
        public static readonly string EXCLUDE_ANGELMON = "exclude angelmon";

        public static readonly string CAIROS = @"(gray_)?(unit_icon_0008_\d_1.*)|(gray_)?(unit_icon_0008_\d_2.*)|(gray_)?(unit_icon_0008_\d_0.*)|(gray_)?(unit_icon_0035_\d_1.*)";
        public static readonly string EXCLUDE_CAIROS = "exclude cairos";

        public static readonly string BOSS = @"(gray_)?(unit_icon_0008_\d_1.*)|(gray_)?(unit_icon_0008_\d_2.*)|(gray_)?(unit_icon_0008_\d_0.*)|(gray_)?(unit_icon_0035_\d_1.*)|(gray_)?(unit_icon_0051_\d_4.*)|(gray_)?(unit_icon_0034_0_3.*)|(gray_)?(unit_icon_0042_\d_3.*)|(gray_)?(unit_icon_0045_\d_4.*)|(gray_)?(unit_icon_0012_4_3.*)|(gray_)?(unit_icon_0019_\d_4.*)";
        public static readonly string EXCLUDE_BOSS = "exclude boss";

        public static readonly string ELLIA = @"(gray_)?irene.*";
        public static readonly string EXCLUDE_ELLIA = "exclude ellia";

        private static readonly Dictionary<int, Rectangle> ANSWER_POS_DICT = new Dictionary<int, Rectangle>()
        {
            {1, new Rectangle(530,400,230,230) },
            {2, new Rectangle(750,400,230,230) },
            {3, new Rectangle(970,400,230,230) },
            {4, new Rectangle(1190,400,230,230) },

            {5, new Rectangle(530,600,230,230) },
            {6, new Rectangle(750,600,230,230) },
            {7, new Rectangle(970,600,230,230) },
            {8, new Rectangle(1190,600,230,230) }
        };

        public static void SolveQuiz(AbstractEmulator emulator)
        {
            // Capture screen with quiz 
            Bitmap screen = emulator.PrintWindow();
            //screen = ConvertToFormat(screen, PixelFormat.Format24bppRgb);

            string quizPattern = GetQuizPattern(screen, emulator.Width, emulator.Height);

            if (string.IsNullOrEmpty(quizPattern))
            {
                Debug.WriteLine("No Quiz found");
                return;
            }

            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = GetAnswer(screen, i);
                if (IsCorrectAnswer(answer, quizPattern, screen.Width, screen.Height))
                {
                    Debug.WriteLine("Found answer at pos: " + i);
                    emulator.Click(answer.point);
                }
            }

            // TODO: Put this in config settings
            emulator.Click(new Point((int)(emulator.Width * 0.501), (int)(emulator.Height * 0.835)));
        }
        
        // The following methods should be private.

        public static string GetQuizPattern(Bitmap screen, int width, int height)
        {
            string pattern = String.Empty;
            double scale = 0.8;

            Rectangle rec = new Rectangle((int)(400 * width / BASE_WIDTH),
                                          (int)(250 * height / BASE_HEIGHT),
                                          (int)(1200 * width / BASE_WIDTH),
                                          (int)(150 * height / BASE_HEIGHT));


            Bitmap questionImg = QuizSolver.CropImage(screen, rec);
            questionImg = ConvertToFormat(questionImg, PixelFormat.Format24bppRgb);
            questionImg = new ResizeBicubic((int)(questionImg.Width * scale), (int)(questionImg.Height * scale)).Apply(questionImg);


            DirectoryInfo d = new DirectoryInfo(CAPTCHA_POPUP_DIR);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            foreach (FileInfo file in Files)
            {
                Bitmap template = new Bitmap(file.FullName);
                template = ConvertToFormat(template, PixelFormat.Format24bppRgb);
                // resize to match the source image
                template = new ResizeBicubic(template.Width * width / BASE_WIDTH, template.Height * height / BASE_HEIGHT).Apply(template);
                template = new ResizeBicubic((int)(template.Width * scale), (int)(template.Height * scale)).Apply(template);

                ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.87f);

                TemplateMatch[] matchings = tm.ProcessImage(questionImg, template);
                if (matchings.Length > 0)
                {
                    pattern += file.Name + " ";
                }
            }

            if (string.IsNullOrEmpty(pattern))
            {
                return pattern;
            }

           if (pattern.Contains("water"))
            {
                if (pattern.Contains("excluding"))
                {
                    pattern = EXCLUDE_WATER;
                }
                else
                {
                    pattern = ALL_WATER;
                }
            }
            else if (pattern.Contains("fire"))
            {
                if (pattern.Contains("excluding"))
                {
                    pattern = EXCLUDE_FIRE;
                }
                else
                {
                    pattern = ALL_FIRE;
                }
            }
            else if (pattern.Contains("wind"))
            {
                if (pattern.Contains("excluding"))
                {
                    pattern = EXCLUDE_WIND;
                }
                else
                {
                    pattern = ALL_WIND;
                }
            }
            else if (pattern.Contains("angelmon"))
            {
                if (pattern.Contains("excluding"))
                {
                    pattern = EXCLUDE_ANGELMON;
                }
                else
                {
                    pattern = ANGELMON;
                }
            }
            else if (pattern.Contains("cairos"))
            {
                if (pattern.Contains("excluding"))
                {
                    pattern = EXCLUDE_CAIROS;
                }
                else
                {
                    pattern = CAIROS;
                }
            }
            else if (pattern.Contains("boss"))
            {
                if (pattern.Contains("excluding"))
                {
                    pattern = EXCLUDE_BOSS;
                }
                else
                {
                    pattern = BOSS;
                }
            }
            else if (pattern.Contains("ellia"))
            {
                if (pattern.Contains("excluding"))
                {
                    pattern = EXCLUDE_ELLIA;
                }
                else
                {
                    pattern = ELLIA;
                }
            }

            return pattern;
        }

        public static (Point point, Bitmap img) GetAnswer(Bitmap screen, int answerNum)
        {
            Rectangle rec = ANSWER_POS_DICT[answerNum];

            rec.X = rec.X * screen.Width / BASE_WIDTH;
            rec.Y = rec.Y * screen.Height / BASE_HEIGHT;
            rec.Width = rec.Width * screen.Width / BASE_WIDTH;
            rec.Height = rec.Height * screen.Height / BASE_HEIGHT;

            Bitmap answerImg = QuizSolver.CropImage(screen, rec);
            answerImg.Save("C:\\TestWin32\\crop" + answerNum + ".png", ImageFormat.Png);


            // TODO: calculate point to click
            return (new Point(rec.X + rec.Width / 2, rec.Y + rec.Height / 2), answerImg);
        }

        public static bool IsCorrectAnswer((Point point, Bitmap img) answer, string pattern, int w, int h)
        {

            float scale = 0.30f;
            answer.img = ConvertToFormat(answer.img, PixelFormat.Format24bppRgb);
            answer.img = new ResizeBicubic((int)(answer.img.Width * scale), (int)(answer.img.Height * scale)).Apply(answer.img);

            DirectoryInfo d = new DirectoryInfo(CAPTCHA_DIR);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files


            bool excluded = false;
            // Revert search for exclude question
            if (pattern == EXCLUDE_FIRE)
            {
                pattern = ALL_FIRE;
                excluded = true;
            }
            else if (pattern == EXCLUDE_WIND)
            {
                pattern = ALL_WIND;
                excluded = true;
            }
            else if (pattern == EXCLUDE_WATER)
            {
                pattern = ALL_WATER;
                excluded = true;
            }
            else if (pattern == EXCLUDE_BOSS)
            {
                pattern = BOSS;
                excluded = true;
            }
            else if (pattern == EXCLUDE_CAIROS)
            {
                pattern = CAIROS;
                excluded = true;
            }
            else if (pattern == EXCLUDE_ELLIA)
            {
                pattern = ELLIA;
                excluded = true;
            }

            int i = 0;
            foreach (FileInfo file in Files)
            {
                

                Match match = Regex.Match(file.Name, pattern, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    continue;
                }

                //Bitmap sourceImage = new Bitmap(@"C:\Test\quiz_1920_1080.png");
                Bitmap template = null;
                try
                {
                    template = new Bitmap(file.FullName);
                }
                catch (Exception e)
                {
                    //ignore
                    continue;
                }

                template = ConvertToFormat(template, PixelFormat.Format24bppRgb);
                template = BitmapUtils.Resize(template, template.Width * w / BASE_WIDTH, template.Height * h / BASE_HEIGHT);

                template = new ResizeBicubic((int)(template.Width * scale), (int)(template.Height * scale)).Apply(template);

                ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.89f);

                TemplateMatch[] matchings = tm.ProcessImage(answer.img, template);

                if (matchings.Length > 0 && !excluded)
                {
                    return true;
                }
                else if (matchings.Length > 0 && excluded)
                {
                    return false;
                }

            }

            return excluded;
        }

        public static Bitmap CropImage(Bitmap source, int x, int y, int w, int h)
        {
            Rectangle rec = new Rectangle(x, y, w, h);
            return CropImage(source, rec);
        }

        public static int FindMatchImage(Bitmap source, Bitmap template)
        {
            source = ConvertToFormat(source, PixelFormat.Format24bppRgb);
            source = new ResizeBicubic((int)(source.Width * 0.4), (int)(source.Height * 0.4)).Apply(source);

            template = ConvertToFormat(template, PixelFormat.Format24bppRgb);
            template = new ResizeBicubic((int)(template.Width * 0.4), (int)(template.Height * 0.4)).Apply(template);

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.90f);

            TemplateMatch[] matchings = tm.ProcessImage(source, template);

            return matchings.Length;
        }

        public static Bitmap CropImage(Bitmap source, Rectangle rec)
        {
            Bitmap target = new Bitmap(rec.Width, rec.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(source, new Rectangle(0, 0, target.Width, target.Height),
                                 rec, GraphicsUnit.Pixel);
            }

            // Test
            target.Save("C:\\TestWin32\\crop-quiz.png", ImageFormat.Png);

            return target;
        }

        private static bool IsAttributePattern(string pattern)
        {
            return pattern == ALL_FIRE || pattern == ALL_WATER || pattern == ALL_WIND || pattern == EXCLUDE_FIRE || pattern == EXCLUDE_WATER || pattern == EXCLUDE_WIND;
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

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
}
