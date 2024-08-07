using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Whirlybird.PlayerSettings;

namespace Whirlybird.Components.Platforms
{
    enum CrackedPlatformState
    {
        Normal,
        Touched,
        Crumbling,
        Broken,
    }
    internal class CrackedPlatform : Platform
    {
        Texture2D[] springSprites;
        string[] springSpriteNames;
        CrackedPlatformState state;
        int timer;

        private const int ANIMATION_COOLDOWN = 5;

        public CrackedPlatform(WhirlybirdGame game, Vector2 position)
            : base(game, position)
        {
            springSpriteNames = new string[3]
            {
                "platforms\\cracked\\crackedPlatform0",
                "platforms\\cracked\\crackedPlatform1",
                "platforms\\cracked\\crackedPlatform2",
            };
            state = CrackedPlatformState.Normal;
            timer = 0;
            springSprites = new Texture2D[springSpriteNames.Length];
            LoadSpringContent();
        }

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
                case CrackedPlatformState.Normal:
                    spriteBatch.Draw(springSprites[0], Rectangle, Color.White);
                    break;
                case CrackedPlatformState.Touched:
                    spriteBatch.Draw(springSprites[1], Rectangle, Color.White);
                    Animate();
                    break;
                case CrackedPlatformState.Crumbling:
                    spriteBatch.Draw(springSprites[2], Rectangle, Player.Preferences.MainColor);
                    Animate();
                    break;
                case CrackedPlatformState.Broken:
                    break;
            }
        }

        public override void Bounce(Bird bird)
        {
            if (state == CrackedPlatformState.Broken)
            {
                return;
            }
            state = CrackedPlatformState.Touched;
            base.Bounce(bird);
        }

        private void Animate()
        {
            timer++;
            if (timer > ANIMATION_COOLDOWN)
            {
                state++;
                timer = 0;
            }
        }
    }
}
