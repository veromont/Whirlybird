
using Microsoft.Xna.Framework;

namespace Whirlybird.Components.Platforms.Moving
{
    internal class MovingPlatform : Platform
    {
        int Velocity;
        
        public MovingPlatform(WhirlybirdGame game, Vector2 position)
            : base(game, position)
        {
            Velocity = 1;
        }
        public override void Update(GameTime gameTime)
        {
            if (Rectangle.X >= game.ScreenWidth - Width || Rectangle.X <= 0)
            {
                Velocity *= -1;
            }
            Rectangle = new Rectangle(Rectangle.X + Velocity, 
                Rectangle.Y, 
                Rectangle.Width, 
                Rectangle.Height);
            base.Update(gameTime);
        }
    }
}
