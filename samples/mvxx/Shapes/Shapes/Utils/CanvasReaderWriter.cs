using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes
{
    public class CanvasReaderWriter
    {
        private static SortedDictionary<string, Common.ShapeType> stringToShapeTypeMap = new SortedDictionary<string, Common.ShapeType>() {
            { "rectangle", Common.ShapeType.Rectangle },
            { "triangle", Common.ShapeType.Triangle },
            { "circle", Common.ShapeType.Circle },
        };

        public delegate void ReadShapeDelegate(Common.ShapeType type, Common.Rectangle boundingRect);

        public static void Read(string fileName, ReadShapeDelegate delegate_)
        {
            foreach (string line in System.IO.File.ReadAllLines(fileName))
            {
                var parts = line.Split(' ');
                if (parts.Length != 5)
                {
                    throw new Exception();
                }

                Common.ShapeType shapeType;
                if (!stringToShapeTypeMap.TryGetValue(parts[0], out shapeType))
                {
                    throw new Exception();
                }

                int parseIntOrFail(int index)
                {
                    return int.Parse(parts[index]);
                }

                Common.Rectangle rect = new Common.Rectangle(
                    new Common.Position(parseIntOrFail(1), parseIntOrFail(2)),
                    new Common.Size(parseIntOrFail(3), parseIntOrFail(4)));

                delegate_(shapeType, rect);
            }
        }

        private static string ShapeTypeToString(Common.ShapeType type)
        {
            foreach (var pair in stringToShapeTypeMap)
            {
                if (pair.Value == type)
                {
                    return pair.Key;
                }
            }
            throw new Exception();
        }

        public delegate void WriteShapeDelegate(Common.ShapeType type, Common.Rectangle boundingRect);
        public delegate void WriteShapesDelegate(WriteShapeDelegate delegate_);

        public static void Write(WriteShapesDelegate delegate_, string fileName)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                delegate_((Common.ShapeType type, Common.Rectangle boundingRect) => {
                    file.WriteLine(ShapeTypeToString(type)
                        + " " + boundingRect.Left
                        + " " + boundingRect.Top
                        + " " + boundingRect.Width
                        + " " + boundingRect.Height);
                });
            }
        }
    }
}
