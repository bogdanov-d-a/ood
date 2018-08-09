using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes
{
    public class Presenter
    {
        private static readonly Common.Rectangle defRect = new Common.Rectangle(new Common.Position(200, 100), new Common.Size(300, 200));

        private readonly Model.Canvas canvas;
        private readonly List<bool> selection = new List<bool>();

        public Presenter(Model.Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void AddRectangle()
        {
            canvas.AddRectangle(defRect);
            selection.Add(false);
            LayoutUpdatedEvent();
        }

        public Common.Rectangle GetRectangle(int index)
        {
            return canvas.GetRectangle(index);
        }

        public void ResetRectangle(int index, Common.Rectangle rect)
        {
            canvas.ResetRectangle(index, rect);
            LayoutUpdatedEvent();
        }

        public bool CheckBounds(Common.Rectangle rectangle)
        {
            return canvas.CheckBounds(rectangle);
        }

        public bool IsRectangleSelected(int index)
        {
            return selection[index];
        }

        public int GetSelectedIndex()
        {
            for (int i = 0; i < selection.Count; ++i)
            {
                if (selection[i])
                {
                    return i;
                }
            }
            return -1;
        }

        public void SelectRectangle(int index, bool selected)
        {
            selection[index] = selected;
            LayoutUpdatedEvent();
        }

        public int RectangleCount
        {
            get {
                return canvas.Count;
            }
        }

        public Common.Size CanvasSize
        {
            get {
                return canvas.SizeP;
            }
        }

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
