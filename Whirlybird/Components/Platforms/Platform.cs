using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Whirlybird.PlayerSettings;
using Whirlybird.Components.Items;

namespace Whirlybird.Components.Platforms
{
    internal class Platform : DrawableGameComponent
    {
        protected WhirlybirdGame game;
        protected SpriteBatch spriteBatch;
        protected Texture2D Sprite;
        string spriteName;

        public Rectangle Rectangle { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }


        const float TIMETICK = 0.016666667F;

        public Platform(WhirlybirdGame game, Vector2 position)
            : base(game)
        {
            spriteBatch = game.SpriteBatch;
            spriteName = "platforms\\basic\\platform";
            this.game = game;

            // DEFAULT VALUES
            Height = 20;
            Width = 60;
            Rectangle = new Rectangle((int)position.X, (int)position.Y, Width, Height);

            LoadContent();
        }

        protected override void LoadContent()
        {
            Sprite = game.Content.Load<Texture2D>(spriteName);
        }

        public bool IsColliding(Bird bird)
        {
            var v = bird.Velocity.Y * TIMETICK;
            var t = v / Math.Abs(Rectangle.Y - bird.Rectangle.Y) > 1;
            return bird.Velocity.Y * TIMETICK / Math.Abs(Rectangle.Y - bird.Rectangle.Y - bird.Rectangle.Height) > 1 &&
            (
                bird.Rectangle.X < Rectangle.X + Width && bird.Rectangle.X > Rectangle.X
                ||
                bird.Rectangle.X + bird.Rectangle.Width < Rectangle.X + Width && bird.Rectangle.X + bird.Rectangle.Width > Rectangle.X
                ||
                bird.Rectangle.X < Rectangle.X && bird.Rectangle.X + bird.Rectangle.Width > Rectangle.X + Width
            );
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(Sprite, Rectangle, Color.White);
            base.Draw(gameTime);
        }

        public virtual void Bounce(Bird bird)
        {
            bird.Velocity = new Vector2(bird.Velocity.X, bird.Acceleration.Y * -1.2F);
            if (Player.Preferences.HardMode)
            {
                game.Player.AddCoin();
            }
        }
    }
}
