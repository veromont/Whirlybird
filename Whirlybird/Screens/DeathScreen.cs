using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Whirlybird.Screens
{
    public enum ScreenState
    {
        Active,
        Hidden,
    }
    internal class DeathScreen
    {

        public DeathScreen(WhirlybirdGame game)
        {
            this.game = game;
        }
        public bool IsActive { get { return screenState == ScreenState.Active; } } 
        public ScreenState ScreenState
        {
            get { return screenState; }
            set { screenState = value; }
        }

        ScreenState screenState = ScreenState.Hidden;

        private SpriteFont font;
        private WhirlybirdGame game;
        public void LoadContent()
        {
            font = game.Content.Load<SpriteFont>("constantia");
        }
        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mState = Mouse.GetState();
            var pressedKeys = keyboardState.GetPressedKeys();
            if (pressedKeys.Length > 0 || mState.LeftButton == ButtonState.Pressed || mState.RightButton == ButtonState.Pressed)
            {
                game.Restart();
                screenState = ScreenState.Hidden;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "GG YOU LOSE", new Vector2(600, 100), Color.Black);
            spriteBatch.DrawString(font, "PRESS ANY KEY TO RESTART", new Vector2(600, 200), Color.Black);

        }
    }
}
