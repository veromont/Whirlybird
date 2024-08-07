using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Whirlybird.Components.Platforms;
namespace Whirlybird.Components.Items
{
    internal class Coin : Item
    {
        public Coin(WhirlybirdGame game, 
            Vector2 position, 
            Texture2D texture)
            : base(texture, position, game, 50, ItemType.Coin)
        {
            this.texture = game.Content.Load<Texture2D>("other\\coin-profile");
        }
    }
}
