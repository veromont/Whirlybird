using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.PlayerSettings;

namespace Whirlybird.Components.Input
{
    public struct StringOption
    {
        public Rectangle Rectangle { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public StringOption()
        {
            Name = string.Empty;
            Rectangle = new Rectangle();
            Info = "";
        }
        public StringOption(string name, Rectangle bounds, string info)
        {
            Name = name;
            Rectangle = bounds;
            Info = info;
        }
    }
    public class StringPicker
    {
        public StringOption SelectedOption { get; private set; }
        public StringOption FocusedOption { get; private set; }

        private List<StringOption> Options;
        private SpriteFont commonFont;
        private SpriteFont headerFont;
        private Rectangle rectangle;
        private InputState input;
        private string headerString;

        public StringPicker(string header, 
            Dictionary<string, string> options, 
            string selectedOption,
            string selectedOptionInfo,
            SpriteFont commonFont,
            SpriteFont headerFont,
            Rectangle rectangle)
        {
            Options = new List<StringOption>();
            var newRectangle = new Rectangle(rectangle.Location, rectangle.Size);
            newRectangle.Y += headerFont.LineSpacing;
            foreach (var option in options)
            {
                newRectangle.Width = (int)commonFont.MeasureString(option.Key).X;
                Options.Add(new StringOption(option.Key, newRectangle, option.Value));
                newRectangle.Y += commonFont.LineSpacing;
            }

            headerString = header;
            FocusedOption = new StringOption();
            SelectedOption = new StringOption(selectedOption, new Rectangle(), selectedOptionInfo);
            this.commonFont = commonFont;
            this.headerFont = headerFont;
            this.rectangle = rectangle;
            input = new InputState();
        }



        public void Update()
        {
            input.Update();
            FocusedOption = new StringOption();
            for (int i = 0; i < Options.Count; i++)
            {
                if (Options[i].Rectangle.Contains(input.CurrentMousePosition))
                {
                    FocusedOption = Options[i];
                    if (input.IsNewLeftMouseClick())
                    {
                        SelectedOption = Options[i];
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(headerFont, 
                headerString, 
                new Vector2(rectangle.X, rectangle.Y), 
                Player.Preferences.FontColor);

            for (int i = 0; i < Options.Count; i++)
            {
                Color color = Options[i].Name == 
                    SelectedOption.Name ? 
                    Color.DarkOrange : 
                    (Options[i].Name == FocusedOption.Name ? 
                    Player.Preferences.SecondFontColor : 
                    Player.Preferences.FontColor);

                spriteBatch.DrawString(commonFont, 
                    Options[i].Name, 
                    new Vector2(Options[i].Rectangle.X, 
                    Options[i].Rectangle.Y), 
                    color);
            }
        }
    }
}
