using LibMaterial.Framework;
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
            RoundWindow(window.Handle);
        }

        public static void RoundWindow(IntPtr window)
        {
            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = DWMWCP_ROUND;
            DwmSetWindowAttribute(window, attribute, ref preference, sizeof(uint));
        }

        public static void MicaWindow(Form window)
        {
            bool DarkStatus = !LibRegistry.GetAppUseLightTheme(); // Win32 doesn't do dark mode :(
            LibApply.Apply_Light_Theme(window.Handle, DarkStatus);
            LibApply.Apply_Transparent_Form(window.Handle, DarkStatus);
            LibApply.Apply_Backdrop_Effect(window.Handle, LibImport.DwmSystemBackdropTypeFlgs.DWMSBT_MAINWINDOW);
        }

        public static float Remap(float value, float InputA, float InputB, float OutputA, float OutputB)
        {
            return ((float)(value) - (float)(InputA)) / ((float)(InputB) - (float)(InputA)) * ((float)(OutputB) - (float)(OutputA)) + (float)(OutputA);
        }
    }

    public static class ListViewExtensions
    {
        private const int LVM_FIRST = 0x1000;
        private const int LVM_SETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 54;
        private const int LVS_EX_TRANSPARENTBKGND = 0x400000;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public static void SetTransparentBackground(ListView listView)
        {
            IntPtr exStyle = new IntPtr((int)SendMessage(listView.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, IntPtr.Zero, IntPtr.Zero));
            exStyle = new IntPtr(exStyle.ToInt32() | LVS_EX_TRANSPARENTBKGND);
            SendMessage(listView.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, IntPtr.Zero, exStyle);
        }
    }
}
