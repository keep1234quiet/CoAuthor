using CoAuthor.src;
using System;
using Word = Microsoft.Office.Interop.Word;

namespace CoAuthor
{

    public partial class ThisAddIn
    {
        #region 变量初始化

        private static HookKeyboard.LowLevelKeyboardProc _proc = HookKeyboard.HookCallback; // 声明成全局的委托，避免被垃圾回收

        //delegate void DelegateTest(int i);
        //DelegateTest delegateTest;
        public static IntPtr _hookID;
        #endregion

        #region 插件加载、卸载
        private void ThisAddIn_Startup(object sender, System.EventArgs e) {
            _hookID = HookKeyboard.SetHook(_proc);
            //Globals.ThisAddIn.Application.WindowSelectionChange += Application_SelectionChange;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e) {
            HookKeyboard.UnhookWindowsHookEx(_hookID);
            //Globals.ThisAddIn.Application.WindowSelectionChange -= Application_SelectionChange;
        }
        #endregion

        #region 光标位置变化
        private static void Application_SelectionChange(Word.Selection selection) {
            // 处理光标位置更改事件的逻辑

            //if(wait_accept && cursor_position_after_hint < selectionRange.Start) // 往左移不管，往右移动了就得少删几个字符（往右移动说明接收了一部分）
            //    cursor_move_num = selectionRange.Start - cursor_position_after_hint; // 往后移动数值增加，往前移动数值变小
        }
        #endregion

        #region VSTO 自动生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InternalStartup() {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion




    }
}