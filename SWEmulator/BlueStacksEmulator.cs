using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace SWEmulator
{
    public class BlueStacksEmulator : AbstractEmulator
    {
        public IntPtr Screen { get; private set; }

        public BlueStacksEmulator()
        {
            Screen = GetScreen();
        }

        public override IntPtr GetMainWindow()
        {
            IntPtr parent = FindWindow(null, "BlueStacks");
            IntPtr subWindow = FindWindowEx(parent, IntPtr.Zero, null, "BlueStacks Android PluginAndroid");
            //IntPtr mainWindow = FindWindowEx(subWindow, IntPtr.Zero, "BlueStacksApp", null);
            IntPtr mainWindow = FindWindowEx(subWindow, IntPtr.Zero, null, "_ctl.Window");
            return subWindow;
        }

        private IntPtr GetScreen()
        {
            IntPtr parent = FindWindow(null, "BlueStacks");
            IntPtr subWindow = FindWindowEx(parent, IntPtr.Zero, null, "BlueStacks Android PluginAndroid");
            //IntPtr mainWindow = FindWindowEx(subWindow, IntPtr.Zero, "BlueStacksApp", null);
            //IntPtr mainWindow = FindWindowEx(subWindow, IntPtr.Zero, null, "_ctl.Window");
            return subWindow;
        }

        public override Bitmap PrintWindow()
        {

            // get te hDC of the target window
            if (!GetWindowRect(Screen, out Rect rc))
            {
                throw new Exception("Failed to Print Window");
            }
            var testBmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppRgb);
            Graphics graphics = Graphics.FromImage(testBmp);
            graphics.CopyFromScreen(rc.left, rc.top, 0, 0, new Size(rc.Width, rc.Height), CopyPixelOperation.SourceCopy);

            string test = $"C:\\TestWin32\\bs-test.png";

            testBmp.Save(test, ImageFormat.Png);

            return testBmp;
        }

        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }
    }
}
