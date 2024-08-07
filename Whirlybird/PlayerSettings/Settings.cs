using Microsoft.Xna.Framework;
using Whirlybird.Localization;

namespace Whirlybird.PlayerSettings
{
    public class Settings
    {
        public string LanguageCode 
        {
            get
            {
                return languageCode;
            }
            set
            {
                languageCode = value;
            } 
        }
        public bool HardMode { get; set; }

        public Color MainColor { get; set; }
        public Color FontColor { get; set; }
        public Color SecondFontColor { get; set; }
        public Color CursorColor { get; set; }

        private string languageCode;
        public Settings()
        {
            LanguageCode = "ua";
            HardMode = false;
            MainColor = Color.Wheat;
            FontColor = Color.Black;
            SecondFontColor = Color.Crimson;
            CursorColor = Color.White;
        }
        public Settings(Settings settings)
        {
            LanguageCode = settings.LanguageCode;
            HardMode = settings.HardMode;
            MainColor = settings.MainColor;
            FontColor = settings.FontColor;
            SecondFontColor = settings.SecondFontColor;
            CursorColor = settings.CursorColor;
        }
    }
}
