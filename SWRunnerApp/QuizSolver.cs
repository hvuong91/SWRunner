using SWEmulator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace SWRunnerApp
{
    /// <summary>
    /// This class contains the logic for solving the quiz.
    /// </summary>
    public static class QuizSolver
    {
        private static string resourceFolder = @"Resources\captcha";

        public static void SolveQuiz(AbstractEmulator emulator)
        {
            // Capture screen with quiz 
            Bitmap screen = emulator.PrintWindow(emulator.GetMainWindow());

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

    }
}
