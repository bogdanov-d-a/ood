using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Utils
{
    public class ShapeToFileSerializer : IShapeSerializer
    {
        private readonly string _fileName;

        public ShapeToFileSerializer(string fileName)
        {
            _fileName = fileName;
        }

        public void SerializeShapes(ShapeEnumerator shapeEnumerator)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(_fileName))
            {
                shapeEnumerator((Common.ShapeType type, Common.Rectangle boundingRect) =>
                    file.WriteLine(ShapeTypeUtils.TypeToString(type)
                        + " " + boundingRect.Left
                        + " " + boundingRect.Top
                        + " " + boundingRect.Width
                        + " " + boundingRect.Height));
            }
        }
    }
}
