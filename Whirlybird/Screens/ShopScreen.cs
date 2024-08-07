using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Whirlybird.Camera;
using Whirlybird.Components;
using Whirlybird.Components.Leaderboard;
using Whirlybird.Debugger;
using Whirlybird.Localization;
using Whirlybird.PlayerSettings;
using Whirlybird.ScreenManaging;

namespace Whirlybird.Screens
{
    public struct ShopItem
    {
        public Texture2D Texture { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsOwned { get; set; }
        public Rectangle Rectangle { get; set; }
        public ShopItem(Texture2D texture, string name, int price, bool isOwned, Rectangle rectangle)
        {
            Texture = texture;
            Name = name;
            Price = price;
            IsOwned = isOwned;
            Rectangle = rectangle;
        }
        public ShopItem()
        {
            Name = "";
            Rectangle = new Rectangle();
            IsOwned = false;
            Price = 0;
            Texture = null;
        }
    }
    public class ShopScreen : GameScreen
    {
        List<ShopItem> shopItems;
        Texture2D crateTexture;
        ShopItem focusedItem;

        bool processingClick = false;
        int itemSize = 350;
        int spacing = 30;

        public ShopScreen(WhirlybirdGame game)
            : base(game, ScreenTransitionType.Shop)
        {
            shopItems = new List<ShopItem>();
            focusedItem = new ShopItem();
        }

        public override void LoadContent()
        {
            crateTexture = game.Content.Load<Texture2D>("other\\crate");
            InitializeItems();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            HandleScrolling();

            var realMousePosition = new Vector2(input.CurrentMousePosition.X, 
                input.CurrentMousePosition.Y + camera.Position.Y - CameraInitialPosition.Y);

            foreach (var item in shopItems)
            {
                if (item.Rectangle.Contains(realMousePosition))
                {
                    CustomMouse.IsFocused = true;
                    focusedItem = item;
                }
            }
            if (!focusedItem.Rectangle.Contains(realMousePosition))
            {
                CustomMouse.IsFocused = false;
                focusedItem = new ShopItem();
            }

            if (input.IsNewLeftMouseClick() && !processingClick)
            {
                processingClick = true;
                var focusedSkin = BirdSkinInfo.SkinNames.Where(s => s.Value == focusedItem.Name).First().Key;
                if (player.OwnedSkins.Contains(focusedSkin))
                {
                    player.CurrentSkin = focusedSkin;
                    ScreenState = ScreenState.Stopped;
                    Transition = ScreenTransitionType.Menu;
                }
                else if (player.Coins >= focusedItem.Price)
                {
                    player.SpendMoney(focusedItem.Price);
                    player.OwnedSkins.Add(focusedSkin);
                    focusedItem.IsOwned = true;
                    player.SaveProgress();
                    Restart();
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var item in shopItems)
            {
                DrawItemFraming(item);
                DrawItemTexture(item);
                DrawItemInfo(item);
            }

            DrawHeader(LocalizationManager.GetString("shop"), 3);
            DrawCoins();
            base.Draw(gameTime);
        }


        public override void Restart()
        {
            focusedItem = new ShopItem();
            shopItems.Clear();
            InitializeItems();
            processingClick = false;
            base.Restart();
        }

        #region Private methods

        private void HandleScrolling()
        {
            int offset = 20;
            int lowestItemPosition = screenHeight;
            if (shopItems.Count > 0)
            {
                lowestItemPosition = shopItems.Select(i => i.Rectangle.Bottom).Min() + offset;
            }

            var scrollValue = input.TrackWheelScrolling();
            if (scrollValue > 0 &&
                camera.Position.Y - screenHeight / 20 >= CameraInitialPosition.Y)
            {
                camera.Move(new Vector2(0, -screenHeight / 20));
            }
            else if (scrollValue < 0 &&
                camera.Position.Y + screenHeight / 20 <= lowestItemPosition)
            {
                camera.Move(new Vector2(0, screenHeight / 20));
            }
        }

        private void InitializeItems()
        {
            int marginTop = 100;
            int marginLeft = 50;


            int itemsPerRow = (ScreenWidth - marginLeft) / (itemSize + spacing);
            for (int i = 0; i < BirdSkinInfo.SkinNames.Count; i++)
            {
                int row = i / itemsPerRow;
                int column = i % itemsPerRow;

                int x = spacing + column * (itemSize + spacing) + marginLeft;
                int y = spacing + row * (itemSize + spacing) + marginTop;

                var skin = (BirdSkin)i;
                var name = BirdSkinInfo.SkinNames[skin];
                var texture = game.Content.Load<Texture2D>($"bird\\{name}\\{name}-left");
                var itemPrice = BirdSkinInfo.SkinPrices[skin];
                var isItemOwned = game.Player.OwnedSkins.Contains(skin);
                var itemRectangle = new Rectangle(x, y, itemSize, itemSize);

                shopItems.Add(new ShopItem(texture, name, itemPrice, isItemOwned, itemRectangle));
            }
        }

        private void DrawItemFraming(ShopItem item)
        {
            DrawRectangle(item.Rectangle, item.IsOwned ? Color.Gray : Color.Red);

            int itemBoxPadding = focusedItem.Name == item.Name ? 8 : 4;

            DrawRectangle(
                new Rectangle(item.Rectangle.X + itemBoxPadding,
                item.Rectangle.Y + itemBoxPadding,
                itemSize - itemBoxPadding * 2,
                itemSize - itemBoxPadding * 2),
                Player.Preferences.MainColor);
        }

        private void DrawItemTexture(ShopItem item)
        {
            int itemTextureWidth = 100;
            int itemTextureheight = 200;

            Rectangle textureRect = new Rectangle(item.Rectangle.X + (itemSize - itemTextureWidth) / 2,
                item.Rectangle.Y + 20,
                itemTextureWidth,
                itemTextureheight);

            if (item.Name == "patriotic" && item.IsOwned)
            {
                textureRect.X -= 80;
                textureRect.Height = 200;
                textureRect.Width = 280;
            }

            if (item.IsOwned)
            {
                SpriteBatch.Draw(item.Texture, textureRect, Color.White);
            }
            else
            {
                textureRect = new Rectangle(item.Rectangle.X + (itemSize - 210) / 2, item.Rectangle.Y + 20, 210, 200);
                SpriteBatch.Draw(crateTexture, textureRect, Player.Preferences.MainColor);
            }
        }

        private void DrawItemInfo(ShopItem item)
        {
            Vector2 namePosition = new Vector2(item.Rectangle.X + 10, item.Rectangle.Y + itemSize - 120);
            SpriteBatch.DrawString(commonFont, LocalizationManager.GetString(item.Name).ToUpper(), namePosition, Player.Preferences.FontColor);

            Vector2 pricePosition = new Vector2(item.Rectangle.X + 10, item.Rectangle.Y + itemSize - 80);
            SpriteBatch.DrawString(commonFont, $"{LocalizationManager.GetString("price")}: {item.Price}", pricePosition, Player.Preferences.FontColor);

            string ownershipStatus = item.IsOwned ? LocalizationManager.GetString("owned") : LocalizationManager.GetString("not-owned");
            Vector2 ownershipPosition = new Vector2(item.Rectangle.X + 10, item.Rectangle.Y + itemSize - 40);
            SpriteBatch.DrawString(commonFont, ownershipStatus, ownershipPosition, Player.Preferences.FontColor);
        }

        #endregion
    }
}
