using AForge.Imaging;
using AForge.Imaging.Filters;
using SWEmulator;
using System;
using System.Collections.Generic;
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
        private static readonly string captchaDir = @"Resources\captcha";
        private static readonly string captchaPopupDir = @"Resources\captchaPopup";

        public static void SolveQuiz(AbstractEmulator emulator)
        {
            // Capture screen with quiz 
            Bitmap screen = emulator.PrintWindow(emulator.GetMainWindow());
            screen = ConvertToFormat(screen, PixelFormat.Format24bppRgb);

            string quizPattern = GetQuizPattern(screen);

            for (int i = 0; i <= 7; i++)
            {
                (Point point, Bitmap img) answer = GetAnswer(i);
                if (IsCorrectAnswer(answer, quizPattern))
                {
                    emulator.Click(answer.point);
                }
            }

            // TODO: Click OK
        }
        
        // The following methods should be private.
        public static string GetQuizPattern(Bitmap screen)
        {
            string pattern = String.Empty;

            Rectangle rec = new Rectangle(400, 250, 1200, 150);
            Bitmap questionImg = QuizSolver.CropImage(screen, rec);
            questionImg = ConvertToFormat(questionImg, PixelFormat.Format24bppRgb);
            questionImg = new ResizeBicubic((int)(questionImg.Width * 0.6), (int)(questionImg.Height * 0.6)).Apply(questionImg);
            

            DirectoryInfo d = new DirectoryInfo(captchaPopupDir);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            // Test
            int match = 0;

            foreach (FileInfo file in Files)
            {
                Bitmap template = new Bitmap(file.FullName);
                template = ConvertToFormat(template, PixelFormat.Format24bppRgb);
                template = new ResizeBicubic((int)(template.Width * 0.6), (int)(template.Height * 0.6)).Apply(template);
                ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.88f);

                TemplateMatch[] matchings = tm.ProcessImage(questionImg, template);
                if (matchings.Length > 0)
                {
                    pattern += file.Name + " ";
                }
            }


            return pattern;
        }

        public static (Point point, Bitmap img) GetAnswer(int answerNum)
        {
            // TODO
            return (new Point(), null);
        }

        public static bool IsCorrectAnswer((Point point, Bitmap img) answer, string pattern)
        {
            // TODO
            return false;
        }

        public static Bitmap CropImage(Bitmap source, int x, int y, int w, int h)
        {
            Rectangle rec = new Rectangle(x, y, w, h);
            return CropImage(source, rec);
        }

        public static int FindMatchImage(Bitmap source, Bitmap template)
        {
            int result = 0;

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
            target.Save("C:\\TestWin32\\crop.png", ImageFormat.Png);

            return target;
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
    }
}
