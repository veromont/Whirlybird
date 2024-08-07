using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Camera;
using Whirlybird.Components.Input;
using Whirlybird.PlayerSettings;

namespace Whirlybird.Components
{
    public class CustomMouse
    {
        public bool IsFocused { get; set; }
        public int Radius
        {
            get
            {
                return IsFocused ? FOCUS_RADIUS : RADIUS;
            }
        }

        private const int RADIUS = 5;
        private const int FOCUS_RADIUS = 7;

        SpriteBatch spriteBatch;
        InputState input;
        GraphicsDevice graphics;

        public CustomMouse(Game game, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            IsFocused = false;
            this.spriteBatch = spriteBatch;
            this.input = new InputState();
            graphics = graphicsDevice;
        }



        public void Draw(Camera2D camera, Vector2 cameraInitialPosition)
        {
            input.Update();

            if (input.IsNewLeftMouseClick() || input.WasRightMouseClicked())
            {
                IsFocused = false;
            }

            var mousePosition = new Vector2(input.CurrentMousePosition.X + camera.Position.X - cameraInitialPosition.X, 
                input.CurrentMousePosition.Y + camera.Position.Y - cameraInitialPosition.Y);

            var circle = GetCircleTexture(Radius);
            
            var borderCircle = GetCircleTexture(Radius + 1);
            spriteBatch.Draw(borderCircle, mousePosition, Color.DarkGray);

            spriteBatch.Draw(circle, mousePosition, Color.White);
        }

        private Texture2D GetCircleTexture(int radius)
        {
            Texture2D texture = new Texture2D(graphics, radius * 2, radius * 2);
            Color[] data = new Color[texture.Width * texture.Height];
            Vector2 center = new Vector2(radius, radius);

            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    Vector2 position = new Vector2(x, y);
                    if (Vector2.Distance(center, position) <= radius)
                    {
                        data[y * texture.Width + x] = Player.Preferences.CursorColor;
                    }
                    else
                    {
                        data[y * texture.Width + x] = Color.Transparent;
                    }
                }
            }

            texture.SetData(data);
            return texture;
        }
    }
}
