using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace CoAuthor
{
    class ResourceCulture
    {
        public static void SetCurrentCulture(string name) {
            if (string.IsNullOrEmpty(name)) {
                name = "en-US";
            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(name);
        }

        public static string GetString(string id) {
            string strCurLanguage = "";
            try {
                ResourceManager rm = new ResourceManager("CoAuthor.language.Lang", Assembly.GetExecutingAssembly());
                CultureInfo ci = Thread.CurrentThread.CurrentCulture;
                strCurLanguage = rm.GetString(id, ci);
            }
            catch (Exception ex) {
                strCurLanguage = $"No id:{id}, please add.({ex.Message})";
            }
            return strCurLanguage;
        }

    }
}
