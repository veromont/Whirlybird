using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Whirlybird.ScreenManaging;
using Microsoft.Xna.Framework;
using System.Text;
using System;
using Whirlybird.Debugger;
using Whirlybird.PlayerSettings;

namespace Whirlybird.Components.Input
{
    public class TextInputHandler
    {
        public bool IsInputFinished { get; private set; }
        public string Input => inputString.ToString();
        public int Cursor { get; private set; }
        public string Message { get; set; }

        private StringBuilder inputString;
        private InputState input;
        private GameScreen screen;
        private Rectangle CursorRectangle;
        public TextInputHandler(InputState state, string message, GameScreen screen)
        {
            inputString = new StringBuilder(64);
            input = state;
            IsInputFinished = false;
            Cursor = 0;
            Message = message;
            this.screen = screen;
            CursorRectangle = new Rectangle(0, 0, 3, 30);
        }


        public void Update()
        {
            if (IsInputFinished)
            {
                return;
            }

            if (input.IsNewKeyPress(Keys.Enter))
            {
                IsInputFinished = true;
            }
            else if (input.IsNewKeyPress(Keys.Left) && Cursor > 0)
            {

                Cursor--;
            }
            else if (input.IsNewKeyPress(Keys.Right) && Cursor != Input.Length)
            {

                Cursor++;
            }
            else if (input.IsNewKeyPress(Keys.Back) && Cursor > 0)
            {

                inputString.Remove(Cursor - 1, 1);
                Cursor--;
            }
            var newKey = input.GetNewPressedKey();
            var c = ConvertKeyToChar(newKey);
            if (c != '\0')
            {
                if (Cursor == Input.Length)
                {
                    inputString.Append(ConvertKeyToChar(newKey));
                }
                else
                {
                    inputString.Insert(Cursor, c);
                }

                Cursor++;
            }
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, int X, int Y)
        {
            CursorRectangle.X = X + (int)font.MeasureString(Message + Input.Substring(0, Cursor)).X;
            CursorRectangle.Y = Y;
            screen.DrawRectangle(CursorRectangle, Player.Preferences.SecondFontColor);
            spriteBatch.DrawString(font, Message + Input, new Vector2(X, Y), Player.Preferences.FontColor);

        }

        private char ConvertKeyToChar(Keys key)
        {
            if (key >= Keys.A && key <= Keys.Z)
            {
                return (char)(key - Keys.A + 'A');
            }
            if (key >= Keys.D0 && key <= Keys.D9)
            {
                return (char)(key - Keys.D0 + '0');
            }

            if (key == Keys.Space)
            {
                return ' ';
            }
            return '\0';
        }

    }
}
