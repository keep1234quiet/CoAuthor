using System;
using System.Runtime.InteropServices;

namespace CoAuthor.src
{
    public static class HookKeyboard
    {
        #region 静态变量
        private const int WH_KEYBOARD = 2;
        private static IntPtr _hookID;
        #endregion

        #region 设置钩子, 卸载钩子，按键和鼠标hook
        public static IntPtr SetHook(LowLevelKeyboardProc proc) {
            uint threadId = (uint)GetCurrentThreadId();
            _hookID = SetWindowsHookEx((int)WH_KEYBOARD, proc, IntPtr.Zero, threadId);
            return _hookID;
        }

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            bool isKeyDown = ((ulong)lParam & 0x40000000) == 0 && nCode == 0;
            //bool isLastKeyUp = ((ulong)lParam & 0x80000000) == 0x80000000 && nCode == 0;
            // Debug.WriteLine($"nCode={nCode},wParam={wParam}, lParam={lParam}, isKeyDown={isKeyDown}");

            if (isKeyDown) {
                try {
                    var userClass = new UserClass();
                    bool continue_call = userClass.ProcessKeydownEvent(wParam);
                    if (!continue_call) {
                        return _hookID;
                    }
                }
                catch (Exception ex) {
                    CALog.Instance.LogError(ex.ToString());
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetCurrentThreadId();

        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        #endregion
    }
}
