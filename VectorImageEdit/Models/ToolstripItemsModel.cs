using System.Collections.Generic;
using VectorImageEdit.Modules.Factories;

namespace VectorImageEdit.Models
{
    internal enum ColorType { PrimaryColor, SecondaryColor };

    class ToolstripItemsModel
    {
        private readonly Dictionary<string, ShapeObject> _shapeActionMap;

        public ToolstripItemsModel()
        {
            ColorMode = ColorType.PrimaryColor;

            _shapeActionMap = new Dictionary<string, ShapeObject>
            {
                {"circle", ShapeObject.Circle},
                {"square", ShapeObject.Square},
                {"rectangle", ShapeObject.Rectangle},
                {"ellipse", ShapeObject.Ellipse},
                {"star", ShapeObject.Star},
                {"line", ShapeObject.Line},
                {"hexagon", ShapeObject.Hexagon},
                {"diamond", ShapeObject.Diamond}
            };
        }

        public ColorType ColorMode { get; set; }

        public void CreateNewShape(string shapeName)
        {
            try
            {
                var global = AppGlobalData.Instance;
                var size = global.Layout.MaximumSize();
                var style = ShapeStyleFactory.CreateShapeStyle(global.PrimaryColor,
                    global.SecondaryColor, global.ShapeEdgeSize);
                var shape = ShapeFactory.CreateShape(_shapeActionMap[shapeName], size, style);
                global.LayerManager.Add(shape);
            }
            catch (KeyNotFoundException) { }
        }
    }
}
