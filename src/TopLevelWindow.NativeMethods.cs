using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApiWrappers
{

    partial class TopLevelWindow
    {
        private static class NativeMethods
        {
            [Flags]
            public enum GetClassLongFlags
            {
                SmallIcon = -34,
                Icon = -14
            }

            internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

            [Flags]
            public enum GetWindowLongFlags
            {
                ID = -12,
                Style = -16,
                ExStyle = -20,
                Parent = -8,
                WndProc = -4,
                hInstance = -6,
                UserData = -21
            }

            [Flags]
            public enum hWndInsertAfterFlags
            {
                Bottom = 1,
                NoTopMost = -2,
                Top = 0,
                TopMost = -1
            }

            public struct Rect
            {
                public int Left;
                public int Top;
                public int Right;

                public int Bottom;
                public Rect(int pLeft, int pTop, int pRight, int pBottom)
                {
                    Left = pLeft;
                    Top = pTop;
                    Right = pRight;
                    Bottom = pBottom;
                }


                public static readonly Rect Empty = new Rect(0, 0, 0, 0);
                public int Height
                {
                    get { return Bottom - Top; }
                }

                public int Width
                {
                    get { return Right - Left; }
                }

                public Size Size
                {
                    get { return new Size(Width, Height); }
                }

                public Point Location
                {
                    get { return new Point(Left, Top); }
                }

                public Rectangle ToRectangle
                {
                    get { return Rectangle.FromLTRB(Left, Top, Right, Bottom); }
                }
            }

            [Flags]
            public enum SendMessageTimeOutFlags
            {
                Normal = 0,
                Block = 1,
                AbortIfHang = 2,
                NoTimeOutIfNotHung = 8
            }

            [Flags]
            public enum SetWindowPosFlags : uint
            {
                NoSize = 0x1,
                NoMove = 0x2,
                NoZOrder = 0x4,
                NoRedraw = 0x8,
                NoActivate = 0x10,
                FrameChanged = 0x20,
                ShowWindow = 0x40,
                HideWindow = 0x80,
                NoCopyBits = 0x100,
                NoOwnerZOrder = 0x200,
                NoSendChanging = 0x400
            }

            [Flags]
            public enum ShowWindowFlags
            {
                Hide = 0,
                Normal = 1,
                ShowMinimized = 2,
                ShowMaximized = 3,
                ShowNoActivate = 4,
                Show = 5,
                Minimize = 6,
                ShowMinNoActive = 7,
                ShowNA = 8,
                Restore = 9,
                ShowDefault = 10,
                ForceMinimize = 11,
                Max = 11
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool EnableWindow(IntPtr hWnd, bool Enabled);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport("User32.Dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr FindWindow(IntPtr lpClassName, string lpWindowName);

            [DllImport("User32.Dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr FindWindow(string lpClassName, IntPtr lpWindowName);

            [DllImport("User32.Dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName,
                IntPtr windowTitle);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr GetClassLong(IntPtr hwnd, int index);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr GetClassLong(IntPtr hwnd, GetClassLongFlags index);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool GetClientRect(IntPtr hWnd, ref Rect lpRect);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr GetForegroundWindow();

            [DllImport("User32.Dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool GetWindowRect(IntPtr hWnd, ref Rect lpRect);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

            [DllImport("Gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetRgnBox(IntPtr hRgn, ref Rect lprc);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int cch);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

            [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessId);

            [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);

            [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool IsIconic(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool IsWindow(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool IsWindowEnabled(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool IsWindowVisible(IntPtr hwnd);

            [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool IsZoomed(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr PostMessage(IntPtr hWnd, WindowMessages msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int RegisterWindowMessage(string lpString);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SendMessage(IntPtr hWnd, WindowMessages msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, int Msg, IntPtr wParam, IntPtr lParam,
                SendMessageTimeOutFlags flags, int timeout, ref IntPtr result);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, WindowMessages Msg, IntPtr wParam,
                IntPtr lParam, SendMessageTimeOutFlags flags, int timeout, ref IntPtr result);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            [DllImport("User32.Dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int SetWindowLong(IntPtr hWnd, GetWindowLongFlags nIndex, IntPtr dwNewLong);

            [DllImport("User32.Dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int SetWindowLong(IntPtr hWnd, GetWindowLongFlags nIndex, object dwNewLong);

            [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy,
                SetWindowPosFlags uFlags);

            [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool SetWindowPos(IntPtr hWnd, hWndInsertAfterFlags hWndInsertAfter, int X, int Y, int cx,
                int cy, SetWindowPosFlags uFlags);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int SetWindowText(IntPtr hwnd, StringBuilder lpString);

            [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool ShowWindow(IntPtr hwnd, ShowWindowFlags nCmdShow);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr UpdateWindow(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool ReleaseCapture();

            [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr GetDC(IntPtr hWnd);

            [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);
        }
    }
}
