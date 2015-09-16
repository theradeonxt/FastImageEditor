using System;
using System.Collections.Generic;
using VectorImageEdit.Modules.Factories;

namespace VectorImageEdit.Models
{
    internal enum ColorType { PrimaryColor, SecondaryColor };

    class ToolstripItemsModel
    {
        private readonly Dictionary<string, IShapeFactory> _shapeActionMap;

        public ToolstripItemsModel()
        {
            ColorMode = ColorType.PrimaryColor;

            _shapeActionMap = new Dictionary<string, IShapeFactory>
            {
                {"circle", new CircleShapeFactory()},
                {"square", new SquareShapeFactory()},
                {"rectangle", new RectangleShapeFactory()},
                {"ellipse", new EllipseShapeFactory()},
                {"star", new StarShapeFactory()},
                {"line", new LineShapeFactory()},
                {"hexagon", new HexagonShapeFactory()},
                {"diamond", new DiamondShapeFactory()},
                {"triangle", new TriangleShapeFactory()}
            };
        }

        public ColorType ColorMode { get; set; }

        public void CreateNewShape(string shapeName)
        {
            try
            {
                var global = AppGlobalData.Instance;
                var size = global.Layout.MaximumSize();
                var style = ShapeStyleBuilder.CreateShapeStyle(global.PrimaryColor,
                    global.SecondaryColor, global.ShapeEdgeSize);
                var shape = _shapeActionMap[shapeName].CreateShape(size, style);
                global.LayerManager.Add(shape);
            }
            catch (NotImplementedException) { }
            catch (KeyNotFoundException) { }
        }
    }
}
