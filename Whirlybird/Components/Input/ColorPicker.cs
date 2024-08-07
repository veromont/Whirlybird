using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Debugger;
using Whirlybird.PlayerSettings;
using Whirlybird.ScreenManaging;
using static System.Net.Mime.MediaTypeNames;

namespace Whirlybird.Components.Input
{
    public class ColorPicker : DrawableGameComponent
    {
        public Color CurrentColor { get; set; }
        public Rectangle ColorRect { get; set; }
        public readonly string AssociatedString;
        SpriteBatch spriteBatch;
        Texture2D colorTexture;
        List<Color> standardColors;
        Rectangle popupRect;
        SpriteFont font;
        Rectangle[] standardColorRects;
        InputState input;
        bool showPopup;

        public ColorPicker(WhirlybirdGame game, 
            Rectangle Position, 
            Color initialColor, 
            InputState input, 
            string assocString) : base(game)
        {
            int popupWidth = 300;
            int popupHeight = 550;
            ColorRect = Position;
            font = game.CommonFont;
            popupRect = new Rectangle((game.ScreenWidth - popupWidth) / 2, game.ScreenHeight / 5, popupWidth, popupHeight);
            this.input = input;
            AssociatedString = assocString;
            standardColors = new List<Color>();
            var properties = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static);
            bool skipFirst = false;
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetGetMethod() != null && propertyInfo.PropertyType == typeof(Color))
                {
                    if (!skipFirst)
                    {
                        skipFirst = true;
                        continue;
                    }
                    Color color = (Color)propertyInfo.GetValue(null, null); 
                    standardColors.Add(color);
                }
            }

            CurrentColor = initialColor;
            showPopup = false;

            LoadContent();
        }

        protected override void LoadContent()
        {
            int offset = 10;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            colorTexture = new Texture2D(GraphicsDevice, 1, 1);
            colorTexture.SetData(new[] { Color.White });

            standardColorRects = new Rectangle[standardColors.Count];
            int squaresInRow = popupRect.Width / 35;
            for (int i = 0; i < standardColors.Count / squaresInRow; i++)
            {
                for (int j = 0; j < squaresInRow && i * 8 + j < standardColors.Count;  j++)
                {
                    standardColorRects[i * 8 + j] = new Rectangle(popupRect.X + offset + 30 * j, popupRect.Y + offset + i * 30, 20, 20);
                }
            }

            popupRect.Width = squaresInRow * 30 + offset;
            popupRect.Height = standardColorRects.Length / squaresInRow * 30 + offset;
        }
        int i = 0;
        public override void Update(GameTime gameTime)
        {
            i++;
            Logger.Log(i.ToString());
            if (showPopup)
            {
                HandlePopupInput();
            }
            else
            {
                HandleMainInput();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int border = 1;
            spriteBatch.Begin();
            var tempRectangle = new Rectangle(ColorRect.X - border,
                ColorRect.Y - border,
                ColorRect.Width + 2 * border,
                ColorRect.Height + 2 * border);
            spriteBatch.Draw(colorTexture, tempRectangle, Color.Black);
            spriteBatch.Draw(colorTexture, ColorRect, CurrentColor);

            if (showPopup)
            {
                spriteBatch.Draw(colorTexture, popupRect, Color.Gray);

                for (int i = 0; i < standardColors.Count; i++)
                {
                    tempRectangle = new Rectangle (standardColorRects[i].X - border, 
                        standardColorRects[i].Y - border, 
                        standardColorRects[i].Width + 2 * border, 
                        standardColorRects[i].Height + 2 * border);
                    spriteBatch.Draw(colorTexture, tempRectangle, Color.Black);
                    spriteBatch.Draw(colorTexture, standardColorRects[i], standardColors[i]);
                }
            }
            spriteBatch.End();
        }

        private void HandleMainInput()
        {
            if (ColorRect.Contains(input.CurrentMousePosition))
            {
                Logger.Log(CurrentColor.ToString());
            }
            if (input.IsNewLeftMouseClick() && ColorRect.Contains(input.CurrentMousePosition))
            {
                showPopup = true;
            }
        }

        private void HandlePopupInput()
        {
            if (input.IsNewLeftMouseClick())
            {
                for (int i = 0; i < standardColors.Count; i++)
                {
                    if (standardColorRects[i].Contains(input.CurrentMousePosition))
                    {
                        CurrentColor = standardColors[i];
                        showPopup = false;
                        break;
                    }
                }
            }

            if (input.IsNewKeyPress(Keys.Down))
            {
                showPopup = false;
            }
        }
    }

}
