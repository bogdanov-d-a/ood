using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes
{
    public class CanvasView
    {
        public interface IRenderTarget
        {
            void DrawRectangle(Common.Rectangle rect);
            void DrawTriangle(Common.Rectangle rect);
            void DrawCircle(Common.Rectangle rect);
            void DrawSelectionRectangle(Common.Rectangle rect);
        }

        public interface IDrawable
        {
            void Draw(IRenderTarget target);
        }

        private IList<IDrawable> _drawables = null;
        private Option<Common.Rectangle> _selectionRect = Option.None<Common.Rectangle>();

        public Option<Common.Size> CanvasSize
        {
            get; set;
        }

        public void Paint(IRenderTarget target)
        {
            if (_drawables == null)
            {
                return;
            }

            foreach (var drawable in _drawables)
            {
                drawable.Draw(target);
            }

            if (_selectionRect.HasValue)
            {
                const int SelOffset = 5;
                var rect = _selectionRect.ValueOrFailure();
                target.DrawSelectionRectangle(new Common.Rectangle(
                    new Common.Position(
                        rect.Left - SelOffset,
                        rect.Top - SelOffset),
                    new Common.Size(
                        rect.Width + 2 * SelOffset,
                        rect.Height + 2 * SelOffset)));
            }
        }

        public void UpdateLayout(IList<IDrawable> drawables, Option<Common.Rectangle> selectionRect)
        {
            _drawables = drawables;
            _selectionRect = selectionRect;
            LayoutUpdatedEvent();
        }

        public delegate void VoidDelegate();
        public VoidDelegate AddRectangleEvent;
        public VoidDelegate AddTriangleEvent;
        public VoidDelegate AddCircleEvent;
        public VoidDelegate RemoveShapeEvent;
        public VoidDelegate UndoEvent;
        public VoidDelegate RedoEvent;
        public event VoidDelegate LayoutUpdatedEvent;

        public delegate void MouseDelegate(Common.Position pos);
        public MouseDelegate MouseDownEvent;
        public MouseDelegate MouseUpEvent;
        public MouseDelegate MouseMoveEvent;
    }
}
