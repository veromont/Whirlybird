using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Camera2D = Whirlybird.Camera.Camera;
using Whirlybird.Platforms;
using Whirlybird.Camera;
using Whirlybird.Components;
using Whirlybird.Screens;

namespace Whirlybird
{
    public class WhirlybirdGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch _spriteBatch;
        ResolutionIndependentRenderer _resolutionIndependence;
        Camera2D _camera;

        DeathScreen deathScreen;

        Texture2D birdSprite;
        Texture2D platformSprite;
        SpriteFont constantiaFont;

        public float cameraLine;
        public float deathLine;

        Bird bird;
        List<Platform> platforms;
        Score score;

        MouseState mState;

        const float TIMETICK = 0.016666667F;

        public WhirlybirdGame()
        {
            cameraLine = 100;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            platforms = new List<Platform>();
            _resolutionIndependence = new ResolutionIndependentRenderer(this);

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
        }

        protected override void Initialize()
        {
            _camera = new Camera2D(_resolutionIndependence);
            _camera.Zoom = 0.8f;
            _camera.Position = new Vector2(_resolutionIndependence.VirtualWidth / 2, _resolutionIndependence.VirtualHeight / 2);
            deathLine = 768;

            InitializeResolutionIndependence();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            score = new Score(0, new Vector2(200, 100));
            bird = new Bird(this, _spriteBatch, "bird");

            bird.Initialize();
            deathScreen = new DeathScreen(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            platformSprite = Content.Load<Texture2D>("platform");
            constantiaFont = Content.Load<SpriteFont>("constantia");
            deathScreen.LoadContent();

            platforms.Add(new Platform(new Rectangle(100, 450, 60, 20)));
            platforms.Add(new Platform(new Rectangle(450, 150, 60, 20)));
        }

        protected override void Update(GameTime gameTime)
        {
            if (deathScreen.IsActive)
            {
                deathScreen.Update(gameTime);
                return;
            }

            if (isObjectOutOfScreen(bird.Position))
            {
                deathScreen.ScreenState = ScreenState.Active;
                //Exit();
            }

            var difference = bird.Position.Y + bird.Height - cameraLine;
            if (difference < -0.1)
            {
                UpdatePositions(difference);
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mState = Mouse.GetState();
            bird.Velocity = new Vector2(mState.Position.X - bird.Position.X, bird.Velocity.Y);
            if (bird.Position.Y >= deathLine - bird.Height)
            {
                bird.Bounce();
            }

            var collidingPlatforms = platforms.Where(p => p.IsColliding(bird));
            if (collidingPlatforms.Count() > 0 && bird.Velocity.Y > 0)
            {
                bird.Bounce();
            }

            GeneratePlatforms();

            bird.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Wheat);
            _spriteBatch.Begin(transformMatrix: _camera.GetViewTransformationMatrix());

            if (deathScreen.IsActive)
            {
                deathScreen.Draw(gameTime, _spriteBatch);
            }
            else
            {
                _resolutionIndependence.BeginDraw();

                _spriteBatch.DrawString(constantiaFont, score.Value.ToString(), score.Position, Color.Black);
                DisplayDebug();
                foreach (var platform in platforms)
                {
                    _spriteBatch.Draw(platformSprite, new Vector2(platform.Position.X, platform.Position.Y), Color.White);
                }

                bird.Draw(gameTime);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void InitializeResolutionIndependence()
        {
            _resolutionIndependence.VirtualWidth = 1366;
            _resolutionIndependence.VirtualHeight = 768;
            _resolutionIndependence.ScreenWidth = 1366;
            _resolutionIndependence.ScreenHeight = 768;
            _resolutionIndependence.Initialize();

            _camera.RecalculateTransformationMatrices();
        }

        private float CalculateHeightPosition(float height)
        {
            return height - cameraLine + 100;
        }

        private void UpdatePositions(float difference)
        {
            cameraLine += difference;
            var pl = platforms.Where(p => p.Position.Y - cameraLine > 1000);
            platforms.RemoveAll(p => p.Position.Y - cameraLine > 1000);

            _camera.Position = new Vector2(_resolutionIndependence.VirtualWidth / 2, _resolutionIndependence.VirtualHeight / 2 + cameraLine - 100);
            score.Value = -cameraLine + 100;
            score.Position = new Vector2(score.Position.X, score.Position.Y + difference);
        }

        private bool isObjectOutOfScreen(Vector2 position)
        {
            return position.Y > _resolutionIndependence.VirtualHeight + cameraLine - 100;
        }

        public void Restart()
        {
            cameraLine = 100;
            _camera.Position = new Vector2(_resolutionIndependence.VirtualWidth / 2, _resolutionIndependence.VirtualHeight / 2);
            platforms.Clear();
            Initialize();
        }

        public void GeneratePlatforms()
        {
            if (platforms.Count >= 1000 || platforms.Count <= 0)
            {
                return;
            }

            Random rnd = new Random();

            var minY = platforms.Select(p => p.Position.Y).Min();
            var centerX = platforms.Where(p => p.Position.Y == minY).Select(p => p.Position.X).Max();

            var Y = rnd.Next((int)minY - 300, (int)minY - 100);
            var X = rnd.Next((int)centerX / 2, (1366 - (int)centerX) / 2);

            platforms.Add(new Platform(new Rectangle(X, Y, 60, 20)));
        }

        private void DisplayDebug()
        {
            _spriteBatch.DrawString(constantiaFont, "bird: " + bird.Position.ToString(), new Vector2(60, 120), Color.Black);
            _spriteBatch.DrawString(constantiaFont, "mouse: " + mState.Position.ToString(), new Vector2(90, 500), Color.Black);
            //_spriteBatch.DrawString(constantiaFont, platforms.Count.ToString(),
            //    new Vector2(score.Position.X, score.Position.Y + 50), 
            //    Color.Black);
            _spriteBatch.DrawString(constantiaFont, platforms.Select(p => p.Position.Y).Min().ToString(),
                new Vector2(score.Position.X, score.Position.Y + 100),
                Color.Black);
        }
    }
}
