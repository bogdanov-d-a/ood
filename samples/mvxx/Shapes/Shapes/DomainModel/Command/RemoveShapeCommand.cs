﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.DomainModel
{
    public class RemoveShapeCommand : AbstractCommand
    {
        public interface ICanvas
        {
            Canvas.Shape GetAt(int index);
            void Insert(int index, Canvas.Shape shape);
            void RemoveAt(int index);
        }

        private readonly ICanvas _canvas;
        private readonly int _index;
        private Option<Canvas.Shape> _shape;

        public RemoveShapeCommand(ICanvas canvas, int index)
        {
            _canvas = canvas;
            _index = index;
            _shape = Option.None<Canvas.Shape>();
        }

        protected override void ExecuteImpl()
        {
            _shape = Option.Some(_canvas.GetAt(_index));
            _canvas.RemoveAt(_index);
        }

        protected override void UnexecuteImpl()
        {
            _canvas.Insert(_index, _shape.ValueOrFailure());
            _shape = Option.None<Canvas.Shape>();
        }
    }
}