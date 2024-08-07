using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Camera;
using Whirlybird.Components;
using Whirlybird.Components.Leaderboard;
using Whirlybird.Components.Platforms;
using Whirlybird.Debugger;
using Whirlybird.Localization;
using Whirlybird.PlayerSettings;
using Whirlybird.ScreenManaging;

namespace Whirlybird.Screens
{
    public struct MenuOption
    {
        public string Name { get; set; }
        public Rectangle Rectangle { get; set; }
        public MenuOption(string name, Rectangle rectangle)
        {
            Name = name;
            Rectangle = rectangle;
        }
    }
    public class MenuScreen : GameScreen
    {
        SpriteFont fancyFont;
        List<MenuOption> menuOptions;
        MenuOption selectedOption;

        Bird bird;
        Platform platform;

        public MenuScreen(WhirlybirdGame game)
            : base(game, ScreenTransitionType.Menu)
        {
            fancyFont = game.FancyFont;
            menuOptions = new List<MenuOption>();

            AddMenuOption("start");
            AddMenuOption("shop");
            AddMenuOption("leaderboard");
            AddMenuOption("settings");
            AddMenuOption("exit");
            
            
            var platformPosition = new Vector2((ScreenWidth - menuOptions.First().Rectangle.X) / 2 + 10, 
                menuOptions.Last().Rectangle.Y);
            var birdPosition = new Vector2((ScreenWidth - menuOptions.First().Rectangle.X) / 2,
                menuOptions.Last().Rectangle.Y - 100);

            bird = new Bird(game, SpriteBatch, this, birdPosition);
            
            bird.Acceleration = new Vector2(0, 200);
            platform = new Platform(game, platformPosition);

        }

        public override void Update(GameTime gameTime)
        {
            UpdateBirdAnimation(gameTime);

            if (!selectedOption.Rectangle.Contains(input.CurrentMousePosition))
            {
                CustomMouse.IsFocused = false;
                selectedOption = new MenuOption("", new Rectangle());
                for (int i = 0; i < menuOptions.Count; i++)
                {
                    MenuOption option = menuOptions[i];
                    if (option.Rectangle.Contains(input.CurrentMousePosition))
                    {
                        selectedOption = option;
                        CustomMouse.IsFocused = true;
                    }
                }
            }


            if (input.IsNewLeftMouseClick())
            {
                switch (selectedOption.Name) 
                {
                    case "start":
                        Transition = ScreenTransitionType.Gameplay;
                        ScreenState = ScreenState.Stopped;
                        break;
                    case "shop":
                        Transition = ScreenTransitionType.Shop;
                        ScreenState = ScreenState.Stopped;
                        break;
                    case "leaderboard":
                        Transition = ScreenTransitionType.Leaderboard;
                        ScreenState = ScreenState.Stopped;
                        break;
                    case "settings":
                        Transition = ScreenTransitionType.Settings;
                        ScreenState = ScreenState.Stopped;
                        break;
                    case "exit":
                        game.Player.SaveProgress();
                        game.Exit();
                        break;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.DrawString(fancyFont, "The Whirlybird", new Vector2(ScreenWidth / 10, screenHeight / 8), Player.Preferences.FontColor);

            DrawBirdAnimation(gameTime);

            foreach (var option in menuOptions)
            {
                //showDebugOptions();
                SpriteBatch.DrawString(commonFont, 
                    LocalizationManager.GetString(option.Name), 
                    new Vector2(option.Rectangle.X, option.Rectangle.Y), 
                    option.Name == selectedOption.Name ? Player.Preferences.SecondFontColor : Player.Preferences.FontColor);
            }

            base.Draw(gameTime);
        }

        private void AddMenuOption(string optionName)
        {
            float offset = screenHeight / 2;
            int padding = 20;

            var option = new MenuOption(optionName, 
                new Rectangle(ScreenWidth / 4 * 3, 
                (int)(offset + menuOptions.Count * (commonFont.LineSpacing + padding)), 
                (int)commonFont.MeasureString(LocalizationManager.GetString(optionName)).X, 
                commonFont.LineSpacing));
            menuOptions.Add(option);
        }

        private void showDebugOptions()
        {

            foreach (var option in menuOptions)
            {
                DrawRectangle(option.Rectangle, Color.AliceBlue);
                SpriteBatch.DrawString(commonFont, option.Name, new Vector2(option.Rectangle.X, option.Rectangle.Y), Player.Preferences.FontColor);
            }
        }

        private void UpdateBirdAnimation(GameTime gameTime)
        {
            if (platform.IsColliding(bird))
            {
                platform.Bounce(bird);
            }
            platform.Update(gameTime);
            bird.Update(gameTime);
            platform.Update(gameTime);
        }

        private void DrawBirdAnimation(GameTime gameTime)
        {
            if (platform.IsColliding(bird))
            {
                platform.Bounce(bird);
            }

            bird.Draw(gameTime);
            platform.Draw(gameTime);
        }
    }
}
