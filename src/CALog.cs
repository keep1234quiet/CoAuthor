using Serilog;
using System;
using System.Diagnostics;
using System.IO;


namespace CoAuthor.src
{
    //public static class CALog
    //{
    //    private static string logpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Logs\CoAuthorLog_{DateTime.Now:yyyy-MM-dd}.txt");
    //    public static void LogError(string content) {
    //        Log.Logger = new LoggerConfiguration()
    //            .MinimumLevel.Error()
    //            .WriteTo.File(logpath)
    //            .CreateLogger();

    //        Log.Error(content);
    //    }
    //}
    public sealed class CALog
    {
        private static readonly CALog instance = new CALog();
        private static string logpath;
        private readonly ILogger logger;

        private CALog() {
            try {
                logpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Logs\CoAuthorLog_{DateTime.Now:yyyy-MM-dd}.txt");

                logger = new LoggerConfiguration()
                    .MinimumLevel.Error()
                    .WriteTo.File(logpath)
                    .CreateLogger();
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        public static CALog Instance {
            get {
                return instance;
            }
        }

        public void LogError(string content) {
            logger.Error(content);
        }
    }

}



