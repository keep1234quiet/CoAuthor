using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CoAuthor.src
{
    public class Config
    {
        public int AnswerNum { get; set; } = 1;
        public int TriggerTime { get; set; } = 3;
        public string ApiKey { get; set; } = "";
        public bool EnableAutoWrite { get; set; } = true;
        public string WriteIntroduce { get; set; } = ResourceCulture.GetString("continue_write_str");
        public string CustomCommand1 { get; set; } = "";
        public string CustomCommand2 { get; set; } = "";
        public string CustomCommand3 { get; set; } = "";
        public string Language { get; set; } = "";
        public int MessageNum { get; set; } = 6;
        public bool EnableProxy { get; set; } = false;
        public string ProxyAddress { get; set; } = "127.0.0.1:10809";

        private string folderPath = @"C:/CoAuthor";
        private string full_path = @"C:/CoAuthor/config.ini";

        private static Config instance = null;

        private Config() {
        }

        public static Config Instance {
            get {
                if (instance == null) {
                    instance = new Config();
                    instance.Load();
                }
                return instance;
            }
        }

        public object[] Save() {
            object[] results = new object[] { true, "" };
            try {
                if (!Directory.Exists(folderPath)) {
                    Directory.CreateDirectory(folderPath);
                }
                string json = JsonConvert.SerializeObject(this);
                File.WriteAllText(this.full_path, json);
            }
            catch (Exception ex) {
                results[0] = false;
                results[1] = ex.ToString();
                return results;
            }
            return results;
        }

        public Dictionary<string, object> Load() {
            Dictionary<string, object> results = new Dictionary<string, object>()
            {
                { "Status", true },
                { "ErrorMessage", "" }
            };
            try {
                if (File.Exists(this.full_path)) {
                    string json = File.ReadAllText(this.full_path);
                    Config config = JsonConvert.DeserializeObject<Config>(json);
                    Type configType = typeof(Config);
                    PropertyInfo[] properties = configType.GetProperties();
                    foreach (PropertyInfo property in properties) {
                        object value = property.GetValue(config, null);
                        property.SetValue(instance, value);
                        results.Add(property.Name, value);
                    }
                }
            }
            catch (Exception ex) {
                results["Status"] = false;
                results["ErrorMessage"] = ex.ToString();
            }
            return results;
        }
    }
}
