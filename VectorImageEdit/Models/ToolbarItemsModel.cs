namespace VectorImageEdit.Models
{
    internal enum ColorType { PrimaryColor, SecondaryColor };

    class ToolbarItemsModel
    {
        public ToolbarItemsModel()
        {
            ColorMode = ColorType.PrimaryColor;
        }

        public ColorType ColorMode { get; set; }
    }
}
