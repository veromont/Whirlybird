using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Whirlybird.Camera;
using Whirlybird.Components;
using Whirlybird.Components.Input;
using Whirlybird.Components.Leaderboard;
using Whirlybird.PlayerSettings;

namespace Whirlybird.ScreenManaging
{
    public enum ScreenState
    {
        Focus,
        Active,
        Stopped,
    }
    public enum ScreenTransitionType
    {
        None,
        Gameplay,
        Death,
        Leaderboard,
        Menu,
        Shop,
        Settings,
    }
    public abstract class GameScreen
    {
        #region Properties   
        public bool IsFocused 
        { 
            get 
            { 
                return ScreenState == ScreenState.Focus; 
            } 
        }
        public bool IsActive 
        { 
            get 
            { 
                return ScreenState == ScreenState.Active || IsFocused; 
            } 
        }
        public Vector2 CameraInitialPosition
        {
            get
            {
                return new Vector2(ScreenWidth / 2, screenHeight / 2);
            }
        }

        public ScreenTransitionType Transition { get; set; } = ScreenTransitionType.None;
        public int ScreenWidth { get; protected set; }
        public int screenHeight { get; protected set; }
        public CustomMouse CustomMouse { get; set; }
        public SpriteBatch SpriteBatch { get; protected set; }
        public ScreenState ScreenState { get; set; } = ScreenState.Stopped;

        protected Vector2 HeaderPosition 
        { 
            get 
            {
                return new Vector2(ScreenWidth / 10,
                    screenHeight / 20 + camera.Position.Y - CameraInitialPosition.Y);
            } 
        }

        #endregion

        public readonly ScreenTransitionType Type;

        protected SpriteFont commonFont;
        protected WhirlybirdGame game;
        protected Camera2D camera;
        protected Leaderboard leaderboard;
        protected InputState input;
        protected Player player;

        public GameScreen(WhirlybirdGame game, ScreenTransitionType type)
        {
            this.game = game;
            SpriteBatch = game.SpriteBatch;
            leaderboard = game.Leaderboard;
            screenHeight = game.ScreenHeight;
            ScreenWidth = game.ScreenWidth;
            Type = type;
            commonFont = game.CommonFont;
            camera = game.Camera;
            camera.Position = CameraInitialPosition;
            player = game.Player;

            input = new InputState();
        }
        

        public virtual void LoadContent()
        {

        }
        public virtual void Update(GameTime gameTime)
        {
            input.Update();
        }

        public virtual void Draw(GameTime gameTime)
        {
            CustomMouse.Draw(camera, CameraInitialPosition);
        }

        public virtual void HandleInput(InputState input)
        {

        }

        public virtual void Restart() 
        {
            camera.Position = CameraInitialPosition;
        }

        public void DrawRectangle(Rectangle rect, Color col)
        {
            Texture2D texture = new Texture2D(game.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });
            SpriteBatch.Draw(texture, rect, col);
        }

        public void DrawLine(Vector2 start, Vector2 end, int thickness, Color color)
        {
            Texture2D texture = new Texture2D(game.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Player.Preferences.FontColor });
            float distance = Vector2.Distance(start, end);
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

            SpriteBatch.Draw(texture, start, null, color, angle, Vector2.Zero, new Vector2(distance, thickness), SpriteEffects.None, 0);
        }

        protected void DrawHeader(string headerString, int thickness)
        {
            DrawRectangle(new Rectangle(0,
                0,
                ScreenWidth,
                (int)(HeaderPosition.Y + 75)),
                Player.Preferences.MainColor);

            SpriteBatch.DrawString(game.HeaderFont, headerString, HeaderPosition, Player.Preferences.FontColor);

            DrawLine(new Vector2(0, HeaderPosition.Y + 75),
                new Vector2(ScreenWidth, HeaderPosition.Y + 75),
                thickness,
                Player.Preferences.FontColor);
        }
        protected void DrawCoins()
        {
            var headerPosition = new Vector2(ScreenWidth * 87 / 100,
                HeaderPosition.Y);

            SpriteBatch.DrawString(game.NumberFont, game.Player.Coins.ToString(), headerPosition, Player.Preferences.FontColor);
            SpriteBatch.Draw(game.CoinTexture,
                new Rectangle(
                    (int)(headerPosition.X + game.NumberFont.MeasureString(game.Player.Coins.ToString()).X),
                    (int)(headerPosition.Y + 15),
                    60,
                    25),
                Player.Preferences.MainColor);
        }
    }
}
