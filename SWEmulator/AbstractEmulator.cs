﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace SWEmulator
{
    public abstract class AbstractEmulator : IEmulator
    {
        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter,
            string className, string windowTitle);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        protected IntPtr MainWindow { get; private set; }

        private const int MIN_WAIT = 100;
        private const int MAX_WAIT = 200;

        private const int OFFSET_X = 7;
        private const int OFFSET_Y = 5;

        public int Width { get; set; }
        public int Height { get; set; }

        public AbstractEmulator()
        {
            MainWindow = GetMainWindow();
            GetWindowSize(MainWindow);
        }

        public void Click(PointF point)
        {
            // Set up random pos for each time
            int offSetX = new Random().Next(2 * OFFSET_X) - OFFSET_X;
            int offSetY = new Random().Next(2 * OFFSET_Y) - OFFSET_Y;
            int coord = (int) (point.Y + OFFSET_Y) << 16 | (int) (point.X + OFFSET_X);

            PostMessage(MainWindow, Win32Constants.WM_LBUTTONDOWN, 1, coord);
            Thread.Sleep(new Random().Next(MIN_WAIT, MAX_WAIT));
            PostMessage(MainWindow, Win32Constants.WM_LBUTTONUP, 0, coord);

            // Random sleep after click
            int randomWaitTime = new Random().Next(300);
            Thread.Sleep(randomWaitTime);
        }

        public void RandomClick()
        {
            int randomX = new Random().Next(Width / 2) + 200;
            int randomY = new Random().Next(Height / 2) + 200;

            Click(new PointF(randomX, randomY));
            Thread.Sleep(200);
        }

        public abstract IntPtr GetMainWindow();

        private void GetWindowSize(IntPtr hWnd)
        {
            if (!GetWindowRect(hWnd, out Rect rct))
            {
                // TODO: Log fail message
                return;
            }

            Width = rct.right - rct.left;
            Height = rct.bottom - rct.top;
        }
    }
}
