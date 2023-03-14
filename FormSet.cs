using CoAuthor.src;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace CoAuthor
{
    public partial class FormSet : Form
    {
        Config config = Config.Instance;
        public FormSet() {
            InitializeComponent();

            Display();
            label_info.Visible = false;
            CommonVariable.form_set = this;

            ResourceCulture.SetCurrentCulture(Config.Instance.Language);
            CommonVariable.form_set.SetResourceCulture();
        }


        private void button_save_config_Click(object sender, EventArgs e) {
            try {
                config.WriteIntroduce = textBox_auto_write_introduce.Text.Trim();
                config.CustomCommand1 = textBox_custom_command_1.Text.Trim();
                config.CustomCommand2 = textBox_custom_command_2.Text.Trim();
                config.CustomCommand3 = textBox_custom_command_3.Text.Trim();

                config.AnswerNum = comboBox_anser_num.SelectedIndex + 1;
                string trigger_time_str = comboBox_triger_time.SelectedItem.ToString();
                config.TriggerTime = int.Parse(trigger_time_str.Substring(0, trigger_time_str.Length - 1));
                config.MessageNum = int.Parse(comboBox_message_num.SelectedItem.ToString()); // 2,4 ,6,8,10, 20
                config.EnableAutoWrite = comboBox_auto_write.SelectedIndex == 0 ? true : false;
                config.EnableProxy = comboBox_proxy_switch.SelectedIndex == 0 ? true : false;
                config.ApiKey = textBox_apikey.Text.Trim();
                config.ProxyAddress = textBox_proxy_addr.Text.Trim();
                //config.Language = comboBox_language.SelectedItem != null ? comboBox_language.SelectedItem.ToString() : "";// 

                string selectedLanguage = comboBox_language.SelectedItem.ToString(); // "中文-(zh-CN)"
                int leftIndex = selectedLanguage.IndexOf("(");
                int rightIndex = selectedLanguage.IndexOf(")");
                string language = selectedLanguage.Substring(leftIndex + 1, rightIndex - leftIndex - 1);

                CommonVariable.current_lang = language;
                config.Language = language;

                config.Save();
                label_info.Text = ResourceCulture.GetString("save_success");
                // 如果用户没有输入APIKey的话，需要进行提示
                if (config.ApiKey == "") {
                    MessageBox.Show($"{ResourceCulture.GetString("apikey_blank_fill_please_str")}");
                }
            }
            catch (Exception ex) {
                label_info.Text = $"{ResourceCulture.GetString("save_failed")}:{ex}";
                label_info.ForeColor = Color.Red;
                CALog.Instance.LogError(label_info.Text);
            }
            finally {
                label_info.Visible = true;
            }
        }

        private void button_reset_config_Click(object sender, EventArgs e) {
            try {
                Config config = Config.Instance;
                config.WriteIntroduce = ResourceCulture.GetString("continue_write_str");
                config.CustomCommand1 = "";
                config.CustomCommand2 = "";
                config.CustomCommand3 = "";
                config.AnswerNum = 1;
                config.TriggerTime = 3;
                config.MessageNum = 6;
                config.EnableAutoWrite = true;
                config.EnableProxy = false;
                config.ApiKey = "";
                config.ProxyAddress = "127.0.0.1:10809";
                config.Language = "en-US";

                // 重置控件值
                textBox_auto_write_introduce.Text = config.WriteIntroduce;
                textBox_custom_command_1.Text = config.CustomCommand1;
                textBox_custom_command_2.Text = config.CustomCommand2;
                textBox_custom_command_3.Text = config.CustomCommand3;
                comboBox_anser_num.SelectedIndex = config.AnswerNum - 1;
                comboBox_triger_time.SelectedItem = $"{config.TriggerTime}s";
                comboBox_message_num.SelectedItem = config.MessageNum.ToString();
                comboBox_auto_write.SelectedIndex = config.EnableAutoWrite ? 0 : 1;
                comboBox_proxy_switch.SelectedIndex = config.EnableProxy ? 0 : 1;
                textBox_apikey.Text = config.ApiKey;
                textBox_proxy_addr.Text = config.ProxyAddress;
                //comboBox_language.SelectedItem = config.Language;
                if (CommonVariable.languageDict.ContainsKey(config.Language)) {
                    comboBox_language.SelectedItem = CommonVariable.languageDict[config.Language] + $"-({config.Language})";
                }

                config.Save();
                label_info.Text = ResourceCulture.GetString("reset_success_str");
            }
            catch (Exception ex) {
                label_info.Text = $"{ResourceCulture.GetString("reset_fail_str")}:{ex}";
                label_info.ForeColor = Color.Red;
                CALog.Instance.LogError(label_info.Text);
            }
            finally {
                label_info.Visible = true;
            }
        }

        private void BuildDict() {
            if (CommonVariable.languageDict.Count != 0) {
                return;
            }
            // 定义数据数组
            string[,] data = new string[,] {
                { "zh-CN", "简体中文" },
                { "en-US", "English" },
                { "es-ES", "Español" },
                { "hi-IN", "हिन्दी" },
                { "ar-SA", "العربية" },
                { "pt-BR", "Português" },
                { "bn-BD", "বাংলা" },
                { "ru-RU", "Русский" },
                { "ja-JP", "日本語" },
                { "de-DE", "Deutsch" },
                { "fr-FR", "Français" },
                { "ur-PK", "اردو" },
                { "it-IT", "Italiano" },
                { "fa-IR", "فارسی" },
                { "tr-TR", "Türkçe" },
                { "ta-IN", "தமிழ்" },
                { "af-ZA", "Afrikaans" },
                { "ko-KR", "한국어" },
                { "id-ID", "BahasaIndonesia" },
                { "uk-UA", "Українська" }
            };

            // 使用for循环向字典中添加数据
            for (int i = 0; i < data.GetLength(0); i++) {
                CommonVariable.languageDict.Add(data[i, 0], data[i, 1]);
            }
        }

        public void Display() {
            try {
                Config config = Config.Instance;
                BuildDict();

                // MessageBox.Show($"config.WriteIntroduce={config.WriteIntroduce}");

                textBox_auto_write_introduce.Text = config.WriteIntroduce;
                textBox_custom_command_1.Text = config.CustomCommand1;
                textBox_custom_command_2.Text = config.CustomCommand2;
                textBox_custom_command_3.Text = config.CustomCommand3;

                comboBox_anser_num.SelectedIndex = config.AnswerNum - 1;
                comboBox_triger_time.SelectedItem = config.TriggerTime.ToString() + "s";
                comboBox_message_num.SelectedItem = config.MessageNum.ToString();
                comboBox_auto_write.SelectedIndex = config.EnableAutoWrite ? 0 : 1;
                comboBox_proxy_switch.SelectedIndex = config.EnableProxy ? 0 : 1;
                textBox_apikey.Text = config.ApiKey;
                textBox_proxy_addr.Text = config.ProxyAddress;

                if (CommonVariable.languageDict.ContainsKey(config.Language)) {
                    comboBox_language.SelectedItem = CommonVariable.languageDict[config.Language] + $"-({config.Language})";
                }
                //comboBox_language.SelectedItem = config.Language; // 这里的语言要加上语言前缀，否则无法正确显示
            }
            catch (Exception ex) {
                string info = $"{ResourceCulture.GetString("laod_config_error_str")}:\n{ex}";
                CALog.Instance.LogError(info);
                MessageBox.Show(info);
            }
        }

        private void FormSet_FormClosed(object sender, FormClosedEventArgs e) {
            CommonVariable.form_set = null;
        }

        private void comboBox_language_SelectedValueChanged(object sender, EventArgs e) {
            if (comboBox_language.SelectedItem != null) {
                string selectedLanguage = comboBox_language.SelectedItem.ToString(); // "中文-(zh-CN)"

                int leftIndex = selectedLanguage.IndexOf("(");
                int rightIndex = selectedLanguage.IndexOf(")");
                string language = selectedLanguage.Substring(leftIndex + 1, rightIndex - leftIndex - 1);

                CommonVariable.current_lang = language;
                ResourceCulture.SetCurrentCulture(language);
                SetResourceCulture();

                Config.Instance.Language = language;    // 保存语言到配置文件中
                Config.Instance.Save();
            }
            // 最后结果还要保存到配置文件中
        }

        public void SetResourceCulture() {
            // 后面有空再优化

            this.label_config_title.Text = ResourceCulture.GetString("FormSet_label_config_title"); //常用配置修改
            this.label_write_introduce.Text = ResourceCulture.GetString("FormSet_label_write_introduce"); //续写引导词
            this.label_custom_command_1.Text = ResourceCulture.GetString("FormSet_label_custom_command_1"); //自定义指令1
            this.label_custom_command_2.Text = ResourceCulture.GetString("FormSet_label_custom_command_2"); //自定义指令2
            this.label_custom_command_3.Text = ResourceCulture.GetString("FormSet_label_custom_command_3"); //自定义指令3
            this.label_answer_num.Text = ResourceCulture.GetString("FormSet_label_answer_num"); //候选回答
            this.label_trigger_time.Text = ResourceCulture.GetString("FormSet_label_trigger_time"); //触发时间
            this.label_message_num.Text = ResourceCulture.GetString("FormSet_label_message_num"); //消息数量
            this.label_auto_write_enable.Text = ResourceCulture.GetString("FormSet_label_auto_write_enable"); //自动续写
            this.label_proxy_enable.Text = ResourceCulture.GetString("FormSet_label_proxy_enable"); //代理开关
            this.label_proxy_address.Text = ResourceCulture.GetString("FormSet_label_proxy_address"); //代理地址
            this.label_language.Text = ResourceCulture.GetString("FormSet_label_language"); //语言
            this.label_tutorial.Text = ResourceCulture.GetString("FormSet_label_tutorial"); //使用教程
            this.label_author_info.Text = ResourceCulture.GetString("FormSet_label_author_info"); //作者信息
            this.label_version.Text = ResourceCulture.GetString("FormSet_label_version"); //软件版本
            this.button_save_config.Text = ResourceCulture.GetString("FormSet_button_save_config"); //保存配置
            this.button_reset_config.Text = ResourceCulture.GetString("FormSet_button_reset_config"); //恢复设置
            this.richTextBox_user_manual.Text = ResourceCulture.GetString("user_manual_str");// 用户手册
            this.textBox_auto_write_introduce.Text = ResourceCulture.GetString("continue_write_str"); // 续写引导词

            CommonVariable.ribbonForm.SetRibbonCulture();
        }

        //private void SetResourceCulture() {
        //    // BUG: 没有设置需要翻译的控件获取会出错
        //    // 获取窗口中所有控件
        //    foreach (var control in GetAllControls(this)) {
        //        // 获取控件的 Text 属性
        //        var property = control.GetType().GetProperty("Text");
        //        if (property != null && property.CanWrite) {
        //            // 获取资源名称并设置属性值
        //            var resourceName = this.Name + "_" + control.Name;// + "_" + property.Name;
        //            var value = ResourceCulture.GetString(resourceName);
        //            if (!string.IsNullOrEmpty(value)) {
        //                property.SetValue(control, value);
        //            }
        //        }
        //    }
        //}

        //// 获取窗口中所有控件
        //private static IEnumerable<Control> GetAllControls(Control control) {
        //    var controls = control.Controls.Cast<Control>();

        //    return controls.SelectMany(ctrl => GetAllControls(ctrl))
        //        .Concat(controls);
        //}



        private void pictureBox_github_Click(object sender, EventArgs e) {
            string url = "https://github.com/keep1234quiet/CoAuthor";
            System.Diagnostics.Process.Start(url);
        }

        private void pictureBox_youtube_Click(object sender, EventArgs e) {
            string url = "https://www.youtube.com/channel/UCjfc9nb6hLZbkWrK2iBSl9g";
            System.Diagnostics.Process.Start(url);
        }

        private void pictureBox_discord_Click(object sender, EventArgs e) {
            string url = "https://discord.gg/h4EBQgHGRe";
            System.Diagnostics.Process.Start(url);
        }

        private void pictureBox_bilibili_Click(object sender, EventArgs e) {
            string url = "https://space.bilibili.com/16497612";
            System.Diagnostics.Process.Start(url);
        }

        private void pictureBox_zhihu_Click(object sender, EventArgs e) {
            string url = "https://www.zhihu.com/people/keep1234quiet";
            System.Diagnostics.Process.Start(url);
        }

        private void pictureBox_jianshu_Click(object sender, EventArgs e) {
            string url = "https://www.jianshu.com/u/2fc23add985d";
            System.Diagnostics.Process.Start(url);
        }
    }
}
