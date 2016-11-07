using System;
using System.Collections.Generic;
using VectorImageEdit.Modules.Factories;

namespace VectorImageEdit.Models
{
    internal enum ColorType { PrimaryColor, SecondaryColor };

    class ToolstripItemsModel
    {
        /// <summary>
        /// Define the actions at creation of every shape (when triggered from the GUI)
        /// </summary>
        private readonly Dictionary<string, DefaultShapeFactory.ShapeCreator> shapeActionMap;

        public ToolstripItemsModel()
        {
            ColorMode = ColorType.PrimaryColor;

            shapeActionMap = new Dictionary<string, DefaultShapeFactory.ShapeCreator>
            {
                {"circle", DefaultShapeFactory.CreateCircle},
                {"square",  DefaultShapeFactory.CreateSquare},
                {"rectangle", DefaultShapeFactory.CreateRectangle},
                {"ellipse", DefaultShapeFactory.CreateEllipse},
                {"star", DefaultShapeFactory.CreateStar},
                {"line", DefaultShapeFactory.CreateLine},
                {"hexagon", DefaultShapeFactory.CreateHexagon},
                {"diamond", DefaultShapeFactory.CreateDiamond},
                {"triangle", DefaultShapeFactory.CreateTriangle}
            };
        }

        /// <summary>
        /// Gets of sets the current active Primary or Secondary Color
        /// </summary>
        public ColorType ColorMode { get; set; }

        public void CreateNewShape(string shapeName)
        {
            try
            {
                var global = AppModel.Instance;
                var size = global.Layout.MaximumSize();

                var style = ShapeStyleBuilder.CreateShapeStyle(global.PrimaryColor,
                    global.SecondaryColor, global.ShapeEdgeSize);

                var shape = shapeActionMap[shapeName](size, style);
                global.LayerManager.Add(shape);
            }
            catch (NotImplementedException) { }
            catch (KeyNotFoundException) { }
        }
    }
}
