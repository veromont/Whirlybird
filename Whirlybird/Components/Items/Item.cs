using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Whirlybird.Components.Platforms;
using Whirlybird.Debugger;
using Whirlybird.PlayerSettings;

namespace Whirlybird.Components.Items
{
    internal class Item
    {
        public Vector2 Position { get; set; }
        public int Size { get; set; }
        public readonly ItemType Type;
        
        protected Texture2D texture;
        protected SpriteBatch spriteBatch;
        protected WhirlybirdGame game;

        public Item(Texture2D texture, 
            Vector2 position, 
            WhirlybirdGame game, 
            int size,
            ItemType type)
        {
            this.texture = texture;
            Position = position;
            spriteBatch = game.SpriteBatch;
            this.game = game;
            Size = size;
            Type = type;
        }

        public virtual void LoadContent()
        {
            
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            spriteBatch.Draw(texture, 
                new Rectangle((int)Position.X, (int)Position.Y, Size, Size), 
                Player.Preferences.MainColor);
        }
    }
}
