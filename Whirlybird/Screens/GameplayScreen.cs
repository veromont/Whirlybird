using System;
using System.Collections.Generic;
using System.Linq;
using Whirlybird.Components;
using Whirlybird.ScreenManaging;
using Whirlybird.Camera;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Whirlybird.Components.Leaderboard;
using Whirlybird.Localization;
using Whirlybird.PlayerSettings;
using Whirlybird.Components.Input;
using Whirlybird.Components.Platforms.Ladder;

namespace Whirlybird.Screens
{
    public class GameplayScreen : GameScreen
    {
        Bird bird;
        Score score;
        Ladder ladder;

        public float CameraLine 
        { 
            get
            {
                return camera.Position.Y - screenHeight / 2 + CameraLineOffset;
            }
        }
        private float CameraLineOffset
        {
            get
            {
                return screenHeight / 3;
            }
        }

        const float TIMETICK = 0.016666667F;
        

        public GameplayScreen(WhirlybirdGame game, Score score)
            : base(game, ScreenTransitionType.Gameplay)
        {
            this.score = score;
            Restart();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (isObjectOutOfScreen(bird.Rectangle))
            {
                ScreenState = ScreenState.Stopped;
                Transition = ScreenTransitionType.Death;
                score.IsLocked = true;
            }

            var difference = bird.Rectangle.Y + bird.Rectangle.Height - CameraLine;
            if (difference < -0.1)
            {
                UpdateCameraPosition(difference);
                UpdateScore(difference);

            }
            HandleBirdMovement(input);
            RemoveExtraPlatforms();
            ladder.Update();

            bird.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawGameplayHeader(score.Value);
            DrawCoins();

            ladder.Draw(gameTime);
            bird.Draw(gameTime);

            base.Draw(gameTime);
        }

        #region Private methods
        private void HandleBirdMovement(InputState input)
        {
            var birdCenter = bird.Rectangle.X + bird.Rectangle.Width / 2;
            if (input.CurrentMouseState.Position.X - birdCenter < 0)
            {
                bird.IsFacedLeft = true;
            }
            else if (input.CurrentMouseState.Position.X - birdCenter > 0)
            {
                bird.IsFacedLeft = false;
            }

            bird.Velocity = new Vector2(input.CurrentMouseState.Position.X - birdCenter, bird.Velocity.Y);
            if (bird.Rectangle.Y >= screenHeight - bird.Rectangle.Height)
            {
                bird.Bounce();
            }

            var collidingPlatforms = ladder.Platforms.Where(p => p.IsColliding(bird));
            if (collidingPlatforms.Count() > 0 && bird.Velocity.Y > 0)
            {
                collidingPlatforms.First().Bounce(bird);
            }
        }

        private float CalculateHeightPosition(float height)
        {
            return height - CameraLine;
        }

        private void UpdateCameraPosition(float difference)
        {
            camera.Position = new Vector2(camera.Position.X, camera.Position.Y + difference);
        }

        private void UpdateScore(float difference)
        {
            score.Value = CameraInitialPosition.Y - CameraLine;
            score.Position = new Vector2(score.Position.X, score.Position.Y + difference);
        }

        private void RemoveExtraPlatforms()
        {
            ladder.Platforms.RemoveAll(p => isObjectOutOfScreen(p.Rectangle));
        }

        private bool isObjectOutOfScreen(Rectangle rectangle)
        {
            return rectangle.Y > screenHeight + CameraLine - CameraLineOffset;
        }

        public override void Restart()
        {
            score.IsLocked = false;
            score.Value = 0;
            score.Position = new Vector2(ScreenWidth * 90 / 100, 50);
            camera.Position = CameraInitialPosition;
            bird = new Bird(game, SpriteBatch, this, CameraInitialPosition);
            ladder = new Ladder(game);
        }

        private void DrawGameplayHeader(float score)
        {
            int spacingX = 20;
            int spacingY = 20;
            var position = HeaderPosition;
            position.X = spacingX;

            SpriteBatch.DrawString(commonFont, 
                LocalizationManager.GetString("your-score"), 
                position, 
                Player.Preferences.FontColor);
            position.X += commonFont.MeasureString(LocalizationManager.GetString("your-score")).X + spacingX;

            SpriteBatch.DrawString(commonFont, 
                Math.Round(score, 2).ToString(),
                position, 
                Player.Preferences.FontColor);

            position.X = spacingX;
            position.Y += spacingY + commonFont.LineSpacing;
            SpriteBatch.DrawString(commonFont, 
                LocalizationManager.GetString("high-score"),
                position, 
                Player.Preferences.FontColor);

            position.X += commonFont.MeasureString(LocalizationManager.GetString("high-score")).X + spacingX;

            SpriteBatch.DrawString(commonFont, 
                Math.Round(leaderboard.GetHighestScore(), 2).ToString(), 
                position, 
                Player.Preferences.FontColor);
            position.X += commonFont.MeasureString(Math.Round(leaderboard.GetHighestScore(), 2).ToString()).X + spacingX;

            DrawCoins();
        }
        #endregion

    }
}
