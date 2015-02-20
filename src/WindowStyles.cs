using System;

namespace WinApiWrappers
{
    [Flags]
    public enum WindowStyles : uint
    {
        OverLapped = 0x0,
        Popup = 2147483648,
        Child = 0x40000000,
        Minimize = 0x20000000,
        Visible = 0x10000000,
        Disabled = 0x8000000,
        ClipSiblings = 0x4000000,
        ClipChildren = 0x2000000,
        Maximize = 0x1000000,
        Border = 0x800000,
        DlgFrame = 0x400000,
        VScroll = 0x200000,
        HScroll = 0x100000,
        SysMenu = 0x80000,
        ThickFrame = 0x40000,
        Group = 0x20000,
        TabStop = 0x10000,
        MinimizeBox = 0x20000,
        MaximizeBox = 0x10000,
        Caption = Border | DlgFrame,
        Tiled = OverLapped,
        Iconic = Minimize,
        Sizebox = ThickFrame,
        Tiledwindow = Overlappedwindow,
        Overlappedwindow = OverLapped | Caption | SysMenu | ThickFrame | MinimizeBox | MaximizeBox,
        PopupWindow = Popup | Border | SysMenu,
        ChildWindow = Child
    }
}