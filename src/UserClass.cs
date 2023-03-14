using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace CoAuthor.src
{
    public class UserClass
    {
        public List<dynamic> messages = new List<dynamic>();
        private Network network = null;


        public UserClass() {
            network = new Network();
        }

        #region 计时器
        public void StartTimer() {
            try {
                if (CommonVariable._timer == null) {
                    CommonVariable._timer = new System.Timers.Timer(1000);
                    CommonVariable._timer.Elapsed += async (sender, e) => await Timer_Tick();
                }
                CommonVariable._timer.Start();
            }
            catch (Exception ex) {
                CALog.Instance.LogError($"Error happen in StartTimer or Timer_Tick:\n{ex}");
            }
        }
        public void ResetTimer() {
            if (CommonVariable._timer != null) {
                CommonVariable._timer.Stop(); // 停止计时器
                CommonVariable._timer.Dispose(); // 释放计时器
                CommonVariable._timer = null;
            }
        }

        public async Task Timer_Tick() {
            try {
                if (CommonVariable.form_set != null)
                    return;
                CommonVariable.user_wait_secode += 1;

                MemoryManager.FlushMemory();

                // Debug.WriteLine($"user_wait_secode={CommonVariable.user_wait_secode}");
                if (CommonVariable.user_wait_secode >= Config.Instance.TriggerTime) {
                    CommonVariable.user_wait_secode = 0; // 重置计时器
                    CommonVariable.input_count_after_wait = 0; // 清空这次输入的字符数
                    CommonVariable._timer.Stop(); // 停止时钟，下次输入时再启动

                    await GetParagraph();
                }
            }
            catch (Exception ex) {
                CALog.Instance.LogError($"Timer_Tick error:\n{ex}");
            }
            // TODO: 每个时间间隔时执行的操作
        }
        #endregion


        public bool ProcessKeydownEvent(IntPtr wParam) {

            char input_char = Convert.ToChar((int)wParam);
            bool isLetterOrDigit = char.IsLetterOrDigit(input_char);// || char.IsPunctuation(input_char) || char.IsSymbol(input_char);// || char.IsWhiteSpace(input_char);
            const bool CONTINUE_PROCESS_KEY_EVENT = true;
            const bool STOP_PROCESS_KEY_EVENT = false;


            if (isLetterOrDigit) {
                CommonVariable.input_count_total += 1;
                CommonVariable.input_count_after_wait += 1;
                if (CommonVariable.input_count_total > 10 && CommonVariable.input_count_after_wait > 6) {
                    StartTimer();
                    CommonVariable.user_wait_secode = 0;
                }
            }

            /*
                在这里处理按键按下后想要做的事
            wParam的值:
            tab键: 0x09
            esc键: 0x1b
            enter键: 0x0d
            左箭头: 0x25
            上箭头: 0x26
            右箭头: 0x27
            下箭头: 0x28
            删除: 0x08
                */
            //Debug.WriteLine($"wParam={wParam}, char={Convert.ToChar((int)wParam)}");

            //插件提示出现内容，在等待用户输入期间，用户如果继续输入，检测输入内容是否与插件内容一致，否则删除提示的内容
            //if (isLastKeyUp && isLetterOrDigit && wait_accept && (int)wParam != 0x27 && cursor_position_after_hint < selectionRange.Start) {
            //    // 检测输入是否导致光标向后移动(不是按右键导致光标的移动)
            //    Word.Range new_word_range = doc.Range(cursor_position_after_hint, selectionRange.Start); // 插入的字符数
            //    // 判断新输入的字符是否和ChatGPT提示的前面部分字符一致(忽略大小写)
            //    if(hintTextByChatGPT.StartsWith(new_word_range.Text, StringComparison.OrdinalIgnoreCase)) {
            //        //Word.Range pre_input_range = doc.Range(selectionRange.Start, selectionRange.Start + new_word_range.Text.Length);
            //        Word.Range pre_input_range = doc.Range(selectionRange.Start, selectionRange.Start + 1);
            //        pre_input_range.Delete();
            //        //complete_str_length -= new_word_range.Text.Length;
            //        //hintTextByChatGPT = hintTextByChatGPT.Substring(new_word_range.Text.Length, complete_str_length);
            //    }
            //    else { /* 如果输入的不一致，说明提示的内容没有帮助，删掉提示的内容 */
            //        range = doc.Range(selectionRange.Start, selectionRange.Start + complete_str_length);
            //        range.Delete();
            //        wait_accept = false;
            //        complete_str_length = 0;
            //    }
            //}

            // 获取当前活动文档
            Word.Document doc = null;
            Word.Range selectionRange = null, range = null;

            if (!CommonVariable.wait_accept)
                return CONTINUE_PROCESS_KEY_EVENT;

            try {
                doc = Globals.ThisAddIn.Application.ActiveDocument;
                // 获取当前选中范围
                selectionRange = Globals.ThisAddIn.Application.Selection.Range;
                range = doc.Range(CommonVariable.cursor_position_after_hint, CommonVariable.cursor_position_after_hint + CommonVariable.complete_str_length);
            }
            catch {
                CommonVariable.wait_accept = false; // ChatGPT出现提示后，如果用户不按ESC，而是直接删除提示的内容，就会出现这个异常，这时候就不再等待用户输入了
                //System.Windows.MessageBox.Show("range error!");
                return CONTINUE_PROCESS_KEY_EVENT;
            }

            switch ((int)wParam) {
                case 0x09: // TAB键
                    range.Font.Color = Word.WdColor.wdColorAutomatic;

                    // 获取当前段落文字的字体样式
                    // Word.Font previousFont = selectionRange.Paragraphs[1].Range.Font;
                    Word.Font previousFont = doc.Characters[selectionRange.Start - 1].Font;

                    // 设置插入文字的字体样式为光标前面文字的字体样式
                    range.Font.Name = previousFont.Name;
                    range.Font.Size = previousFont.Size;
                    range.Font.Bold = previousFont.Bold;
                    range.Font.Italic = previousFont.Italic;
                    range.Font.Underline = previousFont.Underline;
                    range.Font.Color = previousFont.Color;

                    // 移动光标到刚插入的文字末尾
                    object position = CommonVariable.cursor_position_after_hint + CommonVariable.complete_str_length;
                    range = doc.Range(ref position, ref position);
                    range.Select();

                    CommonVariable.cursor_position_after_hint = selectionRange.Start;
                    CommonVariable.complete_str_length = 0;
                    CommonVariable.wait_accept = false; // 用户已接受建议内容，重置等待接收标志
                    return STOP_PROCESS_KEY_EVENT; // 不将事件继续传递下去
                case 0x1b: // ESC键
                    if (CommonVariable.wait_accept == false) {
                        break;
                    }
                    range.Delete();

                    CommonVariable.complete_str_length = 0;
                    CommonVariable.wait_accept = false; // 用户已拒绝建议内容，重置等待接收标志
                    return STOP_PROCESS_KEY_EVENT; // 不将事件继续传递下去
                case 0x26: // 上箭头
                    if (CommonVariable.wait_accept == false)
                        break;
                    range.Delete();

                    CommonVariable.hint_idx = (--CommonVariable.hint_idx + Config.Instance.AnswerNum) % Config.Instance.AnswerNum;
                    CommonVariable.hintTextByChatGPT = CommonVariable.hintTextsByChatGPT[CommonVariable.hint_idx]; // 取下一个元素
                    CommonVariable.complete_str_length = CommonVariable.hintTextByChatGPT.Length;

                    range.InsertAfter(CommonVariable.hintTextByChatGPT); // 或者可以使用 selectionRange.TypeText(hintTextByChatGPT
                    range = doc.Range(selectionRange.Start, selectionRange.Start + CommonVariable.hintTextByChatGPT.Length);
                    range.Font.Color = Word.WdColor.wdColorGray375;

                    return STOP_PROCESS_KEY_EVENT; // 不将事件继续传递下去
                case 0x28: // 下箭头
                    if (CommonVariable.wait_accept == false)
                        break;
                    range.Delete();
                    /*
                     这里有个小BUG，每次删除都会留下一个换行符，导致每一次按都会多一个换行符出来
                     */

                    CommonVariable.hint_idx = (++CommonVariable.hint_idx + Config.Instance.AnswerNum) % Config.Instance.AnswerNum;
                    CommonVariable.hintTextByChatGPT = CommonVariable.hintTextsByChatGPT[CommonVariable.hint_idx]; // 取下一个元素
                    CommonVariable.complete_str_length = CommonVariable.hintTextByChatGPT.Length;

                    range.InsertAfter(CommonVariable.hintTextByChatGPT); // 或者可以使用 selectionRange.TypeText(hintTextByChatGPT
                    range = doc.Range(selectionRange.Start, selectionRange.Start + CommonVariable.hintTextByChatGPT.Length);
                    range.Font.Color = Word.WdColor.wdColorGray375;

                    return STOP_PROCESS_KEY_EVENT; // 不将事件继续传递下去
                case 0x27: // 右箭头
                    break;
                case 0x08: // 删除
                    if (CommonVariable.wait_accept && selectionRange.Start < CommonVariable.cursor_position_after_hint) {
                        CommonVariable.cursor_position_after_hint -= 1;// 如果是一个一个删除那可以这么做
                        // TODO: 如果是在前面选中后再删除，那又不止一个了
                        break;
                    }

                    break;
                default:
                    return CONTINUE_PROCESS_KEY_EVENT;
            }
            return CONTINUE_PROCESS_KEY_EVENT;

        }


        #region 文档操作
        private async Task GetParagraph() {
            try {
                // 获取当前活动文档
                Word.Document doc = Globals.ThisAddIn.Application.ActiveDocument;
                // 获取当前选中范围
                Word.Range selectionRange = Globals.ThisAddIn.Application.Selection.Range;

                Word.Paragraph previousParagraph;

                // 获取当前所在段落
                Word.Paragraph currentParagraph = selectionRange.Paragraphs[1];
                // 获取前面n个段落的文本
                for (int paragraph_num = CommonVariable.PRE_PARAGRAPH_NUM; paragraph_num > 0; paragraph_num--) { // 获取前5个段落
                    try {
                        previousParagraph = currentParagraph.Previous(paragraph_num);
                        if (previousParagraph == null)
                            break;
                        //Debug.WriteLine($"paragraph_num={paragraph_num}, previous_Paragraphs={previousParagraph.Range.Text}");
                        messages.Add(new { role = "user", content = previousParagraph.Range.Text }); // 添加前面几段的内容
                    }
                    catch {
                        /* 前面不一定有5个段落，没有还去读就会异常 */
                    }
                }

                // 在当前光标处插入文本
                if (CommonVariable.wait_accept == false) {
                    CommonVariable.cursor_position_when_request = selectionRange.Start; // 记录发起请求前光标的位置

                    // Debug.WriteLine("Ask for ChatGPT...");

                    await Task.Run(() => {
                        System.Action action = () => { FormLoading.form_loading.Loading(); };
                        FormLoading.form_loading.Invoke(action);
                    });

                    var selection = Globals.ThisAddIn.Application.Selection;
                    // 判断选择是否位于表格内
                    //if (selectionRange.Tables.Count > 0) {
                    //    var tableRange = selectionRange.Tables[1].Range;
                    //    if (selectionRange.InRange(tableRange))  // 判断选择范围是否在表格区域内
                    //    {
                    //        // 这里建议加个checkbox手动选择光标在表格内是否去填数据，有时候又大表格
                    //        // 代码处理逻辑
                    //        // MessageBox.Show("光标当前落在表格内");
                    //    }
                    //}

                    string command_content = Config.Instance.WriteIntroduce;

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    CommonVariable.hintTextsByChatGPT = await GetHintsByChatGPT(command_content + ":" + currentParagraph.Range.Text);
                    if (CommonVariable.hintTextsByChatGPT.Length != 0) {
                        CommonVariable.hintTextByChatGPT = CommonVariable.hintTextsByChatGPT[CommonVariable.hint_idx]; // 默认取数组中第0个元素
                    }

                    stopwatch.Stop();
                    TimeSpan timeSpan = stopwatch.Elapsed;
                    //Debug.WriteLine("请求耗时（毫秒）：" + timeSpan.TotalMilliseconds);

                    await Task.Run(() => {
                        System.Action action = () => { FormLoading.form_loading.HideLoading(); };
                        FormLoading.form_loading.Invoke(action);
                    });


                    int new_cursor_pos = Globals.ThisAddIn.Application.Selection.Range.Start;
                    if (CommonVariable.cursor_position_when_request != new_cursor_pos)
                        return;
                    selectionRange.InsertAfter(CommonVariable.hintTextByChatGPT); // 或者可以使用 selectionRange.TypeText(hintTextByChatGPT

                    CommonVariable.complete_str_length = CommonVariable.hintTextByChatGPT.Length;

                    var range = doc.Range(selectionRange.Start, selectionRange.Start + CommonVariable.hintTextByChatGPT.Length);
                    range.Font.Color = Word.WdColor.wdColorGray375;

                    CommonVariable.cursor_position_after_hint = selectionRange.Start; // 记录出现提示时的光标位置
                    CommonVariable.wait_accept = true;
                }
            }
            catch (Exception ex) {
                CALog.Instance.LogError($"GetParagraph Error:\n{ex}");
            }
        }
        #endregion


        #region 网络请求相关

        public async Task<string[]> GetHintsByChatGPT(string input_content) {
            try {
                var messages_str = JsonConvert.SerializeObject(messages);

                // 检查 json 的长度
                while (messages_str.Length > CommonVariable.MAX_TOKENS) {
                    // 删除第一条添加的消息,直道要发送的消息符合长度要求
                    messages.RemoveAt(0);
                    messages_str = JsonConvert.SerializeObject(messages);
                }

                messages.Add(new { role = "user", content = input_content });
                var requestData = new { model = "gpt-3.5-turbo", messages, n = Config.Instance.AnswerNum };
                var jsonRequest = JsonConvert.SerializeObject(requestData);
                var body = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                string jsonResponse = await this.network.SendRequestAsync(CommonVariable.API_ENDPOINT, body);
                if (jsonResponse == "") {
                    return new string[0];
                }
                // 反序列化输入的 JSON 数据
                JObject json = JObject.Parse(jsonResponse);
                // 获取所有的 'message' 中的 'content'
                JArray choices = (JArray)json["choices"];
                List<string> contents_parsed = new List<string>();
                foreach (var choice in choices) {
                    JObject message = (JObject)choice["message"];
                    string content_parsed = ((string)message["content"]).Trim();
                    content_parsed = Regex.Replace(content_parsed, "\n{2,}", "\n");
                    contents_parsed.Add(content_parsed);
                }
                // 将历史消息记录到 message 中，但只保留最近的 10 条

                messages.Add(new { role = "assistant", content = contents_parsed[0] }); // 仅保存第0条记录
                if (messages.Count > Config.Instance.MessageNum) { // 只记录10条历史消息, 太多了会快速消耗token
                    messages.RemoveAt(0); // 移除2个, 因为每次都会添加2条记录
                    messages.RemoveAt(0);
                }
                //Console.WriteLine($"content_parsed={content_parsed}");

                // 将结果作为字符串数组返回
                return contents_parsed.ToArray();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                System.Windows.MessageBox.Show(ex.Message);
            }
            return new string[0];
        }
        #endregion
    }
}
