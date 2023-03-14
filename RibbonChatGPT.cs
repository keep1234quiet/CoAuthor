using CoAuthor.src;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using Application = Microsoft.Office.Interop.Word.Application;
using Word = Microsoft.Office.Interop.Word;

namespace CoAuthor
{
    public partial class RibbonChatGPT
    {
        private Application wordapp;

        private static System.Windows.Forms.Form form_loading = null;
        private static System.Windows.Forms.Form form_set = null;

        UserClass userClass;


        private void Ribbon1_Load(object sender, RibbonUIEventArgs e) {
            wordapp = Globals.ThisAddIn.Application;
            userClass = new UserClass();

            wordapp.DocumentOpen += new ApplicationEvents4_DocumentOpenEventHandler(handleDocOpen);

            if (form_loading == null || form_loading.IsDisposed) {
                form_loading = new FormLoading();
                form_loading.Show();
            }
            CommonVariable.ribbonForm = this;

            // Auto set language
            CultureInfo culture = CultureInfo.InstalledUICulture;
            if(Config.Instance.Language == "") {
                if (CommonVariable.languages_simple.Contains(culture.Name)) {
                    Config.Instance.Language = culture.Name;
                }
                else{ 
                    Config.Instance.Language = "en-US";
                }
                Config.Instance.Save();
            }
            
            ResourceCulture.SetCurrentCulture(Config.Instance.Language);
            SetRibbonCulture();
        }

        #region 设置Ribbon的语言
        public void SetRibbonCulture() {
            CommonVariable.ribbonForm.button_setting.Label = ResourceCulture.GetString("RibbonChatGPT_button_setting");
            CommonVariable.ribbonForm.toggleButton_autowrite.Label = ResourceCulture.GetString("RibbonChatGPT_toggleButton_autowrite"); //自动续写
            CommonVariable.ribbonForm.button_refactor.Label = ResourceCulture.GetString("RibbonChatGPT_button_refactor"); //重构内容
            CommonVariable.ribbonForm.button_custom_command_1.Label = ResourceCulture.GetString("RibbonChatGPT_button_custom_command_1"); //自定义指令1
            CommonVariable.ribbonForm.button_custom_command_2.Label = ResourceCulture.GetString("RibbonChatGPT_button_custom_command_2"); //自定义指令2
            CommonVariable.ribbonForm.button_custom_command_3.Label = ResourceCulture.GetString("RibbonChatGPT_button_custom_command_3"); //自定义指令3
            CommonVariable.ribbonForm.button_setting.Label = ResourceCulture.GetString("RibbonChatGPT_button_setting"); //设置
            CommonVariable.ribbonForm.button_help.Label = ResourceCulture.GetString("RibbonChatGPT_button_help"); //使用帮助
        }

        #endregion

        #region 文档打开时绑定委托，可以处理一些事情
        private void handleDocOpen(Microsoft.Office.Interop.Word.Document Doc) {
            /* 句柄函数，当文档打开时弹窗显示文档的内容 */
            // System.Windows.Forms.MessageBox.Show(Doc.Range().Text);
            //if (form_loading == null || form_loading.IsDisposed) {
            //    form_loading = new FormLoading();
            //    form_loading.Show();
            //}
        }
        #endregion

        #region 按钮事件
        private void button_refactor_Click(object sender, RibbonControlEventArgs e) {
            System.Threading.ThreadPool.QueueUserWorkItem((state) => ChangeContent(ResourceCulture.GetString("reorgnize_sentence_str")));
        }

