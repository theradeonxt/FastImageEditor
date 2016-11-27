using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ImageInterpolation
{
    public enum ImageCorelation
    {
        MinSizeCrop, // crop images according to a calculated minimum of all of them
        MinSizeScale // scale images according to a calculated minimum of all of them
    }

    public enum ItemRole
    {
        Model,
        Presentation
    }

    class ImageContainer : IDisposable
    {
        private readonly Dictionary<string, Bitmap> model; // back-end model data
        private readonly Dictionary<string, Bitmap> presentation; // front-end data

        public ImageContainer()
        {
            model = new Dictionary<string, Bitmap>();
            presentation = new Dictionary<string, Bitmap>();

            ImageCorelation = ImageCorelation.MinSizeCrop;
            ContainerFormat = PixelFormat.Format32bppArgb;
            Output = "DST";
        }

        /// <summary>
        /// Gets or sets the key of the output item
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the corelation imposed on all images.
        /// </summary>
        public ImageCorelation ImageCorelation { get; set; }

        /// <summary>
        /// Gets or sets the pixel format of the images managed by the container
        /// </summary>
        public PixelFormat ContainerFormat { get; set; }

        /// <summary>
        /// Gets or sets the number of input images the container should manage.
        /// </summary>
        public int InputCount { get; set; }

        /// <summary>
        /// Validates the number of input images managed by the container.
        /// </summary>
        public bool InputValid
        {
            get
            {
                int actualCount = model.Count(item => item.Key != "DST");
                return actualCount == InputCount;
            }
        }

        /// <summary>
        /// Gets the container image with the given key using the provided role.
        /// </summary>
        public Bitmap Item(string key, ItemRole role)
        {
            switch (role)
            {
                case ItemRole.Model:
                    return ItemGet(key, model);
                case ItemRole.Presentation:
                    return ItemGet(key, presentation);
            }
            return null;
        }

        /// <summary>
        /// Adds the image with the given key to the container.
        /// </summary>
        public bool AddItem(string key, Bitmap item, Size presentationSize)
        {
            // add model
            if (!AddItem(key, item, model))
                return false;

            // compute appropriate presentation
            var scaledPresentation = BitmapUtility.Resize(Item(key, ItemRole.Model), presentationSize,
                ResizeType.Scaling, ConversionQuality.HighQuality);

            // add presentation
            return AddItem(key, scaledPresentation, presentation);
        }

        /// <summary>
        /// Releases all resources used by the container, effectively clearing its data.
        /// </summary>
        public void Dispose()
        {
            ForeachInput((i, k) => i.Dispose());
        }

        /// <summary>
        /// Adds an output image in the container, both as model and presentation.
        /// The presentation is resized according to ImageCorelation and the provided size.
        /// </summary>
        /// <remarks>
        /// The output is created only if the number of input images equals InputCount.
        /// </remarks>
        /// <param name="size"> Size of presentation </param>
        public void AddOutput(Size size)
        {
            if (InputValid)
            {
                Size modelSize = model.First().Value.Size;
                Bitmap dst = new Bitmap(modelSize.Width, modelSize.Height, ContainerFormat);
                AddItem(Output, dst, size);
            }
        }

        private Bitmap ItemGet(string key, IDictionary<string, Bitmap> container)
        {
            Bitmap result;
            if (!container.TryGetValue(key, out result))
            {
                Debug.WriteLine("[ImageContainer] no item with key: " + key);
            }
            return result;
        }

        private bool AddItem(string key, Bitmap item, IDictionary<string, Bitmap> container)
        {
            if (container.ContainsKey(key))
                return false;

            container.Add(key, item);
            Size min = ComputeMinSize(container);

            var updatedItems = new List<KeyValuePair<string, Bitmap>>();

            ForeachInput(container, (i, k) =>
            {
                // apply configured image corelation
                var resized = BitmapUtility.Resize(i, min, GetRType(),
                    ConversionQuality.HighQuality, ConversionType.Overwrite);

                // ensure configured image format
                var processedItem = BitmapUtility.ConvertToFormat(resized, ContainerFormat,
                    ConversionQuality.HighQuality, ConversionType.Overwrite);

                updatedItems.Add(new KeyValuePair<string, Bitmap>(k, processedItem));
            });

            updatedItems.ForEach(i => container.Remove(i.Key));
            updatedItems.ForEach(i => container.Add(new KeyValuePair<string, Bitmap>(i.Key, i.Value)));

            return true;
        }

        private ResizeType GetRType()
        {
            switch (ImageCorelation)
            {
                case ImageCorelation.MinSizeCrop:
                    return ResizeType.Crop;
                case ImageCorelation.MinSizeScale:
                    return ResizeType.Scaling;
            }
            return ResizeType.Scaling;
        }

        private Size ComputeMinSize(IDictionary<string, Bitmap> container)
        {
            int minWidth = container.First().Value.Width;
            int minHeight = container.First().Value.Height;

            ForeachInput(container, (i, k) =>
            {
                minWidth = i.Width < minWidth ? i.Width : minWidth;
                minHeight = i.Height < minHeight ? i.Height : minHeight;
            });
            return new Size(minWidth, minHeight);
        }

        private void ForeachInput(IDictionary<string, Bitmap> container, Action<Bitmap, string> lambda)
        {
            foreach (var item in container)
                lambda(item.Value, item.Key);
        }

        private void ForeachInput(Action<Bitmap, string> lambda)
        {
            ForeachInput(model, lambda);
            ForeachInput(presentation, lambda);
        }
    }
}
