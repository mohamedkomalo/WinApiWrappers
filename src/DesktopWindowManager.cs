using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinApiWrappers
{
    public class DesktopWindowManager
    {
        public static bool IsAeroEnabled()
        {
            bool aeroEnabled = false;

            if (Environment.OSVersion.Version.Major >= 6)
            {
                NativeMethods.DwmIsCompositionEnabled(out aeroEnabled);
            }

            return aeroEnabled;
        }

        public static bool IsAeroOpaque()
        {
            if (Environment.OSVersion.Version.Major < 6) return true;

            DWM_COLORIZATION_PARAMS colorizationParams;
            DwmGetColorizationParameters(out colorizationParams);
            return colorizationParams.fOpaque;
        }

        [DllImport("dwmapi.dll", EntryPoint = "#127", PreserveSig = false)]
        private static extern void DwmGetColorizationParameters(out DWM_COLORIZATION_PARAMS parameters);

        private struct DWM_COLORIZATION_PARAMS
        {
            public uint clrColor;
            public uint clrAfterGlow;
            public uint nIntensity;
            public uint clrAfterGlowBalance;
            public uint clrBlurBalance;
            public uint clrGlassReflectionIntensity;
            public bool fOpaque;
        }

        public static void EnableBlurBehindWindow(IWin32Window window, Rectangle blurBounds)
        {
            var blurFlags = new DWM_BLURBEHIND();
            blurFlags.dwFlags = DwmBlurBehindFlags.DWM_BB_ENABLE | DwmBlurBehindFlags.DWM_BB_BLURREGION;
            blurFlags.fEnable = true;
            blurFlags.hRgnBlur = NativeMethods.CreateRectRgn(blurBounds.Left, blurBounds.Top, blurBounds.Width, blurBounds.Height);
            NativeMethods.DwmEnableBlurBehindWindow(window.Handle, ref blurFlags);
            NativeMethods.DeleteObject(blurFlags.hRgnBlur);
        }

        public static void DisableBlurBehindWindow(IWin32Window window)
        {
            var blurFlags = new DWM_BLURBEHIND();
            blurFlags.dwFlags = DwmBlurBehindFlags.DWM_BB_ENABLE;
            blurFlags.fEnable = false;
            NativeMethods.DwmEnableBlurBehindWindow(window.Handle, ref blurFlags);
        }

        [Flags]
        private enum DwmBlurBehindFlags : uint
        {
            DWM_BB_ENABLE = 0x00000001,
            DWM_BB_BLURREGION = 0x00000002,
            DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DWM_BLURBEHIND
        {
            public DwmBlurBehindFlags dwFlags;
            public bool fEnable;
            public IntPtr hRgnBlur;
            public bool fTansitionOnMaximized;
        }

        private static class NativeMethods
        {
            [DllImport("dwmapi.dll")]
            public static extern IntPtr DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurFlags);

            [DllImport("dwmapi.dll")]
            public static extern int DwmIsCompositionEnabled(out bool pfEnabled);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr CreateRectRgn(int Left, int Top, int Right, int Bottom);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool DeleteObject(IntPtr hObject);
        }
    }
}
