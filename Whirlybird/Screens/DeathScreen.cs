using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics.CodeAnalysis;
using Whirlybird.Camera;
using Whirlybird.Components;
using Whirlybird.Components.Input;
using Whirlybird.Components.Leaderboard;
using Whirlybird.Localization;
using Whirlybird.PlayerSettings;
using Whirlybird.ScreenManaging;

namespace Whirlybird.Screens
{
    internal class DeathScreen : GameScreen
    {
        TextInputHandler textInputHandler;
        Score score;
        bool isInputActive;
        Texture2D birdSprite;

        public DeathScreen(WhirlybirdGame game, Score score)
            : base(game, ScreenTransitionType.Death)
        {
            textInputHandler = new TextInputHandler(input, LocalizationManager.GetString("input-name-invitation"), this);
            Transition = ScreenTransitionType.None;
            this.score = score;
        }

        public override void LoadContent()
        {
            birdSprite = game.Content.Load<Texture2D>("bird\\common\\common-right");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            textInputHandler.Update();
            if (textInputHandler.IsInputFinished && isInputActive)
            {
                leaderboard.AddRecord(textInputHandler.Input, score.Value);
                isInputActive = false;
                input.Clear();
            }
            if (leaderboard.MinValue < score.Value && !textInputHandler.IsInputFinished)
            {
                isInputActive = true;
            }
            if (!isInputActive && (input.GetNewPressedKey() == Keys.R || input.IsNewLeftMouseClick() || input.WasRightMouseClicked()))
            {
                ScreenState = ScreenState.Stopped;
                Transition = ScreenTransitionType.Gameplay;
            }
            base.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {


            if (isInputActive)
            {
                SpriteBatch.DrawString(commonFont, 
                    $"{LocalizationManager.GetString("your-result")} {leaderboard.GetPosition(score.Value)}",
                    new Vector2(ScreenWidth / 4, screenHeight * 1 / 4), 
                    Player.Preferences.FontColor);

                textInputHandler.Draw(SpriteBatch, commonFont, ScreenWidth / 4, screenHeight * 1 / 4 + 70);
            }
            else
            {
                var restartString = LocalizationManager.GetString("press-to-restart");
                var position = new Vector2((ScreenWidth - commonFont.MeasureString(restartString).X) / 2, 
                    screenHeight * 4 / 5);

                int textureHeight = screenHeight / 2;
                int textureWidth = textureHeight * 25 / 60;
                
                SpriteBatch.Draw(birdSprite, 
                    new Rectangle((ScreenWidth - textureWidth) / 2, 
                    screenHeight / 7, 
                    textureWidth, 
                    textureHeight), 
                    Color.White);

                SpriteBatch.DrawString(commonFont, 
                    restartString, 
                    position, 
                    Player.Preferences.FontColor);

                var menuString = LocalizationManager.GetString("menu-invitation");
                position.Y += commonFont.LineSpacing;
                position.X = (ScreenWidth - commonFont.MeasureString(menuString).X) / 2;
                SpriteBatch.DrawString(commonFont,
                    menuString, position,
                    Player.Preferences.FontColor);
            }
            base.Draw(gameTime);
        }

        public override void Restart()
        {
            textInputHandler = new TextInputHandler(input, LocalizationManager.GetString("input-name-invitation"), this);
            isInputActive = true;
            base.Restart();

        }
    }
}
