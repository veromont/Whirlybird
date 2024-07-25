using Microsoft.Xna.Framework;

namespace Whirlybird.Components
{
    internal class Score
    {
        public float Value { get; set; }
        public Vector2 Position { get; set; }

        public Score(float value, Vector2 position)
        {
            Value = value;
            Position = position;
        }
    }
}
