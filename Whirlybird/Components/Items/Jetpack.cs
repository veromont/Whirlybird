using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Whirlybird.Components.Platforms;

namespace Whirlybird.Components.Items
{
    internal class Jetpack : Item
    {
        public Jetpack(Texture2D texture, WhirlybirdGame game, Vector2 position)
            : base(texture, position, game, 50, ItemType.Jetpack)
        {

        }
    }
}
