using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Utils
{
    class ShapeTypeUtils
    {
        private static SortedDictionary<string, Common.ShapeType> stringToShapeTypeMap = new SortedDictionary<string, Common.ShapeType>() {
            { "rectangle", Common.ShapeType.Rectangle },
            { "triangle", Common.ShapeType.Triangle },
            { "circle", Common.ShapeType.Circle },
        };

        public static Common.ShapeType StringToType(string str)
        {
            Common.ShapeType result;
            if (!stringToShapeTypeMap.TryGetValue(str, out result))
            {
                throw new Exception();
            }
            return result;
        }

        public static string TypeToString(Common.ShapeType type)
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
    }
}
