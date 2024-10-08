﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Whirlybird.PlayerSettings;

namespace Whirlybird.Camera
{
    public class ResolutionIndependentRenderer
    {
        readonly Game _game;
        Viewport _viewport;
        float _ratioX;
        float _ratioY;
        Vector2 _virtualMousePosition = new Vector2();

        public Color BackgroundColor { get; set; }

        public ResolutionIndependentRenderer(Game game)
        {
            _game = game;
            VirtualWidth = 1366;
            VirtualHeight = 768;

            ScreenWidth = 1024;
            ScreenHeight = 768;
            BackgroundColor = Player.Preferences.MainColor;
        }

        public int VirtualHeight { get; set; }

        public int VirtualWidth { get; set; }

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        public void Initialize()
        {
            SetupVirtualScreenViewport();

            _ratioX = (float)_viewport.Width / VirtualWidth;
            _ratioY = (float)_viewport.Height / VirtualHeight;

            _dirtyMatrix = true;
        }

        public void SetupFullViewport()
        {
            var vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = ScreenWidth;
            vp.Height = ScreenHeight;
            _game.GraphicsDevice.Viewport = vp;
            _dirtyMatrix = true;
        }

        public void BeginDraw()
        {
            // Start by reseting viewport to (0,0,1,1)
            SetupFullViewport();
            // Clear to Black
            _game.GraphicsDevice.Clear(BackgroundColor);
            // Calculate Proper Viewport according to Aspect Ratio
            SetupVirtualScreenViewport();
            // and clear that
            // This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
        }

        public bool RenderingToScreenIsFinished;
        private static Matrix _scaleMatrix;
        private bool _dirtyMatrix = true;

        public Matrix GetTransformationMatrix()
        {
            if (_dirtyMatrix)
                RecreateScaleMatrix();

            return _scaleMatrix;
        }

        private void RecreateScaleMatrix()
        {
            Matrix.CreateScale((float)ScreenWidth / VirtualWidth, (float)ScreenWidth / VirtualWidth, 1f, out _scaleMatrix);
            _dirtyMatrix = false;
        }

        public Vector2 ScaleMouseToScreenCoordinates(Vector2 screenPosition)
        {
            var realX = screenPosition.X - _viewport.X;
            var realY = screenPosition.Y - _viewport.Y;

            _virtualMousePosition.X = realX / _ratioX;
            _virtualMousePosition.Y = realY / _ratioY;

            return _virtualMousePosition;
        }

        public void SetupVirtualScreenViewport()
        {
            var targetAspectRatio = VirtualWidth / (float)VirtualHeight;
            // figure out the largest area that fits in this resolution at the desired aspect ratio
            var width = ScreenWidth;
            var height = (int)(width / targetAspectRatio + .5f);

            if (height > ScreenHeight)
            {
                height = ScreenHeight;
                // PillarBox
                width = (int)(height * targetAspectRatio + .5f);
            }

            // set up the new viewport centered in the backbuffer
            _viewport = new Viewport
            {
                X = (ScreenWidth / 2) - (width / 2),
                Y = (ScreenHeight / 2) - (height / 2),
                Width = width,
                Height = height
            };

            _game.GraphicsDevice.Viewport = _viewport;
        }
    }
}
