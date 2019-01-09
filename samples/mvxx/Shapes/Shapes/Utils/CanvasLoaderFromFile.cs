using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Utils
{
    public class CanvasLoaderFromFile : ICanvasLoader
    {
        private readonly string _fileName;

        public CanvasLoaderFromFile(string fileName)
        {
            _fileName = fileName;
        }

        public void LoadShapes(CanvasBuilder canvasBuilder)
        {
            foreach (string line in System.IO.File.ReadAllLines(_fileName))
            {
                var parts = line.Split(' ');
                if (parts.Length != 5)
                {
                    throw new Exception();
                }

                Common.ShapeType shapeType = ShapeTypeUtils.StringToType(parts[0]);

                int parseIntOrFail(int index)
                {
                    return int.Parse(parts[index]);
                }

                Common.Rectangle rect = new Common.Rectangle(
                    new Common.Position(parseIntOrFail(1), parseIntOrFail(2)),
                    new Common.Size(parseIntOrFail(3), parseIntOrFail(4)));

                canvasBuilder(shapeType, rect);
            }
        }
    }
}
