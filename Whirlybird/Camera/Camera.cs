﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlybird.Camera
{
    class Camera
    {
        float _zoom;
        float _rotation;
        Vector2 _position;
        Matrix _transform = Matrix.Identity;
        bool _isViewTransformationDirty = true;
        Matrix _camTranslationMatrix = Matrix.Identity;
        Matrix _camRotationMatrix = Matrix.Identity;
        Matrix _camScaleMatrix = Matrix.Identity;
        Matrix _resTranslationMatrix = Matrix.Identity;

        ResolutionIndependentRenderer ResolutionIndependentRenderer;
        Vector3 _camTranslationVector = Vector3.Zero;
        Vector3 _camScaleVector = Vector3.Zero;
        Vector3 _resTranslationVector = Vector3.Zero;

        public Camera(ResolutionIndependentRenderer resolutionIndependence)
        {
            ResolutionIndependentRenderer = resolutionIndependence;

            _zoom = 0.1f;
            _rotation = 0.0f;
            _position = Vector2.Zero;
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _isViewTransformationDirty = true;
            }
        }

        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < 0.1f)
                {
                    _zoom = 0.1f;
                }
                _isViewTransformationDirty = true;
            }
        }

        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                _isViewTransformationDirty = true;
            }
        }

        public Matrix GetViewTransformationMatrix()
        {
            if (_isViewTransformationDirty)
            {
                _camTranslationVector.X = -_position.X;
                _camTranslationVector.Y = -_position.Y;

                Matrix.CreateTranslation(ref _camTranslationVector, out _camTranslationMatrix);
                Matrix.CreateRotationZ(_rotation, out _camRotationMatrix);

                _camScaleVector.X = _zoom;
                _camScaleVector.Y = _zoom;
                _camScaleVector.Z = 1;

                Matrix.CreateScale(ref _camScaleVector, out _camScaleMatrix);

                _resTranslationVector.X = ResolutionIndependentRenderer.VirtualWidth * 0.5f;
                _resTranslationVector.Y = ResolutionIndependentRenderer.VirtualHeight * 0.5f;
                _resTranslationVector.Z = 0;

                Matrix.CreateTranslation(ref _resTranslationVector, out _resTranslationMatrix);

                _transform = _camTranslationMatrix *
                                _camRotationMatrix *
                                _camScaleMatrix *
                                _resTranslationMatrix *
                                ResolutionIndependentRenderer.GetTransformationMatrix();

                _isViewTransformationDirty = false;
            }

            return _transform;
        }

        public void RecalculateTransformationMatrices()
        {
            _isViewTransformationDirty = true;
        }
    }
}