        public void ChangeContent(string user_command) {

            /*
             重构选中内容
             */

            string[] hintTextsByChatGPT = new string[3];

            Word.Selection sec = Globals.ThisAddIn.Application.Selection;
            Word.Words wds = sec.Words;
            string content_selected = wds.Application.Selection.Text;
            //System.Windows.Forms.MessageBox.Show($"{content_selected}");


            if (content_selected.Trim() == "") {
                System.Windows.Forms.MessageBox.Show(ResourceCulture.GetString("not_selected_content_str"));
                return;
            }

            System.Threading.ThreadPool.QueueUserWorkItem((state) => {
                System.Action action = () => { FormLoading.form_loading.Loading(); };
                FormLoading.form_loading.Invoke(action);
            });

            hintTextsByChatGPT = userClass.GetHintsByChatGPT(user_command + ":" + content_selected).GetAwaiter().GetResult();

            System.Threading.ThreadPool.QueueUserWorkItem((state) => {
                System.Action action = () => { FormLoading.form_loading.HideLoading(); };
                FormLoading.form_loading.Invoke(action);
            });

            // System.Windows.Forms.MessageBox.Show($"{hintTextsByChatGPT[0]}\n\n{hintTextsByChatGPT[1]}\n\n{hintTextsByChatGPT[2]}");

            // 获取当前活动文档
            Word.Document doc = null;
            Word.Range selectionRange = null, range = null;
            doc = Globals.ThisAddIn.Application.ActiveDocument;
            // 获取当前选中范围
            selectionRange = Globals.ThisAddIn.Application.Selection.Range;

            Word.Font oldFont = doc.Characters[selectionRange.Start].Font;

            selectionRange.Delete();
            selectionRange.InsertAfter(hintTextsByChatGPT[0]);


            range = doc.Range(selectionRange.Start, selectionRange.Start + hintTextsByChatGPT[0].Length);

            // 设置插入文字的字体样式为光标前面文字的字体样式
            range.Font.Name = oldFont.Name;
            range.Font.Size = oldFont.Size;
            range.Font.Bold = oldFont.Bold;
            range.Font.Italic = oldFont.Italic;
            range.Font.Underline = oldFont.Underline;
            range.Font.Color = oldFont.Color;

            // 移动光标到刚插入的文字末尾
            object position = selectionRange.Start + hintTextsByChatGPT[0].Length;
            range = doc.Range(ref position, ref position);
            range.Select();
        }

        private void toggleButton_autowrite_Click(object sender, RibbonControlEventArgs e) {
            if (toggleButton_autowrite.Checked == true) {
                toggleButton_autowrite.Image = Properties.Resources.pen_write_blue;
            }
            else {
                toggleButton_autowrite.Image = Properties.Resources.pen_write_gray;
            }

            Config.Instance.EnableAutoWrite = toggleButton_autowrite.Checked;
            Config.Instance.Save();
        }

        private void button_setting_Click(object sender, RibbonControlEventArgs e) {
            if (form_set == null || form_set.IsDisposed) {
                form_set = new FormSet();
                form_set.Show();
            }
        }

        private void button_help_Click(object sender, RibbonControlEventArgs e) {
            // 打开指定的网页
            string url = "https://www.google.com";
            System.Diagnostics.Process.Start(url);
        }

        #endregion

        private void button_custom_command_1_Click(object sender, RibbonControlEventArgs e) {
            try {
                if (Config.Instance.CustomCommand1 != "") {
                    System.Threading.ThreadPool.QueueUserWorkItem((state) => ChangeContent(Config.Instance.CustomCommand1));
                }
                else {
                    MessageBox.Show(ResourceCulture.GetString("read_config_fail_or_cmd1_empty_str"));
                }
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show($"{ResourceCulture.GetString("exec_cmd1_error_str")}: {ex}");
                CALog.Instance.LogError(ex.ToString());
            }
        }

        private void button_custom_command_2_Click(object sender, RibbonControlEventArgs e) {
            try {
                if (Config.Instance.CustomCommand2 != "") {
                    System.Threading.ThreadPool.QueueUserWorkItem((state) => ChangeContent(Config.Instance.CustomCommand2));
                }
                else {
                    MessageBox.Show(ResourceCulture.GetString("read_config_fail_or_cmd2_empty_str"));
                }
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show($"{ResourceCulture.GetString("exec_cmd2_error_str")}: {ex}");
                CALog.Instance.LogError(ex.ToString());
            }
        }

        private void button_custom_command_3_Click(object sender, RibbonControlEventArgs e) {
            try {
                if (Config.Instance.CustomCommand3 != "") {
                    System.Threading.ThreadPool.QueueUserWorkItem((state) => ChangeContent(Config.Instance.CustomCommand3));
                }
                else {
                    MessageBox.Show(ResourceCulture.GetString("read_config_fail_or_cmd3_empty_str"));
                }
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show($"{ResourceCulture.GetString("exec_cmd3_error_str")}: {ex}");
                CALog.Instance.LogError(ex.ToString());
            }
        }
    }
}
