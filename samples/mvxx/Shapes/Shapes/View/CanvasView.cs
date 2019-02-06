using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.View
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

        private List<Common.Shape> _shapes = new List<Common.Shape>();
        private int _selectionIndex = -1;

        public void Paint(IRenderTarget target)
        {
            foreach (var shape in _shapes)
            {
                switch (shape.type)
                {
                    case Common.ShapeType.Rectangle:
                        target.DrawRectangle(shape.boundingRect);
                        break;
                    case Common.ShapeType.Triangle:
                        target.DrawTriangle(shape.boundingRect);
                        break;
                    case Common.ShapeType.Circle:
                        target.DrawCircle(shape.boundingRect);
                        break;
                    default:
                        throw new Exception();
                }
            }

            if (_selectionIndex != -1)
            {
                const int SelOffset = 5;
                var rect = GetShape(_selectionIndex).boundingRect;
                target.DrawSelectionRectangle(new Common.Rectangle(
                    new Common.Position(
                        rect.Left - SelOffset,
                        rect.Top - SelOffset),
                    new Common.Size(
                        rect.Width + 2 * SelOffset,
                        rect.Height + 2 * SelOffset)));
            }
        }

        public void AddShape(int index, Common.Shape shape)
        {
            _shapes.Insert(index, shape);
        }

        public Common.Shape GetShape(int index)
        {
            return _shapes.ElementAt(index);
        }

        public void RemoveShape(int index)
        {
            _shapes.RemoveAt(index);
        }

        public int ShapeCount
        {
            get => _shapes.Count;
        }

        public void SetSelectionIndex(int index)
        {
            _selectionIndex = index;
        }

        public interface IViewEvents
        {
            bool FormClosing();
            Common.Size CanvasSize { get; }
        }

        public IViewEvents ViewEvents = null;

        public interface IDialogHandlers
        {
            Option<string> ShowOpenFileDialog();
            Option<string> ShowSaveFileDialog();
            Common.ClosingAction ShowUnsavedDocumentClosePrompt();
        }

        public interface IViewHandlers
        {
            void InvalidateLayout();
            IDialogHandlers DialogHandlers { get; }
        }

        public IViewHandlers ViewHandlers = null;
    }
}
