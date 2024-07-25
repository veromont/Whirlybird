using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Components;

namespace Whirlybird.Platforms
{
    internal class Platform
    {
        const float TIMETICK = 0.016666667F;

        public Vector2 Position { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public Platform(Rectangle platformRectangle)
        {
            Position = new Vector2(platformRectangle.X, platformRectangle.Y);
            Height = platformRectangle.Height;
            Width = platformRectangle.Width;
        }

        public bool IsColliding(Bird bird)
        {
            var v = bird.Velocity.Y * TIMETICK;
            var t = (v / Math.Abs(Position.Y - bird.Position.Y)) > 1;
            return (bird.Velocity.Y * TIMETICK / Math.Abs(Position.Y - bird.Position.Y - bird.Height)) > 1 &&
            (
                bird.Position.X < Position.X + Width && bird.Position.X > Position.X
                ||
                bird.Position.X + bird.Width < Position.X + Width && bird.Position.X + bird.Width > Position.X
            );
        }
    }
}
