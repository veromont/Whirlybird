using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Debugger;
using Whirlybird.PlayerSettings;
using Whirlybird.ScreenManaging;

namespace Whirlybird.Components.Input
{
    public class Checkbox
    {
        public bool IsChecked { get; set; }
        public Rectangle Rectangle { get; private set; }

        private InputState input;

        public Checkbox(InputState state, Rectangle rectangle)
        {
            input = state;
            Rectangle = rectangle;
            IsChecked = false;
        }

        public void Update()
        {

            if (input.IsNewLeftMouseClick() && Rectangle.Contains(input.CurrentMousePosition))
            {
                IsChecked = !IsChecked;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameScreen screen)
        {
            int thickness = 1;

            screen.DrawRectangle(Rectangle, Player.Preferences.FontColor);
            screen.DrawRectangle(new Rectangle(Rectangle.X + thickness, 
                Rectangle.Y + thickness, 
                Rectangle.Width - thickness * 2, 
                Rectangle.Height - thickness * 2)
                , Color.White);

            if (IsChecked) 
            {
                screen.DrawLine(new Vector2(Rectangle.X, Rectangle.Y),
                    new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height),
                    thickness,
                    Player.Preferences.FontColor);
                screen.DrawLine(new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y),
                    new Vector2(Rectangle.X, Rectangle.Y + Rectangle.Height),
                    thickness,
                    Player.Preferences.FontColor);
            }
        }
    }
}
