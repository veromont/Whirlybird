using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Whirlybird.Components
{
    internal class Bird : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D birdSprite;
        WhirlybirdGame game;
        float prevCameraLine;

        string birdSpriteName;

        const float TIMETICK = 0.016666667F;
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; private set; }
        public float Mass { get; private set; }

        public Vector2 Position { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        public Bird(WhirlybirdGame game, SpriteBatch Batch, string spriteName)
            : base(game)
        {
            spriteBatch = Batch;
            birdSpriteName = spriteName;
            this.game = game;
            birdSpriteName = spriteName;
            prevCameraLine = 0;

            Velocity = Vector2.Zero;
            Acceleration = new Vector2(0, 500);
            Position = new Vector2(200, 200);
            Mass = 1000;
            Height = 50;
            Width = 50;
        }

        public void ApplyForce(Vector2 direction)
        {
            float a = direction.X / Mass;
            float b = direction.Y / Mass;
            Velocity = new Vector2(Velocity.X + a, Velocity.Y + b);
        }

        public void Bounce()
        {
            Velocity = new Vector2(Velocity.X, Acceleration.Y * -1.2F);
        }

        public override void Initialize()
        {
            birdSprite = game.Content.Load<Texture2D>("bird");
        }

        public override void Update(GameTime gameTime)
        {
            Position += Acceleration * TIMETICK * TIMETICK + Velocity * TIMETICK;
            Velocity += Acceleration * TIMETICK;
            Position = new Vector2(Position.X, Position.Y);
            prevCameraLine = game.cameraLine;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(birdSprite, new Vector2(Position.X, Position.Y), Color.White);
        }
    }
}
