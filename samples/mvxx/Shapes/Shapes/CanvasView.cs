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

        public CanvasView(RequestPaintingDelegate requestPainting)
        {
            _requestPainting = requestPainting;
        }

        public Option<Common.Size> CanvasSize
        {
            get; set;
        }

        public void Paint(IRenderTarget target)
        {
            const int SelOffset = 5;

            _requestPainting((IDrawable drawable) => {
                drawable.Draw(target);
            },
            (Common.Rectangle rect) => {
                target.DrawSelectionRectangle(new Common.Rectangle(
                    new Common.Position(
                        rect.Left - SelOffset,
                        rect.Top - SelOffset),
                    new Common.Size(
                        rect.Width + 2 * SelOffset,
                        rect.Height + 2 * SelOffset)));
            });
        }

        public delegate void VoidDelegate();
        public VoidDelegate AddRectangleEvent;
        public VoidDelegate AddTriangleEvent;
        public VoidDelegate AddCircleEvent;
        public VoidDelegate RemoveShapeEvent;
        public VoidDelegate UndoEvent;
        public VoidDelegate RedoEvent;
        public VoidDelegate LayoutUpdatedEvent;

        public delegate void MouseDelegate(Common.Position pos);
        public MouseDelegate MouseDownEvent;
        public MouseDelegate MouseUpEvent;
        public MouseDelegate MouseMoveEvent;

        public delegate void DrawableDelegate(IDrawable drawable);
        public delegate void SelectionDelegate(Common.Rectangle rect);
        public delegate void RequestPaintingDelegate(DrawableDelegate drawableDelegate, SelectionDelegate selectionDelegate);
        private readonly RequestPaintingDelegate _requestPainting;
    }
}
