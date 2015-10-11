﻿using System;
using System.Drawing;
using JetBrains.Annotations;
using VectorImageEdit.Properties;

namespace VectorImageEdit.Modules.Layers
{
    /// <summary> 
    /// Picture Module
    ///
    /// - represents an object that draws a bitmap
    ///
    /// </summary>
    [Serializable]
    public class Picture : Layer
    {
        [NotNull]
        private Bitmap _image;

        public Picture([NotNull]Bitmap image, Rectangle region, int depthLevel)
            : base(region, depthLevel, "Layer - Picture")
        {
            _image = image;
        }

        ~Picture()
        {
            Dispose();
        }

        [NotNull]
        public Bitmap Image
        {
            get { return _image; }
            set
            {
                Dispose();
                _image = value;
            }
        }

        #region Interface implementation

        public override void DrawGraphics(Graphics destination)
        {
            destination.DrawImage(_image, Region);
        }
        public override void Dispose()
        {
            if (_image != Resources.placeholder)
            {
                _image.Dispose();
            }
        }

        #endregion
    }
}
