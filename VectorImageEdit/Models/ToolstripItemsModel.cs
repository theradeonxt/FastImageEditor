using System;
using System.Collections.Generic;
using VectorImageEdit.Interfaces;
using VectorImageEdit.Modules.Factories;

namespace VectorImageEdit.Models
{
    internal enum ColorType { PrimaryColor, SecondaryColor };

    class ToolstripItemsModel
    {
        /// <summary>
        /// Define the actions at creation of every shape (when triggered from the GUI)
        /// </summary>
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

        /// <summary>
        /// Gets of sets the current active Primary or Secondary Color
        /// </summary>
        public ColorType ColorMode { get; set; }

        #region The Model Working Methods

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

        #endregion
    }
}
