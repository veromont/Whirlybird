using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Whirlybird.Components
{
    public class BirdSprite
    {
        public Texture2D FacedRight { get; set; }
        public Texture2D FacedLeft { get; set; }
        public BirdSkin Skin 
        {
            get
            {
                return skin; 
            }
            set
            {
                if (BirdSkinInfo.SkinNames.Keys.Contains(skin))
                {
                    skin = value;
                    LoadSprite();
                }
            } 
        }
        public string Name 
        {
            get
            {
                return BirdSkinInfo.SkinNames[Skin];
            }
        }


        BirdSkin skin;
        Bird Bird;
        public BirdSprite(Bird bird, BirdSkin skin)
        {
            Bird = bird;
            this.skin = skin;
            FacedLeft = new Texture2D(bird.GraphicsDevice, 1, 1);
            FacedRight = new Texture2D(bird.GraphicsDevice, 1, 1);
        }

        public void LoadSprite()
        {
            var name = BirdSkinInfo.SkinNames[Skin];
            FacedRight = Bird.Game.Content.Load<Texture2D>($"bird\\{name}\\{name}-right");
            FacedLeft = Bird.Game.Content.Load<Texture2D>($"bird\\{name}\\{name}-left");

        }
    }
}
