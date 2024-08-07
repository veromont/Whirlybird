using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Components;
using Whirlybird.Localization;
using Whirlybird.PlayerSettings;
using Whirlybird.ScreenManaging;
using Whirlybird.Components.Input;
using System.Reflection.Emit;
using Whirlybird.Debugger;

namespace Whirlybird.Screens
{
    public class SettingsScreen : GameScreen
    {
        Settings LocalSettings;

        Checkbox hardmodeCheckbox;
        List<Checkbox> checkboxes;

        ColorPicker mainColorPicker;
        ColorPicker fontColorPicker;
        ColorPicker secondFontColorPicker;
        ColorPicker cursorColorPicker;
        List<ColorPicker> colorPickers;

        StringPicker languagePicker;
        List<StringPicker> stringPickers;

        Rectangle confirmRectangle;
        bool isConfirmButtonFocused;
        public SettingsScreen(WhirlybirdGame game)
            : base(game, ScreenTransitionType.Settings)
        {
            InitializeComponents();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!confirmRectangle.Contains(input.CurrentMousePosition))
            {
                CustomMouse.IsFocused = false;
                isConfirmButtonFocused = false;
            }

            if (input.IsNewLeftMouseClick())
            {
                LocalSettings.HardMode = hardmodeCheckbox.IsChecked;
                LocalSettings.MainColor = mainColorPicker.CurrentColor;
                LocalSettings.FontColor = fontColorPicker.CurrentColor;
                LocalSettings.SecondFontColor = secondFontColorPicker.CurrentColor;
                LocalSettings.CursorColor = cursorColorPicker.CurrentColor;
                LocalSettings.LanguageCode = languagePicker.SelectedOption.Info;
            }


            if (hardmodeCheckbox.Rectangle.Contains(input.CurrentMousePosition) || 
                colorPickers.Any(c => c.ColorRect.Contains(input.CurrentMousePosition)))
            {
                CustomMouse.IsFocused = true;
            }
            else if (confirmRectangle.Contains(input.CurrentMousePosition))
            {
                CustomMouse.IsFocused = true;
                isConfirmButtonFocused = true;
                if (input.IsNewLeftMouseClick())
                {
                    Player.Preferences = LocalSettings;
                    Transition = ScreenTransitionType.Menu;
                    ScreenState = ScreenState.Stopped;
                }
            }
            else
            {
                CustomMouse.IsFocused = false;
            }

            foreach (var colorPicker in colorPickers)
            {
                colorPicker.Update(gameTime);
            }
            foreach (var checkbox in checkboxes)
            {
                checkbox.Update();
            }
            foreach (var stringPicker in stringPickers)
            {
                stringPicker.Update();
            }
            base.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            DrawHeader(LocalizationManager.GetString("settings"), 2);
            string isOn = hardmodeCheckbox.IsChecked ? 
                LocalizationManager.GetString("on") : 
                LocalizationManager.GetString("off");

            DrawCheckbox($"{LocalizationManager.GetString("hardmode-pick")} {isOn}", hardmodeCheckbox);

            foreach (var colPick in colorPickers)
            {
                DrawColorPicker(colPick, gameTime);
            }

            foreach (var stringPicker in stringPickers)
            {
                stringPicker.Draw(SpriteBatch);
            }

            DrawConfirmRectangle();

            base.Draw(gameTime);
        }

        public override void Restart()
        {
            InitializeComponents();
            base.Restart();
        }

        private void DrawColorPicker(ColorPicker colorPicker, GameTime gametime)
        {
            int offset = 10;

            SpriteBatch.DrawString(commonFont, colorPicker.AssociatedString,
                new Vector2(colorPicker.ColorRect.X - commonFont.MeasureString(colorPicker.AssociatedString).X - offset,
                colorPicker.ColorRect.Y),
                LocalSettings.FontColor);
            colorPicker.Draw(gametime);
        }

        private void DrawCheckbox(string associatedString, Checkbox checkbox)
        {
            int offset = 10;

            SpriteBatch.DrawString(commonFont, associatedString,
                new Vector2(checkbox.Rectangle.X - commonFont.MeasureString(associatedString).X - offset,
                checkbox.Rectangle.Y),
                LocalSettings.FontColor);
            checkbox.Draw(SpriteBatch, this);
        }

        private void DrawConfirmRectangle()
        {
            string text = LocalizationManager.GetString("confirm-changes");
            confirmRectangle = new Rectangle((int)(ScreenWidth - commonFont.MeasureString(text).X - 40),
                (int)(screenHeight - commonFont.LineSpacing - 40),
                (int)(commonFont.MeasureString(text).X),
                (int)(commonFont.LineSpacing));
            DrawRectangle(confirmRectangle, LocalSettings.FontColor);
            int border = 2;
            confirmRectangle = new Rectangle(confirmRectangle.X + border,
                confirmRectangle.Y + border,
                confirmRectangle.Width - 2 * border,
                confirmRectangle.Height - 2 * border);
            DrawRectangle(confirmRectangle, LocalSettings.MainColor);

            SpriteBatch.DrawString(commonFont,
                text,
                new Vector2(confirmRectangle.X, confirmRectangle.Y),
                isConfirmButtonFocused ? LocalSettings.SecondFontColor : LocalSettings.FontColor);
        }

        private void InitializeComponents()
        {
            int size = screenHeight / 20;
            int offset = size + 20;
            var SettingRectangle = new Rectangle(ScreenWidth - size - offset, ScreenWidth / 10, size, size);

            hardmodeCheckbox = new Checkbox(input, SettingRectangle);
            SettingRectangle.Y += offset;

            mainColorPicker = new ColorPicker(game, 
                SettingRectangle, 
                Player.Preferences.MainColor, 
                input,
                LocalizationManager.GetString("main-colorpick"));
            SettingRectangle.Y += offset;

            fontColorPicker = new ColorPicker(game,
                SettingRectangle,
                Player.Preferences.FontColor,
                input,
                LocalizationManager.GetString("font-colorpick"));
            SettingRectangle.Y += offset;

            secondFontColorPicker = new ColorPicker(game,
                SettingRectangle,
                Player.Preferences.SecondFontColor,
                input,
                LocalizationManager.GetString("second-font-colorpick"));
            SettingRectangle.Y += offset;

            cursorColorPicker = new ColorPicker(game,
                SettingRectangle,
                Player.Preferences.CursorColor,
                input,
                LocalizationManager.GetString("cursor-colorpick"));
            SettingRectangle.Y += offset;


            SettingRectangle = new Rectangle(ScreenWidth / 20, screenHeight / 5, size * 3, size);

            languagePicker = new StringPicker(LocalizationManager.GetString("choose-language"),
            new Dictionary<string, string>
            {
                { LocalizationManager.GetString("ua"), "ua" },
                { LocalizationManager.GetString("en"), "en" },
                { LocalizationManager.GetString("es"), "es" },
            },
            LocalizationManager.GetString(Player.Preferences.LanguageCode),
            Player.Preferences.LanguageCode,
            commonFont,
            game.HeaderFont,
            SettingRectangle);

            colorPickers = new List<ColorPicker>
            {
                mainColorPicker,
                fontColorPicker,
                secondFontColorPicker,
                cursorColorPicker,
            };

            checkboxes = new List<Checkbox>
            {
                hardmodeCheckbox,
            };

            stringPickers = new List<StringPicker>
            {
                languagePicker,
            };

            LocalSettings = new Settings(Player.Preferences);


        }
    }
}
