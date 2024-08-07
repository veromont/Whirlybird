using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Components.Items;
using Whirlybird.Components.Platforms.Moving;
using Whirlybird.Debugger;
using Whirlybird.PlayerSettings;
using Whirlybird.ScreenManaging;

namespace Whirlybird.Components.Platforms.Ladder
{
    internal class Ladder
    {
        public List<Platform> Platforms { get; private set; } = new List<Platform>();

        Random rnd = new Random();
        int r;

        bool movingPlatformPrev;
        WhirlybirdGame game;
        Texture2D jetpackTexture;
        public Ladder(WhirlybirdGame game)
        {
            this.game = game;
            jetpackTexture = game.Content.Load<Texture2D>("other\\jetpack");
            AddPlatform(100, game.ScreenHeight - 250);
            AddPlatform(450, game.ScreenHeight - 500);
        }

        public void Update()
        {
            if (Player.Preferences.HardMode)
            {
                GenerateHardMode();
            }
            else
            {
                GeneratePlatforms();
            }

            foreach (Platform platform in Platforms)
            {
                platform.Update(new GameTime());
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var platform in Platforms)
            {
                platform.Draw(gameTime);
            }
        }

        public void GeneratePlatforms()
        {
            if (Platforms.Count >= 1000 || Platforms.Count <= 0)
            {
                return;
            }

            var minY = Platforms.Select(p => p.Rectangle.Y).Min();
            var centerX = Platforms.Where(p => p.Rectangle.Y == minY).Select(p => p.Rectangle.X).Max();

            var Y = rnd.Next((int)minY - 300, (int)minY - 100);
            var X = movingPlatformPrev ? 
                rnd.Next(0, game.ScreenWidth - 100) : 
                rnd.Next((int)centerX / 2, Math.Min((int)centerX + 300, game.ScreenWidth - 100));

            r = rnd.Next(0, 100);



            if (r > 90)
            {
                AddSpringPlatform(X, Y);
            }
            else if (r > 75)
            {
                AddCrackedPlatform(X, Y);
            }
            else if (r > 50)
            {
                AddMovingPlatform(X, Y);
            }
            else if (r > 30)
            {
                AddEnemy(X, Y);
            }
            else
            {
                AddPlatform(X, Y);
            }

            r = rnd.Next(0, 100);

            int MaxAttempts = 10000;
            if (r > 70)
            {
                Item itemToAdd = r % 5 == 0
                    ? new Jetpack(jetpackTexture, game, new Vector2())
                    : new Coin(game, new Vector2(), game.CoinTexture);

                bool platformFound = false;
                int attempts = 0;
                var platformExample = Platforms.First();
                while (!platformFound && attempts < MaxAttempts)
                {
                    X = rnd.Next(0, game.ScreenWidth - platformExample.Width);
                    Y = rnd.Next((int)minY - 350, (int)minY - 100);

                    platformFound = Platforms.All(p => !p.Rectangle.Intersects(new Rectangle(X, Y, platformExample.Width, platformExample.Height)));
                    attempts++;
                }

                if (platformFound)
                {
                    AddPlatformWithItem(itemToAdd, X, Y);
                }
            }
        }

        public void GenerateHardMode()
        {
            if (Platforms.Count >= 1000 || Platforms.Count <= 0)
            {
                return;
            }

            var minY = Platforms.Select(p => p.Rectangle.Y).Min();
            var centerX = Platforms.Where(p => p.Rectangle.Y == minY).Select(p => p.Rectangle.X).Max();

            var Y = rnd.Next((int)minY - 300, (int)minY - 100);
            var X = movingPlatformPrev ?
                rnd.Next(0, game.ScreenWidth - 100) :
                rnd.Next((int)centerX / 2, Math.Min((int)centerX + 300, game.ScreenWidth - 100));

            r = rnd.Next(0, 100);

            if (r > 50)
            {
                AddEnemy(X, Y);
            }
            else
            {
                AddMovingPlatform(X, Y);
            }
        }

        private Platform AddPlatform(float x, float y)
        {
            var newPlatform = new Platform(game, new Vector2(x, y));
            Platforms.Add(newPlatform);
            movingPlatformPrev = false;
            return newPlatform;
        }

        private SpringPlatform AddSpringPlatform(float x, float y)
        {
            var newPlatform = new SpringPlatform(game, new Vector2(x, y));
            Platforms.Add(newPlatform);
            movingPlatformPrev = false;

            return newPlatform;
        }

        private CrackedPlatform AddCrackedPlatform(float x, float y)
        {
            var newPlatform = new CrackedPlatform(game, new Vector2(x, y));
            Platforms.Add(newPlatform);
            movingPlatformPrev = false;

            return newPlatform;
        }

        private PlatformWithItem AddPlatformWithItem(Item item, float x, float y)
        {
            var newPlatform = new PlatformWithItem(game, new Vector2(x, y), item);
            Platforms.Add(newPlatform);
            movingPlatformPrev = false;

            return newPlatform;
        }

        private MovingPlatform AddMovingPlatform(float x, float y)
        {
            var newPlatform = new MovingPlatform(game, new Vector2(x, y));
            Platforms.Add(newPlatform);
            movingPlatformPrev = true;

            return newPlatform;
        }

        private Enemy AddEnemy(float x, float y)
        {
            var newPlatform = new Enemy(game, new Vector2(x, y+10));
            Platforms.Add(newPlatform);
            movingPlatformPrev = true;

            return newPlatform;
        }
    }
}
