using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Whirlybird.PlayerSettings;
using Whirlybird.ScreenManaging;
using Whirlybird.Screens;

namespace Whirlybird.Components
{
    public class Bird : DrawableGameComponent
    {
        GameScreen screen;

        SpriteBatch spriteBatch;
        BirdSprite birdSprite;
        WhirlybirdGame game;


        const float TIMETICK = 0.016666667F;
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; private set; }

        public Rectangle Rectangle { get; private set; }
        public Vector2 Position
        {
            get
            {
                return new Vector2(Rectangle.X, Rectangle.Y);
            }
            set
            {
                Rectangle = new Rectangle((int)value.X, (int)value.Y, Rectangle.Width, Rectangle.Height);
            }
        }
        public bool IsFacedLeft { get; set; }
        public Player Player { get; set; }
        public BirdSkin Skin
        {
            get
            {
                return birdSprite.Skin;
            }
            set
            {
                skin = value;
                birdSprite.Skin = skin;
                if (Skin == BirdSkin.patriotic)
                {
                    Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, 105, 75);
                }
                else
                {
                    Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, 50, 100);
                }
            }
        }
        private BirdSkin skin;
        public Bird(WhirlybirdGame game,
            SpriteBatch Batch, 
            GameScreen gameplayScreen,
            Vector2 Position)
            : base(game)
        {
            Player = game.Player;
            birdSprite = new BirdSprite(this, Player.CurrentSkin);
            spriteBatch = Batch;
            this.game = game;
            screen = gameplayScreen;

            Velocity = Vector2.Zero;
            Acceleration = new Vector2(0, 500);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0 , 0);

            Skin = Player.CurrentSkin;
            Mass = 1000;

            LoadContent();
        }

        protected override void LoadContent()
        {
            birdSprite.LoadSprite();
        }

        public override void Update(GameTime gameTime)
        {
            Position += Acceleration * TIMETICK * TIMETICK + Velocity * TIMETICK;
            Velocity += Acceleration * TIMETICK;
            Position = new Vector2(Position.X, Position.Y);

            if (Player.CurrentSkin != Skin)
            {
                Skin = Player.CurrentSkin;
                LoadContent();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var whiteBackgroundSkins = new List<BirdSkin>
            {
                BirdSkin.amogus,
                BirdSkin.patriotic,
            };

            var color = whiteBackgroundSkins.Contains(Skin) ? Player.Preferences.MainColor : Color.White;
            var sprite = IsFacedLeft ? birdSprite.FacedLeft : birdSprite.FacedRight;

            //screen.DrawRectangle(Rectangle, Color.White);
            spriteBatch.Draw(sprite, Rectangle, color);
        }

        public void Bounce()
        {
            Velocity = new Vector2(Velocity.X, Acceleration.Y * -1.2F);
        }
    }
}
