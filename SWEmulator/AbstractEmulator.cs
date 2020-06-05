using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public int Width
            {
                get { return right - left; }
            }

            public int Height
            {
                get { return bottom - top; }
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        protected IntPtr MainWindow { get; private set; }
        
        private const int MIN_WAIT = 200;
        private const int MAX_WAIT = 300;

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

        public void Click(Point point)
        {
            int offSetX = new Random().Next(2 * OFFSET_X) - OFFSET_X;
            int offSetY = new Random().Next(2 * OFFSET_Y) - OFFSET_Y;
            int coord = (point.Y + OFFSET_Y) << 16 | (point.X + OFFSET_X);

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

        public Bitmap PrintWindow(IntPtr mainWindow)
        {
            
            Rect rc;
            GetWindowRect(mainWindow, out rc);

            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format24bppRgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            // meh, gotta do this later
            PrintWindow(mainWindow, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);

            bmp.Save("C:\\TestWin32\\test.png", ImageFormat.Bmp);

            Thread.Sleep(500);
            gfxBmp.Dispose();

            return bmp;
        }

        public void PressEsc()
        {
            PressKey(Win32Constants.VK_ESCAPE);
        }

        public void PressKey(int key)
        {
            PostMessage(MainWindow, Win32Constants.WM_KEYDOWN, key, 0);
            Thread.Sleep(200);
            PostMessage(MainWindow, Win32Constants.WM_KEYUP, key, 0);
        }

        public abstract IntPtr GetMainWindow();

        public abstract Bitmap PrintWindow();

        private void GetWindowSize(IntPtr hWnd)
        {
            // TODO: Timer on this since it might be stuck forever
            int tries = 100;
            Rect rct;
            while (!GetWindowRect(hWnd, out rct) && tries-- > 0)
            {
                // TODO: Log fail message
                return;
            }

            if (tries <= 0)
            {
                throw new Exception("Failed to get Window Size");
            }

            Width = rct.right - rct.left;
            Height = rct.bottom - rct.top;
        }

        /// <summary>
        ///     Specifies a raster-operation code. These codes define how the color data for the
        ///     source rectangle is to be combined with the color data for the destination
        ///     rectangle to achieve the final color.
        /// </summary>
        public enum TernaryRasterOperations : uint
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062,
            /// <summary>
            /// Capture window as seen on screen.  This includes layered windows
            /// such as WPF windows with AllowsTransparency="true"
            /// </summary>
            CAPTUREBLT = 0x40000000
        }
    }
}
