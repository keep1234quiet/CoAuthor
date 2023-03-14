using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoAuthor.src
{
    public class Network
    {
        Config config = Config.Instance;

        public async Task<string> SendRequestAsync(string url, HttpContent content) {
            config.Load();

            bool useProxy = config.EnableProxy; // 是否使用代理
            string proxy_addr = config.ProxyAddress != "" ? config.ProxyAddress : "127.0.0.1:10809"; // 代理地址

            var clientHandler = new HttpClientHandler();
            clientHandler.SslProtocols = SslProtocols.Tls12;
            if (useProxy) {
                var proxy = new WebProxy($"http://{proxy_addr}"); // 替换成本地代理地址
                clientHandler.Proxy = proxy;
            }
            if (config.ApiKey == "") {
                //System.Windows.Forms.MessageBox.Show("CoAuthor Plugin:\nPlease enter the APIKey in the settings first");
                DialogResult result = System.Windows.Forms.MessageBox.Show($"{ResourceCulture.GetString("please_fill_apikey_str")}.\n\nI understand now, please refrain from further prompting", "CoAuthor Plugin", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK) {
                    Config.Instance.EnableAutoWrite = false;
                    Config.Instance.Save();
                }
                return "";
            }

            var client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromSeconds(CommonVariable.TIMEOUT); // 设置超时时间为60秒

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.ApiKey}");

            try {
                using (var response = await client.PostAsync(url, content)) {
                    if (response.IsSuccessStatusCode) {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else {
                        throw new Exception($"Failed to send request to {url}. Error code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex) {
                string info = $"{ResourceCulture.GetString("network_error_str")}:\n{ex}";
                System.Windows.MessageBox.Show(info);
                CALog.Instance.LogError(info);
            }
            return "";
        }
    }
}
