using NUnit.Framework;
using SWEmulator;
using SWRunner;
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
            Bitmap sourceImage = new Bitmap(quizImgDir + "\\1.png");


            string pattern = QuizSolver.GetQuizPattern(sourceImage, 1920, 1080);

            Assert.AreEqual(QuizSolver.ALL_WATER, pattern);

        }

        [Test]
        public void TestCropImage()
        {
            Bitmap sourceImage = new Bitmap(quizImgDir + "\\1.png");

            Rectangle rec = new Rectangle(400, 250, 1200, 150);
            QuizSolver.CropImage(sourceImage, rec);
        }

        [Test]
        public void TestResizeImage()
        {
            Bitmap source = new Bitmap(quizImgDir + "\\6.png");
            BitmapUtils.Resize(source, 1280, 720).Save(@"C:\TestWin32\720-6.png");
        }

        [Test]
        public void TestCropQuiz()
        {
            string path = quizImgDir + "\\1.png";
            //string path = @"C:\TestWin32\720-1.png";
            Bitmap source = new Bitmap(path);
            for (int i = 1; i <= 8; i++)
            {
                QuizSolver.GetAnswer(source, i);
            }
        }

        [Test]
        public void TestCorrectAnswerAllWater()
        {
            string testScreenPath = quizImgDir + "\\1.png";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.GetQuizPattern(testScreen, 1920, 1080);

            Assert.AreEqual(QuizSolver.ALL_WATER, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1920, 1080))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
            string expected = "148";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCorrectAnswerAngelmon()
        {
            string testScreenPath = quizImgDir + "\\2.png";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.GetQuizPattern(testScreen, 1920, 1080);

            Assert.AreEqual(QuizSolver.ANGELMON, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1920, 1080))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
            string expected = "136";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCorrectAnswerExcludingFire()
        {
            string testScreenPath = quizImgDir + "\\3.png";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.GetQuizPattern(testScreen, 1920, 1080);

            Assert.AreEqual(QuizSolver.EXCLUDE_FIRE, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1920, 1080))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
            string expected = "27";

            Assert.AreEqual(expected, actual);
        }

        [Test, Ignore("Takes too long")]
        public void TestCorrectAnswerAllFire()
        {
            string testScreenPath = quizImgDir + "\\4.png";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.GetQuizPattern(testScreen, 1920, 1080);

            Assert.AreEqual(QuizSolver.ALL_FIRE, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1920, 1080))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
            string expected = "67";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCorrectAnswerCairos()
        {
            string testScreenPath = quizImgDir + "\\5.png";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.GetQuizPattern(testScreen, 1920, 1080);

            Assert.AreEqual(QuizSolver.CAIROS, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1920, 1080))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
            string expected = "137";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCorrectAnswerAngelmon2()
        {
            string testScreenPath = quizImgDir + "\\6.png";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.GetQuizPattern(testScreen, 1920, 1080);

            Assert.AreEqual(QuizSolver.ANGELMON, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1920, 1080))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
            string expected = "1236";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCorrectAnswerAllWind()
        {
            string testScreenPath = quizImgDir + "\\7.jpg";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.GetQuizPattern(testScreen, 1920, 1080);

            Assert.AreEqual(QuizSolver.ALL_WIND, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1920, 1080))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
            string expected = "568";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCorrectAnswerBoss()
        {
            string testScreenPath = quizImgDir + "\\10.png";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.GetQuizPattern(testScreen, 1280, 720);

            Assert.AreEqual(QuizSolver.BOSS, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1280, 720))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
            string expected = "147";

            Assert.AreEqual(expected, actual);
        }

        [Test, Ignore("Ignore for now")]
        public void CreateGrayImages()
        {
            string testScreenPath = quizImgDir + "\\10.png";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.ANGELMON;

            //Assert.AreEqual(QuizSolver.BOSS, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1280, 720))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
        }

        [Test]
        public void TestCorrectAnswerBoss2()
        {
            string testScreenPath = quizImgDir + "\\11.png";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.GetQuizPattern(testScreen, 1280, 720);

            Assert.AreEqual(QuizSolver.BOSS, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1280, 720))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
            string expected = "23";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCorrectAnswerEllia()
        {
            string testScreenPath = quizImgDir + "\\9.png";
            Bitmap testScreen = new Bitmap(testScreenPath);

            string quizPattern = QuizSolver.GetQuizPattern(testScreen, 1280, 720);

            Assert.AreEqual(QuizSolver.ELLIA, quizPattern);

            string actual = "";
            for (int i = 1; i <= 8; i++)
            {
                (Point point, Bitmap img) answer = QuizSolver.GetAnswer(testScreen, i);
                if (QuizSolver.IsCorrectAnswer(answer, quizPattern, 1280, 720))
                {
                    actual += i;
                    //emulator.Click(answer.point);
                }
            }
            string expected = "35";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestQuiz()
        {
            NoxEmulator emulator = new NoxEmulator();
            QuizSolver.SolveQuiz(emulator);
        }
    }
}
