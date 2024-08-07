using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using Whirlybird.Debugger;
using Whirlybird.PlayerSettings;
namespace Whirlybird.Localization
{
    public static class LocalizationManager
    {
        private static Dictionary<string, ResourceManager> resourceManagers = new Dictionary<string, ResourceManager>();
        private static CultureInfo currentCulture = CultureInfo.CurrentCulture;

        public static void AddResourceManager(string languageCode, ResourceManager resourceManager)
        {
            resourceManagers[languageCode] = resourceManager;
        }

        public static void SetCulture(string languageCode)
        {
            currentCulture = new CultureInfo(languageCode);
        }

        public static string GetString(string key)
        {
            
            string value = resourceManagers[currentCulture.Name].GetString(key);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            
            return key;
        }
        public static void Update()
        {
            currentCulture = new CultureInfo(Player.Preferences.LanguageCode);
        }
    }
}
