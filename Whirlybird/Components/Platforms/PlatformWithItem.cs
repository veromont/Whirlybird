using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Components.Items;

namespace Whirlybird.Components.Platforms
{
    enum ItemType
    {
        Coin,
        Jetpack,
    }
    internal class PlatformWithItem : Platform
    {
        Item item;
        bool itemUsed = false;
        public PlatformWithItem(WhirlybirdGame game, Vector2 position, Item item)
            : base(game, position)
        {
            this.item = item;
            item.Position = new Vector2(position.X + Width / 2 - item.Size / 2, 
                position.Y - item.Size);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!itemUsed)
            {
                item.Draw();
            }
            base.Draw(gameTime);
        }

        public override void Bounce(Bird bird)
        {
            if (!itemUsed)
            {
                switch (item.Type)
                {
                    case ItemType.Coin:
                        game.Player.AddCoin();
                        break;
                    case ItemType.Jetpack:
                        bird.Velocity = new Vector2(bird.Velocity.X, bird.Acceleration.Y * -5F);
                        return;
                }
            }
            itemUsed = true;
            base.Bounce(bird);
        }
    }
}
