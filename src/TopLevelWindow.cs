using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WinApiWrappers
{
    public partial class TopLevelWindow : IDeviceContext, IWin32Window
    {
        private IntPtr _handle;

        private IntPtr _hDc = IntPtr.Zero;

        public TopLevelWindow(IntPtr windowHandle)
        {
            _handle = windowHandle;
        }

        public TopLevelWindow(string caption)
        {
            _handle = NativeMethods.FindWindow(IntPtr.Zero, caption);
        }

        public static TopLevelWindow FromHandle(IntPtr windowHandle)
        {
            return new TopLevelWindow(windowHandle);
        }

        public static TopLevelWindow FromHandle(int windowHandle)
        {
            return new TopLevelWindow(new IntPtr(windowHandle));
        }

        public static TopLevelWindow FromClass(string Class)
        {
            return new TopLevelWindow((IntPtr) NativeMethods.FindWindow(Class, IntPtr.Zero));
        }

        public static bool operator ==(TopLevelWindow window1, TopLevelWindow window2)
        {
            return window1.Handle == window2.Handle;
        }

        public static bool operator !=(TopLevelWindow window1, TopLevelWindow window2)
        {
            return window1.Handle != window2.Handle;
        }

        public static TopLevelWindow TaskBarWindow {
            get { return FromClass("Shell_TrayWnd"); }
        }

        public static TopLevelWindow DesktopWindow {
            get { return new TopLevelWindow(NativeMethods.GetDesktopWindow()); }
        }

        public static List<TopLevelWindow> Windows {
            get {
                List<TopLevelWindow> functionReturnValue = new List<TopLevelWindow>();
                GCHandle listHandle = GCHandle.Alloc(functionReturnValue);
                NativeMethods.EnumWindows(new NativeMethods.EnumWindowsProc(EnumWindowsProcMy), GCHandle.ToIntPtr(listHandle));
                listHandle.Free();
                return functionReturnValue;
            }
        }

        private static bool EnumWindowsProcMy(IntPtr handle, IntPtr pointer)
        {
            List<TopLevelWindow> list = GCHandle.FromIntPtr(pointer).Target as List<TopLevelWindow>;

            TopLevelWindow window = new TopLevelWindow(handle);
            if ((window.Styles & (int)WindowStyles.Child) != (int)WindowStyles.Child)
            {
                list.Add(window);
            }
            return true;
        }

        public static bool ReleaseCapture()
        {
            return NativeMethods.ReleaseCapture();
        }

        public List<TopLevelWindow> Controls {
            get {
                List<TopLevelWindow> functionReturnValue = default(List<TopLevelWindow>);
                functionReturnValue = new List<TopLevelWindow>();
                GCHandle listHandle = GCHandle.Alloc(Controls);
                NativeMethods.EnumChildWindows(Handle, new NativeMethods.EnumWindowsProc(EnumChildWindowsProc), GCHandle.ToIntPtr(listHandle));
                listHandle.Free();
                return functionReturnValue;
            }
        }

        private bool EnumChildWindowsProc(IntPtr handle, IntPtr pointer)
        {
            List<TopLevelWindow> list = GCHandle.FromIntPtr(pointer).Target as List<TopLevelWindow>;
            list.Add(new TopLevelWindow(handle));
            return true;
        }

        public static TopLevelWindow ActiveWindow {
            get { return TopLevelWindow.FromHandle((IntPtr) NativeMethods.GetForegroundWindow()); }
            set { value.Activate(); }
        }

        public void Activate()
        {
            NativeMethods.SetForegroundWindow(Handle);
        }

        public void Close()
        {
            PostMessage(WindowMessages.Close);
        }

        public void Minimize()
        {
            WindowVisibleState = FormWindowState.Minimized;
        }

        public void Maximize()
        {
            WindowVisibleState = FormWindowState.Maximized;
        }

        public void Restore()
        {
            WindowVisibleState = FormWindowState.Normal;
        }

        public void Show()
        {
            NativeMethods.ShowWindow(Handle, NativeMethods.ShowWindowFlags.Show);
        }

        public void Hide()
        {
            NativeMethods.ShowWindow(Handle, NativeMethods.ShowWindowFlags.Hide);
        }

        public void Update()
        {
            NativeMethods.UpdateWindow(Handle);
        }

        public void SysCommands(SystemCommands command)
        {
            NativeMethods.PostMessage(Handle, WindowMessages.SysCommand, new IntPtr((int)command), IntPtr.Zero).ToInt32();
        }

        public IntPtr SendMessage(WindowMessages msg, IntPtr wParam, IntPtr lParam)
        {
            return NativeMethods.SendMessage(Handle, msg, wParam, lParam);
        }

        public IntPtr SendMessageTimeOut(WindowMessages msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr zero = IntPtr.Zero;
            return NativeMethods.SendMessageTimeout(Handle, msg, wParam, lParam, NativeMethods.SendMessageTimeOutFlags.AbortIfHang, 0, ref zero);
        }

        private IntPtr SendMessageTimeOut(WindowMessages msg, IntPtr wParam, IntPtr lParam, NativeMethods.SendMessageTimeOutFlags flags, int timeout, ref IntPtr result)
        {
            return NativeMethods.SendMessageTimeout(Handle, (int)msg, wParam, lParam, flags, timeout, ref result);
        }

        public void PostMessage(WindowMessages msg, IntPtr wParam, IntPtr lParam)
        {
            NativeMethods.PostMessage(Handle, (int)msg, wParam, lParam);
        }

        public void PostMessage(WindowMessages msg)
        {
            NativeMethods.PostMessage(Handle, (int)msg, IntPtr.Zero, IntPtr.Zero);
        }

        public void FrameChanged()
        {
            NativeMethods.SetWindowPos(Handle, 0, 0, 0, 0, 0, NativeMethods.SetWindowPosFlags.NoMove | NativeMethods.SetWindowPosFlags.NoSize | NativeMethods.SetWindowPosFlags.NoActivate | NativeMethods.SetWindowPosFlags.NoZOrder | NativeMethods.SetWindowPosFlags.FrameChanged);
        }

        public void BringToFront()
        {
            NativeMethods.SetWindowPos(Handle, NativeMethods.hWndInsertAfterFlags.Top, 0, 0, 0, 0, NativeMethods.SetWindowPosFlags.NoMove | NativeMethods.SetWindowPosFlags.NoSize | NativeMethods.SetWindowPosFlags.NoActivate);
        }

        private Icon TestIcon(Icon ic)
        {
            if ((((ic != null)) && ((ic.Height > 0) && (ic.Width > 0)))) {
                return ic;
            }
            return null;
        }

        public TopLevelWindow FindControl(string className)
        {
            return new TopLevelWindow((IntPtr) NativeMethods.FindWindowEx(Handle, IntPtr.Zero, className, IntPtr.Zero));
        }

        public bool Visible {
            get { return NativeMethods.IsWindowVisible(Handle); }
            set {
                if (value) {
                    Show();
                } else {
                    Hide();
                }
            }
        }

        private String _processName;
        public string ProcessName {
            get {
                if (_processName == null)
                {
                    Process myProcess = Process;
                    _processName = myProcess.ProcessName;
                    myProcess.Dispose();
                }
                return _processName;
            }
        }

        public Process Process {
            get {
                int pid = 0;
                NativeMethods.GetWindowThreadProcessId(Handle, ref pid);
                Process handleProcess = Process.GetProcessById(pid);
                return handleProcess;
            }
        }

        public bool Focused {
            get { return NativeMethods.GetForegroundWindow() == Handle; }
        }

        public string Title {
            get {
                StringBuilder stringBuffer = new StringBuilder(4096);
                NativeMethods.GetWindowText(Handle, stringBuffer, stringBuffer.Capacity);
                return stringBuffer.ToString();
            }
            set {
                StringBuilder stringBuffer = new StringBuilder(value);
                NativeMethods.SetWindowText(Handle, stringBuffer);
            }
        }

        public Icon Icon {
            get {
                IntPtr iconHandle = default(IntPtr);
                SendMessageTimeOut(WindowMessages.Geticon, IntPtr.Zero, IntPtr.Zero, NativeMethods.SendMessageTimeOutFlags.AbortIfHang, 0, ref iconHandle);
                if (iconHandle == IntPtr.Zero) {
                    SendMessageTimeOut(WindowMessages.Geticon, new IntPtr(1), IntPtr.Zero, NativeMethods.SendMessageTimeOutFlags.AbortIfHang, 0, ref iconHandle);
                }
                if (iconHandle == IntPtr.Zero) {
                    iconHandle = NativeMethods.GetClassLong(Handle, NativeMethods.GetClassLongFlags.SmallIcon);
                }
                if (iconHandle == IntPtr.Zero) {
                    SendMessageTimeOut(WindowMessages.Querydragicon, new IntPtr(1), IntPtr.Zero, NativeMethods.SendMessageTimeOutFlags.AbortIfHang, 0, ref iconHandle);
                }
                if (iconHandle != IntPtr.Zero) {
                    return TestIcon(Icon.FromHandle(iconHandle));
                }
                return null;
            }
        }

        public string ClassName {
            get {
                StringBuilder stringBuffer = new StringBuilder(257);
                NativeMethods.GetClassName(Handle, stringBuffer, stringBuffer.Capacity);
                return stringBuffer.ToString();
            }
        }

        public TopLevelWindow Parent {
            get { return TopLevelWindow.FromHandle((int) NativeMethods.GetWindowLong(Handle, (int)NativeMethods.GetWindowLongFlags.Parent)); }
            set { NativeMethods.SetWindowLong(Handle, NativeMethods.GetWindowLongFlags.Parent, value.Handle); }
        }

        public object UserData {
            get { return NativeMethods.GetWindowLong(Handle, (int)NativeMethods.GetWindowLongFlags.UserData); }
            set { NativeMethods.SetWindowLong(Handle, NativeMethods.GetWindowLongFlags.UserData, value); }
        }

        public Rectangle Bounds {
            get {
                NativeMethods.Rect rect = default(NativeMethods.Rect);
                NativeMethods.GetWindowRect(Handle, ref rect);
                return rect.ToRectangle;
            }
            set {
                bool noSendChanging = false;
                NativeMethods.SetWindowPosFlags flags = NativeMethods.SetWindowPosFlags.NoActivate | NativeMethods.SetWindowPosFlags.NoZOrder;
                if (noSendChanging) {
                    flags = flags | NativeMethods.SetWindowPosFlags.NoSendChanging;
                }
                NativeMethods.SetWindowPos(Handle, 0, value.X, value.Y, value.Width, value.Height, flags);
            }
        }

        public Rectangle ClientBounds {
            get {
                NativeMethods.Rect rect = new NativeMethods.Rect();
                NativeMethods.GetClientRect(Handle, ref rect);
                return rect.ToRectangle;
            }
        }


        public Point Location {
            get { return Bounds.Location; }
            set { NativeMethods.SetWindowPos(Handle, 0, value.X, value.Y, 0, 0, NativeMethods.SetWindowPosFlags.NoSendChanging | NativeMethods.SetWindowPosFlags.NoSize | NativeMethods.SetWindowPosFlags.NoZOrder | NativeMethods.SetWindowPosFlags.NoActivate); }
        }

        public Size Size {
            get { return Bounds.Size; }
            set { NativeMethods.SetWindowPos(Handle, 0, 0, 0, value.Width, value.Height, NativeMethods.SetWindowPosFlags.NoSendChanging | NativeMethods.SetWindowPosFlags.NoMove | NativeMethods.SetWindowPosFlags.NoZOrder | NativeMethods.SetWindowPosFlags.NoActivate); }
        }

        public int Left {
            get { return Bounds.Left; }
            set { Location = new Point(value, Top); }
        }

        public int Top {
            get { return Bounds.Top; }
            set { Location = new Point(Left, value); }
        }

        public int Right {
            get { return Bounds.Right; }
        }

        public int Bottom {
            get { return Bounds.Bottom; }
        }

        public int Width {
            get { return Bounds.Width; }
            set { Size = new Size(value, Height); }
        }

        public int Height {
            get { return Bounds.Height; }
            set { Size = new Size(Width, value); }
        }

        public bool Enabled {
            get { return NativeMethods.IsWindowEnabled(Handle); }
            set { NativeMethods.EnableWindow(Handle, value); }
        }

        public bool Exists {
            get { return NativeMethods.IsWindow(Handle); }
        }

        public bool TopMost {
            get { return Convert.ToBoolean(ExStyles & (int)WindowExtendedStyles.TopMost); }
            set {
                NativeMethods.hWndInsertAfterFlags flag = NativeMethods.hWndInsertAfterFlags.NoTopMost;
                if (value) {
                    flag = NativeMethods.hWndInsertAfterFlags.TopMost;
                }
                NativeMethods.SetWindowPos(Handle, flag, 0, 0, 0, 0, NativeMethods.SetWindowPosFlags.NoActivate | NativeMethods.SetWindowPosFlags.NoMove | NativeMethods.SetWindowPosFlags.NoSendChanging | NativeMethods.SetWindowPosFlags.NoSize);
            }
        }

        public bool Maximized {
            get { return WindowVisibleState == FormWindowState.Maximized; }
            set {
                if (value) {
                    Maximize();
                } else {
                    Restore();
                }
            }
        }


        public bool Minimized {
            get { return WindowVisibleState == FormWindowState.Minimized; }
            set {
                if (value) {
                    Minimize();
                } else {
                    Restore();
                }
            }
        }

        public bool MinimizeBox {
            get { return Convert.ToBoolean(Styles & (int)WindowStyles.MinimizeBox); }
        }

        public bool MaximizeBox {
            get { return Convert.ToBoolean(Styles & (int)WindowStyles.MaximizeBox); }
        }

        public bool CloseBox {
            get { return (Styles & (int)WindowStyles.SysMenu) == (int)WindowStyles.SysMenu; }
        }

        public bool CloseBoxOnly {
            get { return CloseBox && !MinimizeBox && !MaximizeBox && !HelpBox; }
        }

        public bool HelpBox {
            get { return (ExStyles & (int)WindowExtendedStyles.ContextHelp) == (int)WindowExtendedStyles.ContextHelp; }
        }

        public bool HasCaption {
            get { return (Styles & (int)WindowStyles.Caption) == (int)WindowStyles.Caption; }
        }

        public int Styles {
            get { return NativeMethods.GetWindowLong(Handle, (int)NativeMethods.GetWindowLongFlags.Style); }
            set { NativeMethods.SetWindowLong(Handle, NativeMethods.GetWindowLongFlags.Style, new IntPtr(value)); }
        }

        public int ExStyles {
            get { return NativeMethods.GetWindowLong(Handle, (int)NativeMethods.GetWindowLongFlags.ExStyle); }
            set { NativeMethods.SetWindowLong(Handle, NativeMethods.GetWindowLongFlags.ExStyle, new IntPtr(value)); }
        }

        public bool SizeBox {
            get { return Convert.ToBoolean(Styles & (int)WindowStyles.Sizebox); }
        }

        public FormWindowState WindowVisibleState {
            get {
                if (NativeMethods.IsIconic(Handle))
                    return FormWindowState.Minimized;
                if (NativeMethods.IsZoomed(Handle))
                    return FormWindowState.Maximized;
                return FormWindowState.Normal;
            }
            set {
                switch (value) {
                    case FormWindowState.Normal:
                        NativeMethods.ShowWindow(Handle, NativeMethods.ShowWindowFlags.Normal);
                        break;
                    case FormWindowState.Minimized:
                        NativeMethods.ShowWindow(Handle, NativeMethods.ShowWindowFlags.Minimize);

                        break;
                    case FormWindowState.Maximized:
                        NativeMethods.ShowWindow(Handle, NativeMethods.ShowWindowFlags.ShowMaximized);

                        break;
                }
            }
        }

        public IntPtr Region {
            set { NativeMethods.SetWindowRgn(Handle, value, true); }
        }

        public IntPtr Handle {
            get { return _handle; }
        }

        public Screen Screen {
            get {
                return Screen.FromHandle(Handle);
            }
        }

        public void Dispose()
        {
            if (_hDc != IntPtr.Zero)
                ReleaseHdc();
        }

        public IntPtr GetHdc()
        {
            if (_hDc == IntPtr.Zero) 
                _hDc = NativeMethods.GetDC(_handle);

            return _hDc;
        }

        public void ReleaseHdc()
        {
            NativeMethods.ReleaseDC(_handle, _hDc);
            _hDc = IntPtr.Zero;
        }
    }
}
