using System;

namespace WinApiWrappers
{
    [Flags]
    public enum SystemCommands : int
    {
        Close = 0xf060,
        ContextHelp = 0xf180,
        Default = 0xf160,
        HotKey = 0xf150,
        HScroll = 0xf080,
        KeyMenu = 0xf100,
        Maximize = 0xf030,
        Minimize = 0xf020,
        MonitorPower = 0xf170,
        MouseMenu = 0xf090,
        Move = 0xf010,
        NextWindow = 0xf040,
        PrevWindow = 0xf050,
        Restore = 0xf120,
        ScreenSave = 0xf140,
        Size = 0xf000,
        TaskList = 0xf130,
        VScroll = 0xf070,
        DragMove = 0xf012,
        LeftSize = 0xf001,
        RightSize = 0xf002,
        UpSize = 0xf003,
        UpLeftSize = 0xf004,
        UpRightSize = 0xf005,
        DnSize = 0xf006,
        DnLeftSize = 0xf007,
        DnRightSize = 0xf008
    }
}