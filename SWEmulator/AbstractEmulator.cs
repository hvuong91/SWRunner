using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
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

        public int Width { get; set; }
        public int Height { get; set; }

        public AbstractEmulator(string windowName)
        {
            MainWindow = GetMainWindow(windowName);
            GetWindowSize(MainWindow);
        }

        public void Click(PointF point)
        {
            int coord = (int) point.Y << 16 | (int) point.X;
            PostMessage(MainWindow, Win32Constants.WM_LBUTTONDOWN, 1, coord);
            Thread.Sleep(new Random().Next(MIN_WAIT, MAX_WAIT));
            PostMessage(MainWindow, Win32Constants.WM_LBUTTONUP, 0, coord);
        }

        public abstract IntPtr GetMainWindow(string windowName);

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
