using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Whirlybird.Components.Input
{
    public class InputState
    {

        public KeyboardState CurrentKeyboardState;
        public KeyboardState LastKeyboardState;
        public MouseState CurrentMouseState;
        public MouseState LastMouseState;

        public InputState()
        {
        }

        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            LastMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            UpdateMouseStates();
        }

        Vector2 prevMousePosition = Vector2.Zero;
        Vector2 currentMousePosition = Vector2.Zero;

        public Vector2 CurrentMousePosition
        {
            get
            {
                return currentMousePosition;
            }
        }

        public Vector2 PrevMousePosition
        {
            get
            {
                return prevMousePosition;
            }
        }

        public Vector2 MouseDelta
        {
            get
            {
                return prevMousePosition - currentMousePosition;
            }
        }

        void UpdateMouseStates()
        {
            currentMousePosition.X = CurrentMouseState.X;
            currentMousePosition.Y = CurrentMouseState.Y;

            prevMousePosition.X = LastMouseState.X;
            prevMousePosition.Y = LastMouseState.Y;

        }


        public bool IsNewKeyPress(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key);
        }

        public bool IsNewLeftMouseClick()
        {
            return CurrentMouseState.LeftButton == ButtonState.Released && LastMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool WasRightMouseClicked()
        {
            return CurrentMouseState.RightButton == ButtonState.Released && LastMouseState.RightButton == ButtonState.Pressed;
        }

        public Keys GetNewPressedKey()
        {
            var result = CurrentKeyboardState.GetPressedKeys().Except(LastKeyboardState.GetPressedKeys());
            return result.FirstOrDefault(Keys.None);
        }

        public int TrackWheelScrolling()
        {
            return CurrentMouseState.ScrollWheelValue - LastMouseState.ScrollWheelValue;
        }

        public void Clear()
        {
            CurrentMouseState = new MouseState();
            LastMouseState = new MouseState();
            CurrentKeyboardState = new KeyboardState();
            LastKeyboardState = new KeyboardState();
        }
    }
}