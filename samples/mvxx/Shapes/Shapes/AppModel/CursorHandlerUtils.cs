using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.AppModel
{
    class CursorHandlerUtils
    {
        private CursorHandlerUtils()
        {
        }

        public static void OffsetClampBounds(ref Common.Rectangle rectangle, Common.Size canvasSize)
        {
            rectangle.Left = Math.Max(0, rectangle.Left);
            rectangle.Top = Math.Max(0, rectangle.Top);

            int moveX = Math.Min(canvasSize.width - 1, rectangle.Right) - rectangle.Right;
            rectangle.Left += moveX;

            int moveY = Math.Min(canvasSize.height - 1, rectangle.Bottom) - rectangle.Bottom;
            rectangle.Top += moveY;
        }

        public static void ResizeClampBounds(ref Common.Rectangle rectangle, Common.Size canvasSize)
        {
            var oldRightBottom = rectangle.RightBottom;

            rectangle.Left = Math.Max(0, rectangle.Left);
            rectangle.Top = Math.Max(0, rectangle.Top);

            rectangle.Right = Math.Min(canvasSize.width - 1, oldRightBottom.x);
            rectangle.Bottom = Math.Min(canvasSize.height - 1, oldRightBottom.y);
        }

        public static bool DoesValueMatch(int target, int tolerance, int value)
        {
            return (value > target - tolerance) && (value < target + tolerance);
        }

        public enum RectEdgeHor
        {
            Top,
            Bottom,
            None,
        };

        public enum RectEdgeVert
        {
            Left,
            Right,
            None,
        };

        public struct RectEdges
        {
            public RectEdgeHor hor;
            public RectEdgeVert vert;
        };

        public static RectEdges FindRectEdges(Common.Rectangle rect, Common.Position point)
        {
            const int Tolerance = 4;

            RectEdges result = new RectEdges();
            result.hor = RectEdgeHor.None;
            result.vert = RectEdgeVert.None;

            bool xMatchesLeft = DoesValueMatch(rect.Left, Tolerance, point.x);
            bool xMatchesRight = DoesValueMatch(rect.Right, Tolerance, point.x);
            bool xIntersects = (rect.Left - Tolerance < point.x
                && point.x < rect.Right + Tolerance);

            bool yMatchesTop = DoesValueMatch(rect.Top, Tolerance, point.y);
            bool yMatchesBottom = DoesValueMatch(rect.Bottom, Tolerance, point.y);
            bool yIntersects = (rect.Top - Tolerance < point.y
                && point.y < rect.Bottom + Tolerance);

            if (yIntersects)
            {
                result.vert = xMatchesLeft ? RectEdgeVert.Left :
                    xMatchesRight ? RectEdgeVert.Right :
                    RectEdgeVert.None;
            }

            if (xIntersects)
            {
                result.hor = yMatchesTop ? RectEdgeHor.Top :
                    yMatchesBottom ? RectEdgeHor.Bottom :
                    RectEdgeHor.None;
            }

            return result;
        }
    }
}
