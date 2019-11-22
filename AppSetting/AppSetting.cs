using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace ADTool.AppSetting
{
    /// <summary>
    /// This class is used for get value or section of type System.Configuration.NameValueSectionHandler inside of config file.
    /// </summary>
    public class AppSetting
    {
        /// <summary>
        /// Get value of key setting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSetting(string key)
        {
            string result = String.Empty;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "Not Found";
            }
            catch (ConfigurationErrorsException)
            {
                result = "Can't recover a NameValueCollection object with the configuracion app data";
            }

            return result;
        }

        /// <summary>
        /// Get section of config file
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllSettings(string section = null)
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            try
            {
                var appSettings = String.IsNullOrEmpty(section) ? ConfigurationManager.AppSettings
                    : ConfigurationManager.GetSection(section) as NameValueCollection;

                if (appSettings != null && appSettings.Count > 0)
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        settings.Add(key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new Exception("Can't recover a NameValueCollection object with the configuracion app data", ex.InnerException);
            }
            return settings;
        }

    }
}
