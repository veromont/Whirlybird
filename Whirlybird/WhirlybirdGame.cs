using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Resources;
using Whirlybird.Camera;
using Whirlybird.Components;
using Whirlybird.Components.Leaderboard;
using Whirlybird.Localization;
using Whirlybird.PlayerSettings;
using Whirlybird.ScreenManaging;
using Whirlybird.Screens;

namespace Whirlybird
{
    public class WhirlybirdGame : Game
    {
        public SpriteBatch SpriteBatch { get; private set; }
        public Camera2D Camera { get; private set; }
        public CustomMouse CustomMouse { get; set; }
        public Leaderboard Leaderboard { get; private set; }
        public Player Player { get; private set; }

        public SpriteFont CommonFont { get; private set; }
        public SpriteFont HeaderFont { get; private set; }
        public SpriteFont NumberFont { get; private set; }
        public SpriteFont FancyFont { get; private set; }
        public Texture2D CoinTexture { get; private set; }

        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }

        GraphicsDeviceManager graphics;
        ResolutionIndependentRenderer _resolutionIndependence;
        ScreenManager screenManager;

        // screens
        DeathScreen deathScreen;
        GameplayScreen gameplayScreen;
        LeaderboardScreen leaderBoardScreen;
        MenuScreen menuScreen;
        ShopScreen shopScreen;
        SettingsScreen settingsScreen;

        public WhirlybirdGame()
        {
            Player = new Player();

            graphics = new GraphicsDeviceManager(this);
            _resolutionIndependence = new ResolutionIndependentRenderer(this);
            Camera = new Camera2D(_resolutionIndependence);
            Camera.Zoom = 1f;
            Content.RootDirectory = "Content";
            Player = new Player();

            var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            ScreenWidth = displayMode.Width;
            ScreenHeight = displayMode.Height;


            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            
            graphics.IsFullScreen = true;
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            ResourceManager englishResources = new ResourceManager("Whirlybird.Localization.Strings", typeof(WhirlybirdGame).Assembly);
            ResourceManager ukrainianResources = new ResourceManager("Whirlybird.Localization.StringsUa", typeof(WhirlybirdGame).Assembly);
            ResourceManager spanishResources = new ResourceManager("Whirlybird.Localization.StringsEs", typeof(WhirlybirdGame).Assembly);

            LocalizationManager.AddResourceManager("en", englishResources);
            LocalizationManager.AddResourceManager("ua", ukrainianResources);
            LocalizationManager.AddResourceManager("es", spanishResources);


            LocalizationManager.SetCulture(Player.Preferences.LanguageCode);

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Leaderboard = new Leaderboard("Leaderboard.bin");
            CustomMouse = new CustomMouse(this, SpriteBatch, GraphicsDevice);
            Leaderboard.LoadData();

            InitializeResolutionIndependence();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            CommonFont = Content.Load<SpriteFont>("fonts\\constantia");
            NumberFont = Content.Load<SpriteFont>("fonts\\numbers");
            FancyFont = Content.Load<SpriteFont>("fonts\\wonderland");
            HeaderFont = Content.Load<SpriteFont>("fonts\\alice-header");

            CoinTexture = Content.Load<Texture2D>("other\\coin");
            InitializeScreens();
        }

        protected override void Update(GameTime gameTime)
        {
            screenManager.Update(gameTime);
            LocalizationManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Player.Preferences.MainColor);
            SpriteBatch.Begin(transformMatrix: Camera.GetViewTransformationMatrix());
            _resolutionIndependence.BeginDraw();

            screenManager.Draw(gameTime);

            SpriteBatch.End();
            base.Draw(gameTime);
        }



        #region Private methods
        private void InitializeResolutionIndependence()
        {
            _resolutionIndependence.VirtualWidth = ScreenWidth;
            _resolutionIndependence.VirtualHeight = ScreenHeight;
            _resolutionIndependence.ScreenWidth = ScreenWidth;
            _resolutionIndependence.ScreenHeight = ScreenHeight;
            _resolutionIndependence.Initialize();

            Camera.RecalculateTransformationMatrices();
        }

        private void InitializeScreens()
        {
            var score = new Score(0, new Vector2());
            deathScreen = new DeathScreen(this, score);
            gameplayScreen = new GameplayScreen(this, score);
            leaderBoardScreen = new LeaderboardScreen(this);
            menuScreen = new MenuScreen(this);
            shopScreen = new ShopScreen(this);
            settingsScreen = new SettingsScreen(this);

            var screens = new List<GameScreen>
            {
                deathScreen,
                gameplayScreen,
                leaderBoardScreen,
                menuScreen,
                shopScreen,
                settingsScreen,
            };
            menuScreen.ScreenState = ScreenState.Focus;

            screenManager = new ScreenManager(this, screens);
            screenManager.Initialize();
        }
        #endregion
    }
}
