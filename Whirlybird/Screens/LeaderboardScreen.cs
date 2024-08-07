using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Whirlybird.Camera;
using Whirlybird.Components.Leaderboard;
using Whirlybird.ScreenManaging;
using Whirlybird.Debugger;
using Whirlybird.Localization;
using Whirlybird.PlayerSettings;

namespace Whirlybird.Screens
{
    public class LeaderboardScreen : GameScreen
    {
        List<LeaderboardRecord> RecordsOrdered
        {
            get
            {
                return leaderboard.Records.OrderByDescending(x => x.Score).ToList();
            }
        }

        SpriteFont numbersFont;
        int page;
        int recordsPerPage;

        public LeaderboardScreen(WhirlybirdGame game)
            : base(game, ScreenTransitionType.Leaderboard)
        {
            page = 0;
            recordsPerPage =(screenHeight - (screenHeight / 8) - screenHeight / 10) / (screenHeight / 15);
            numbersFont = game.NumberFont;

        }

        public override void Update(GameTime gameTime)
        {

            if (input.IsNewKeyPress(Keys.Right) || input.IsNewLeftMouseClick())
            {
                page = Math.Min(page + 1, RecordsOrdered.Count / recordsPerPage);
            }
            else if (input.IsNewKeyPress(Keys.Left) || input.WasRightMouseClicked())
            {
                page = Math.Max(0, page - 1);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float usedScreenHeight = 0;
            DrawHeader(LocalizationManager.GetString("leaderboard"), 3);

            usedScreenHeight += screenHeight * 2 / 10;

            int startRecordIndex = page * recordsPerPage;
            for (int i = startRecordIndex; i < startRecordIndex + recordsPerPage && i < RecordsOrdered.Count; i++)
            {
                var record = RecordsOrdered[i];
                var position = new Vector2(ScreenWidth / 20, usedScreenHeight + (screenHeight / 15) * (i - startRecordIndex));

                SpriteBatch.DrawString(numbersFont, (i + 1).ToString(), position, Player.Preferences.FontColor);

                SpriteBatch.DrawString(commonFont,
                    record.Name,
                    new Vector2(ScreenWidth / 10, position.Y + 5),
                    Player.Preferences.FontColor);

                SpriteBatch.DrawString(numbersFont,
                    record.Score.ToString(),
                    new Vector2(position.X + ScreenWidth / 2, position.Y),
                    Player.Preferences.FontColor);
            }

            base.Draw(gameTime);
        }
    }
}
