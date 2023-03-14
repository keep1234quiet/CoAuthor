using System;
using System.Runtime.InteropServices;

namespace CoAuthor.src
{

    public static class MemoryManager
    {
        public static int recovery_cnt = 0;
        /// <summary>
        /// 刷新内存
        /// </summary>
        public static void FlushMemory() {
            recovery_cnt += 1;
            if (recovery_cnt % CommonVariable.memory_recovery_interval == 0) {
                GarbageCollect();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }
                recovery_cnt = 0;
            }
        }

        /// <summary>
        /// 主动通知系统进行垃圾回收
        /// </summary>
        public static void GarbageCollect() {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /// <summary>
        /// 把不频繁执行或者已经很久没有执行的代码,没有必要留在物理内存中,只会造成浪费;放在虚拟内存中,等执行这部分代码的时候,再调出来
        /// </summary>
        /// <param name="process"></param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
    }

}
