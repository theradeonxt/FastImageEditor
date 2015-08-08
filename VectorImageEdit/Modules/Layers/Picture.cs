using System;
using System.Drawing;
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
        private Bitmap _image;

        public Picture(Bitmap image, Rectangle region, int depthLevel)
            : base(region, depthLevel, "Layer - Picture")
        {
            _image = image ?? Resources.placeholder;
        }

        ~Picture()
        {
            Dispose();
        }

        public Bitmap Image
        {
            get { return _image; }
            set
            {
                Dispose();
                _image = value ?? Resources.placeholder;
            }
        }

        #region Interface implementation

        public override void DrawGraphics(Graphics destination)
        {
            destination.DrawImage(_image, Region);
        }

        public override void Dispose()
        {
            // TODO: Should dispose the bitmap if it's the placeholder?
            if (_image != null && _image != Resources.placeholder)
            {
                _image.Dispose();
            }
        }

        #endregion
    }
}
