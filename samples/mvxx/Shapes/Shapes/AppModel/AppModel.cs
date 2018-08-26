﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.AppModel
{
    public class AppModel
    {
        private static readonly Common.Rectangle defRect = new Common.Rectangle(new Common.Position(200, 100), new Common.Size(300, 200));

        private readonly DomainModel.Canvas canvas;
        private int selectedIndex = -1;

        public AppModel(DomainModel.Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void AddRectangle()
        {
            canvas.AddRectangle(defRect);
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

        public int GetSelectedIndex()
        {
            return selectedIndex;
        }

        public void SelectRectangle(int index)
        {
            selectedIndex = index;
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
