﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public interface IShapeFactory
    {
        IShape CreateShape(ShapeTypes.Type type, Common.Rectangle boundingRect);
    }
}
