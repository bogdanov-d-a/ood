using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Optional;
using Optional.Unsafe;

namespace ShapesLite
{
    public partial class ShapesLiteForm : Form
    {
        private readonly Views.CanvasView _canvasView;
        private readonly Views.ControlView _controlView;
        private Option<Common.Position<int>> _touchPos = Option.None<Common.Position<int>>();

        public ShapesLiteForm(Views.CanvasView canvasView, Views.ControlView controlView)
        {
            InitializeComponent();
            DoubleBuffered = true;

            _canvasView = canvasView;
            _canvasView.InvalidateEvent += () => Invalidate();

            _controlView = controlView;
        }

        private void ShapesLiteForm_Paint(object sender, PaintEventArgs e)
        {
            _canvasView.Draw(e.Graphics);
        }

        private bool IsInsideShape(int index, Common.Position<int> position)
        {
            return _canvasView.ShapeList.GetAt(index).Contains(position);
        }

        private int FindShapeByPosition(Common.Position<int> position)
        {
            for (int i = 0; i < _canvasView.ShapeList.Count; ++i)
            {
                if (IsInsideShape(i, position))
                {
                    return i;
                }
            }
            return -1;
        }

        private Common.Position<int> GetMousePos()
        {
            Point rawPos = PointToClient(MousePosition);
            return new Common.Position<int>(rawPos.X - Views.CanvasView.DrawOffset, rawPos.Y - Views.CanvasView.DrawOffset);
        }

        private void ShapesLiteForm_MouseDown(object sender, MouseEventArgs e)
        {
            Common.Position<int> pos = GetMousePos();
            _canvasView.SelectedShapeIndex.Value = FindShapeByPosition(pos);

            if (_canvasView.SelectedShapeIndex.Value != -1)
            {
                var shape = _canvasView.ShapeList.GetAt(_canvasView.SelectedShapeIndex.Value);
                _touchPos = Option.Some(new Common.Position<int>(pos.x - shape.Left, pos.y - shape.Top));
            }
            else
            {
                _touchPos = Option.None<Common.Position<int>>();
            }
        }

        private void UpdateViewShapePosition()
        {
            Common.Position<int> pos = GetMousePos();
            Common.Position<int> touchPos = _touchPos.ValueOrFailure();
            int index = _canvasView.SelectedShapeIndex.Value;
            Common.Size<int> size = _canvasView.ShapeList.GetAt(index).Size;
            _canvasView.ShapeList.SetAt(index, new Common.RectangleInt(
                pos.x - touchPos.x, pos.y - touchPos.y, size.width, size.height));
        }

        private void ShapesLiteForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_touchPos.HasValue)
            {
                return;
            }
            UpdateViewShapePosition();
        }

        private void ShapesLiteForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_touchPos.HasValue)
            {
                return;
            }
            UpdateViewShapePosition();
            int index = _canvasView.SelectedShapeIndex.Value;
            _canvasView.OnFinishMovingEvent(index, _canvasView.ShapeList.GetAt(index));
            _touchPos = Option.None<Common.Position<int>>();
        }

        private void addShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controlView.AddShapeEvent();
        }

        private void removeShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controlView.RemoveShapeEvent();
        }
    }
}
