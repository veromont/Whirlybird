using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Components;
using Whirlybird.Components.Input;
using Whirlybird.PlayerSettings;
using Whirlybird.Screens;

namespace Whirlybird.ScreenManaging
{
    internal class ScreenManager : DrawableGameComponent
    {
        List<GameScreen> screens = new List<GameScreen>();
        SpriteBatch spriteBatch;
        SpriteFont font;
        InputState input;
        CustomMouse customMouse;

        public ScreenManager(WhirlybirdGame game, List<GameScreen> screens)
            : base(game)
        {
            this.screens = screens;
            spriteBatch = game.SpriteBatch;
            customMouse = game.CustomMouse;
            font = game.CommonFont;
        }

        public override void Initialize()
        {
            input = new InputState();

            foreach (var screen in screens)
            {
                screen.CustomMouse = customMouse;
            }
            base.Initialize();

        }

        protected override void LoadContent()
        {

            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            input.Update();
            if (input.IsNewKeyPress(Keys.Escape))
            {
                // player.SaveProgress();
                ReturnToMenu();
                input.Clear();
            }
            foreach (GameScreen screen in screens)
            {
                if (screen.IsActive)
                {
                    screen.Update(gameTime);
                    if (screen.Transition != ScreenTransitionType.None)
                    {
                        var transitionScreen = screens.Where(s => s.Type == screen.Transition).First();
                        transitionScreen.Restart();
                        transitionScreen.ScreenState = ScreenState.Focus;

                        screen.Transition = ScreenTransitionType.None;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.IsFocused)
                {
                    screen.Draw(gameTime);
                }
            }
            base.Draw(gameTime);    
        }

        private void ReturnToMenu()
        {
            foreach (var screen in screens)
            {
                screen.ScreenState = ScreenState.Stopped;
                screen.Transition = ScreenTransitionType.None;
                if (screen.Type == ScreenTransitionType.Menu)
                {
                    screen.Restart();
                    screen.ScreenState = ScreenState.Focus;
                }
            }

        }
    }
}
