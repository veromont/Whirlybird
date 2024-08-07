using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.PlayerSettings;

namespace Whirlybird.Components.Platforms.Moving
{
    enum EnemyState
    {
        Normal,
        Angry,
    }
    internal class Enemy : Platform
    {
        Vector2 Velocity;

        int normalSize = 50;
        int smallSize = 20;
        int size = 0;
        Texture2D[] enemySprites;
        string[] enemySpriteNames;
        bool growing;
        EnemyState state;
        int timer;
        private const int animationDuration = 60;

        public Enemy(WhirlybirdGame game, Vector2 position)
            : base(game, position)
        {
            Velocity = new Vector2(1, 0);
            enemySpriteNames = new string[]
{
                "platforms\\enemy\\enemy-chill",
                "platforms\\enemy\\enemy-angry",
};
            state = EnemyState.Normal;
            Random rnd = new Random();
            timer = 0;
            size = rnd.Next(smallSize, normalSize);
            enemySprites = new Texture2D[enemySpriteNames.Length];
            LoadSpringContent();

            Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, normalSize, normalSize);
        }
        public override void Update(GameTime gameTime)
        {
            if (Rectangle.X >= game.ScreenWidth - Width || Rectangle.X <= 0)
            {
                Velocity *= -1;
            }

            if (timer % 2 == 0)
            {
                size = growing ? size + 1 : size - 1;
                if (size == normalSize || size == smallSize)
                {
                    growing = !growing;
                    state = state == EnemyState.Normal ? EnemyState.Angry : EnemyState.Normal;
                }
            }
            timer++;

            Rectangle = new Rectangle(Rectangle.X + (int)Velocity.X, 
                Rectangle.Y + (int)Velocity.Y, 
                size, 
                size);
        }

        private void LoadSpringContent()
        {
            for (int i = 0; i < enemySpriteNames.Length; i++)
            {
                enemySprites[i] = game.Content.Load<Texture2D>(enemySpriteNames[i]);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            switch (state)
            {
                case EnemyState.Normal:
                    spriteBatch.Draw(enemySprites[0], Rectangle, Player.Preferences.MainColor);
                    break;
                case EnemyState.Angry:
                    spriteBatch.Draw(enemySprites[1], Rectangle, Player.Preferences.MainColor);
                    break;
            }
        }

        public override void Bounce(Bird bird)
        {
            if (state == EnemyState.Angry)
            {
                bird.Velocity = new Vector2(bird.Velocity.X, bird.Acceleration.Y * 1000F);
            }
            else
            {
                base.Bounce(bird);
                Velocity = new Vector2(0, 10);
                for (int i = 0; i < 5; i++)
                {
                    game.Player.AddCoin();
                }
            }
        }
    }
}
