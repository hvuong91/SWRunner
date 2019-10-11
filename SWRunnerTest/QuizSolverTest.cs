using NUnit.Framework;
using SWRunnerApp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SWRunnerTest
{
    [TestFixture]
    class QuizSolverTest
    {
        private static readonly string captchaDir = @"TestData\captcha";
        private static readonly string captchaPopupDir = @"TestData\captchaPopup";
        private static readonly string quizImgDir = @"TestData\quiz_img";

        [Test]
        public void TestGetQuizPattern()
        {
            // All water
            Bitmap sourceImage = new Bitmap(quizImgDir + "\\3.png");


            string pattern = QuizSolver.GetQuizPattern(sourceImage);

            Assert.AreEqual("2", pattern);

        }

        [Test]
        public void TestCropImage()
        {
            Bitmap sourceImage = new Bitmap(quizImgDir + "\\1.png");

            Rectangle rec = new Rectangle(400, 250, 1200, 150);
            QuizSolver.CropImage(sourceImage, rec);
        }
    }
}
