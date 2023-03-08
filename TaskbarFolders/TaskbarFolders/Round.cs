using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TaskbarFolders
{
    internal static class Round
    {
        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_WINDOW_CORNER_PREFERENCE = 33
        }

        const int DWMWCP_DEFAULT = 0;
        const int DWMWCP_DONOTROUND = 1;
        const int DWMWCP_ROUND = 2;
        const int DWMWCP_ROUNDSMALL = 3;

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern long DwmSetWindowAttribute(IntPtr hwnd,
                                                         DWMWINDOWATTRIBUTE attribute,
                                                         ref int pvAttribute,
                                                         uint cbAttribute);

        public static void RoundWindow(Form window)
        {
            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = DWMWCP_ROUND;
            DwmSetWindowAttribute(window.Handle, attribute, ref preference, sizeof(uint));
        }

        public static float Remap(float value, float InputA, float InputB, float OutputA, float OutputB)
        {
            return ((float)(value) - (float)(InputA)) / ((float)(InputB) - (float)(InputA)) * ((float)(OutputB) - (float)(OutputA)) + (float)(OutputA);
        }
    }
}
