using System.Collections.Generic;

namespace CoAuthor.src
{
    public class CommonVariable
    {
        public const int WM_KEYDOWN = 0x0100;

        public static int input_count_total = 0; // 统计输入了多少次
        public static int input_count_after_wait = 0; // 统计暂停后了多少次
        public static int user_wait_secode = 0; // 用户等待的秒数

        public static bool wait_accept = false; // 是否等待用户接受建议内容
        public static int complete_str_length = 0; // 建议内容的长度

        public static int cursor_position_after_hint = 0; // 出现提示时的光标位置
        public static int cursor_position_when_request = 0; // 记录发起请求时光标的位置，再插入提示内容时检查光标位置是否改变，如果改变了，就算请求回来了也不插入，直接丢弃
        public static int cursor_move_num = 0; // 用于记录提示后用户移动了多远的光标

        public static string hintTextByChatGPT = "";
        public static string[] hintTextsByChatGPT = new string[3];
        public static int hint_idx = 0; // 这个用来记录取了hintTextsByChatGPT数组中第几个元素
        public static int ANSWER_NUM = 1; // 默认需要备选回答的数量
        public static int PRE_PARAGRAPH_NUM = 2; // 读取前两段的内容，太多了耗时太久，消耗token过快
        public static int MAX_MESSAGES_NUM = 6; // messages中最多保存MAX_MESSAGES_NUM/2次对话的内容

        public static int TIMEOUT = 50; // 网络请求超时 50s
        public static int MAX_TOKENS = (int)(4000 * 0.7); // 最多发送2800个字符，约4000个token，根据openai的规则计算(也许不太准确)

        public static string API_KEY = "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";
        public static string API_ENDPOINT = "https://api.openai.com/v1/chat/completions";

        public static string current_lang = "en-US";
        public static List<string> languages = new List<string>() { "中文-(zh-CN)", "English-(en-US)", "Español-(es-ES)", "हिन्दी-(hi-IN)", "العربية-(ar-SA)", "Português-(pt-BR)", "বাংলা-(bn-BD)", "Русский-(ru-RU)", "日本語-(ja-JP)", "Deutsch-(de-DE)", "Français-(fr-FR)", "اردو-(ur-PK)", "Italiano-(it-IT)", "فارسی-(fa-IR)", "Türkçe-(tr-TR)", "தமிழ்-(ta-IN)", "Afrikaans-(af-ZA)", "한국어-(ko-KR)", "BahasaIndonesia-(id-ID)", "Українська-(uk-UA)" };
        public static List<string> languages_simple = new List<string> { "zh-CN", "en-US", "es-ES", "hi-IN", "ar-SA", "pt-BR", "bn-BD", "ru-RU", "ja-JP", "de-DE", "fr-FR", "ur-PK", "it-IT", "fa-IR", "tr-TR", "ta-IN", "af-ZA", "ko-KR", "id-ID", "uk-UA" };


        public static int memory_recovery_interval = 60 * 3; // 3分钟回收一次内存

        public static Dictionary<string, string> languageDict = new Dictionary<string, string>();

        public static System.Timers.Timer _timer;
        public static FormSet form_set;
        public static RibbonChatGPT ribbonForm;
    }
}
