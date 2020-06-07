using System;
using System.Collections.Generic;
using System.Text;

namespace SWEmulator
{
    public static class Win32Constants
    {
        public const int WM_KEYUP = 0x101;
        public const int WM_KEYDOWN = 0x100;

        public const int VK_ESCAPE = 0x1B;

        public const int WM_COMMAND = 0x111;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;

        public const int VB_KEY3 = 51;
    }
}
