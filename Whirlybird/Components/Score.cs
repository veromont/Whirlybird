using Microsoft.Xna.Framework;
using System;

namespace Whirlybird.Components
{
    public class Score
    {
        public float Value 
        { get
            {
                return scoreValue;
            }
            set
            {
                if (!IsLocked)
                {
                    scoreValue = value;
                }
            }
        }
        public Vector2 Position { get; set; }
        public bool IsLocked { get; set;}

        private float scoreValue;
        public Score(float value, Vector2 position)
        {
            scoreValue = 0;
            IsLocked = false;
            Value = value;
            Position = position;
        }

        public override string ToString()
        {
            return Math.Round((double)Value, 2).ToString();
        }
    }
}
