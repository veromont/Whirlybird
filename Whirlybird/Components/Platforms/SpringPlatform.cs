using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Whirlybird.PlayerSettings;

namespace Whirlybird.Components.Platforms
{
    enum SpringPlatformState
    {
        Normal,
        Triggered,
        Straightened,
    }
    internal class SpringPlatform : Platform
    {
        Texture2D[] springSprites;
        string[] springSpriteNames;
        SpringPlatformState state;
        int timer;

        private const int TIME_TO_SHOOT = 10;

        public SpringPlatform(WhirlybirdGame game, Vector2 position)
            : base(game, position)
        {
            springSpriteNames = new string[]
            {
                "platforms\\spring\\spring0",
                "platforms\\spring\\spring1",
                "platforms\\spring\\spring2",
            };
            state = SpringPlatformState.Normal;
            timer = 0;
            springSprites = new Texture2D[springSpriteNames.Length];
            LoadSpringContent();
        }

        // TODO: fix this horror
        private void LoadSpringContent()
        {
            for (int i = 0; i < springSpriteNames.Length; i++) 
            {
                springSprites[i] = game.Content.Load<Texture2D>(springSpriteNames[i]);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            switch (state)
            {
                case SpringPlatformState.Normal:
                    spriteBatch.Draw(springSprites[0], Rectangle, Player.Preferences.MainColor);
                    break;
                case SpringPlatformState.Triggered:
                    spriteBatch.Draw(springSprites[1], Rectangle, Player.Preferences.MainColor);
                    
                    break;
                case SpringPlatformState.Straightened:
                    spriteBatch.Draw(springSprites[2], Rectangle, Player.Preferences.MainColor);
                    break;
            }
        }

        public override void Bounce(Bird bird)
        {
            state = SpringPlatformState.Triggered;
            bird.Velocity = new Vector2(bird.Velocity.X, bird.Acceleration.Y * -3.6F);
        }

        private void Animate()
        {
            timer++;
            if (timer > TIME_TO_SHOOT)
            {
                state++;
                timer = 0;
            }
        }
    }

}
