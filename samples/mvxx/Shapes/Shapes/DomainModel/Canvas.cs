﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class Canvas
    {
        private readonly Common.Size _canvasSize;
        private readonly List<Common.Shape> _shapeList = new List<Common.Shape>();

        public Canvas(Common.Size canvasSize)
        {
            _canvasSize = canvasSize;
        }

        private bool IsShapeInsideCanvas(Common.Rectangle rectangle)
        {
            return rectangle.Left >= 0 &&
                rectangle.Top >= 0 &&
                rectangle.Right < _canvasSize.width &&
                rectangle.Bottom < _canvasSize.height;
        }

        public void InsertShape(int index, Common.Shape shape)
        {
            if (!IsShapeInsideCanvas(shape.boundingRect))
            {
                throw new Exception();
            }
            _shapeList.Insert(index, shape);
        }

        public Common.Shape GetShapeAt(int index)
        {
            return _shapeList.ElementAt(index);
        }

        public void RemoveShapeAt(int index)
        {
            _shapeList.RemoveAt(index);
        }

        public Common.Size CanvasSize
        {
            get => _canvasSize;
        }

        public int ShapeCount
        {
            get => _shapeList.Count;
        }
    }
}
