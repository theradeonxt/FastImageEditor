using System;
using System.Drawing;
using JetBrains.Annotations;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.Properties;

namespace VectorImageEdit.Modules.BasicShapes
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
        public Picture([NotNull]Bitmap image, Rectangle region, int depthLevel)
            : base(region, depthLevel, "Layer - Picture")
        {
            this.image = image;
        }

        ~Picture()
        {
            Dispose();
        }

        [NotNull]
        private Bitmap image;
        [NotNull]
        public Bitmap Image
        {
            get { return image; }
            set
            {
                Dispose();
                image = value;
                OnPropertyChanged();
            }
        }

        public override void DrawGraphics([NotNull]Graphics destination)
        {
            destination.DrawImage(image, Region);
        }

        public override void Dispose()
        {
            if (image != Resources.placeholder)
            {
                image.Dispose();
            }
        }
    }
}
