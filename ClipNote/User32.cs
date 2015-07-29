using System;
using System.Runtime.InteropServices;
using System.Text;

namespace eClipx
{
    public static class User32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);


        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int FindWindow(
            string lpClassName, // class name 
            string lpWindowName // window name 
        );

        [DllImport("user32.dll")]
        public static extern int SendMessage(
            int hWnd, // handle to destination window 
            uint Msg, // message 
            int wParam, // first message parameter 
            int lParam // second message parameter 
        );

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(
            IntPtr hWnd // handle to window
            );

        private const int GWL_EXSTYLE = (-20);
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int WS_EX_APPWINDOW = 0x40000;

        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDLAST = 1;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_OWNER = 4;
        public const int GW_CHILD = 5;

        public delegate int EnumWindowsProcDelegate(int hWnd, int lParam);

        [DllImport("user32")]
        public static extern int EnumWindows(EnumWindowsProcDelegate lpEnumFunc, int lParam);

        [DllImport("User32.Dll")]
        public static extern void GetWindowText(int h, StringBuilder s, int nMaxCount);

        [DllImport("user32", EntryPoint = "GetWindowLongA")]
        public static extern int GetWindowLongPtr(int hwnd, int nIndex);

        [DllImport("user32")]
        public static extern int GetParent(int hwnd);

        [DllImport("user32")]
        public static extern int GetWindow(int hwnd, int wCmd);

        [DllImport("user32")]
        public static extern int IsWindowVisible(int hwnd);

        [DllImport("user32")]
        public static extern int GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(int hWnd);
        /// <summary>Shows a Window</summary>
        /// <remarks>
        /// <para>To perform certain special effects when showing or hiding a 
        /// window, use AnimateWindow.</para>
        ///<para>The first time an application calls ShowWindow, it should use 
        ///the WinMain function's nCmdShow parameter as its nCmdShow parameter. 
        ///Subsequent calls to ShowWindow must use one of the values in the 
        ///given list, instead of the one specified by the WinMain function's 
        ///nCmdShow parameter.</para>
        ///<para>As noted in the discussion of the nCmdShow parameter, the 
        ///nCmdShow value is ignored in the first call to ShowWindow if the 
        ///program that launched the application specifies startup information 
        ///in the structure. In this case, ShowWindow uses the information 
        ///specified in the STARTUPINFO structure to show the window. On 
        ///subsequent calls, the application must call ShowWindow with nCmdShow 
        ///set to SW_SHOWDEFAULT to use the startup information provided by the 
        ///program that launched the application. This behavior is designed for 
        ///the following situations: </para>
        ///<list type="">
        ///    <item>Applications create their main window by calling CreateWindow 
        ///    with the WS_VISIBLE flag set. </item>
        ///    <item>Applications create their main window by calling CreateWindow 
        ///    with the WS_VISIBLE flag cleared, and later call ShowWindow with the 
        ///    SW_SHOW flag set to make it visible.</item>
        ///</list></remarks>
        /// <param name="hWnd">Handle to the window.</param>
        /// <param name="nCmdShow">Specifies how the window is to be shown. 
        /// This parameter is ignored the first time an application calls 
        /// ShowWindow, if the program that launched the application provides a 
        /// STARTUPINFO structure. Otherwise, the first time ShowWindow is called, 
        /// the value should be the value obtained by the WinMain function in its 
        /// nCmdShow parameter. In subsequent calls, this parameter can be one of 
        /// the WindowShowStyle members.</param>
        /// <returns>
        /// If the window was previously visible, the return value is nonzero. 
        /// If the window was previously hidden, the return value is zero.
        /// </returns>
        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        /// <summary>Enumeration of the different ways of showing a window using 
        /// ShowWindow</summary>
        internal enum WindowShowStyle : uint
        {
            /// <summary>Hides the window and activates another window.</summary>
            /// <remarks>See SW_HIDE</remarks>
            Hide = 0,
            /// <summary>Activates and displays a window. If the window is minimized 
            /// or maximized, the system restores it to its original size and 
            /// position. An application should specify this flag when displaying 
            /// the window for the first time.</summary>
            /// <remarks>See SW_SHOWNORMAL</remarks>
            ShowNormal = 1,
            /// <summary>Activates the window and displays it as a minimized window.</summary>
            /// <remarks>See SW_SHOWMINIMIZED</remarks>
            ShowMinimized = 2,
            /// <summary>Activates the window and displays it as a maximized window.</summary>
            /// <remarks>See SW_SHOWMAXIMIZED</remarks>
            ShowMaximized = 3,
            /// <summary>Maximizes the specified window.</summary>
            /// <remarks>See SW_MAXIMIZE</remarks>
            Maximize = 3,
            /// <summary>Displays a window in its most recent size and position. 
            /// This value is similar to "ShowNormal", except the window is not 
            /// actived.</summary>
            /// <remarks>See SW_SHOWNOACTIVATE</remarks>
            ShowNormalNoActivate = 4,
            /// <summary>Activates the window and displays it in its current size 
            /// and position.</summary>
            /// <remarks>See SW_SHOW</remarks>
            Show = 5,
            /// <summary>Minimizes the specified window and activates the next 
            /// top-level window in the Z order.</summary>
            /// <remarks>See SW_MINIMIZE</remarks>
            Minimize = 6,
            /// <summary>Displays the window as a minimized window. This value is 
            /// similar to "ShowMinimized", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
            ShowMinNoActivate = 7,
            /// <summary>Displays the window in its current size and position. This 
            /// value is similar to "Show", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWNA</remarks>
            ShowNoActivate = 8,
            /// <summary>Activates and displays the window. If the window is 
            /// minimized or maximized, the system restores it to its original size 
            /// and position. An application should specify this flag when restoring 
            /// a minimized window.</summary>
            /// <remarks>See SW_RESTORE</remarks>
            Restore = 9,
            /// <summary>Sets the show state based on the SW_ value specified in the 
            /// STARTUPINFO structure passed to the CreateProcess function by the 
            /// program that started the application.</summary>
            /// <remarks>See SW_SHOWDEFAULT</remarks>
            ShowDefault = 10,
            /// <summary>Windows 2000/XP: Minimizes a window, even if the thread 
            /// that owns the window is hung. This flag should only be used when 
            /// minimizing windows from a different thread.</summary>
            /// <remarks>See SW_FORCEMINIMIZE</remarks>
            ForceMinimized = 11
        }

        #region Clipboard
        [DllImport("User32.dll")]
        public static extern int SetClipboardViewer(int hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        #endregion

    }
}
