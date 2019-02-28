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
        private readonly CanvasView _canvasView;
        private readonly ControlView _controlView;
        private Option<Common.Position<int>> _touchPos = Option.None<Common.Position<int>>();

        public ShapesLiteForm(CanvasView canvasView, InfoView infoView, ControlView controlView)
        {
            InitializeComponent();
            DoubleBuffered = true;

            _canvasView = canvasView;
            _canvasView.InvalidateEvent += () => Invalidate();

            infoTextBox.Text = infoView.GetText();
            infoView.TextChangedEvent += (string text) => {
                infoTextBox.Text = text;
            };

            _controlView = controlView;
        }

        private void ShapesLiteForm_Paint(object sender, PaintEventArgs e)
        {
            _canvasView.Draw(e.Graphics);
        }

        private bool IsInsideShape(Common.Position<int> position)
        {
            return _canvasView.ShapeBoundingRect.Value.Contains(position);
        }

        private Common.Position<int> GetMousePos()
        {
            Point rawPos = PointToClient(MousePosition);
            return new Common.Position<int>(rawPos.X - CanvasView.DrawOffset, rawPos.Y - CanvasView.DrawOffset);
        }

        private void ShapesLiteForm_MouseDown(object sender, MouseEventArgs e)
        {
            Common.Position<int> pos = GetMousePos();
            _canvasView.IsShapeSelected.Value = IsInsideShape(pos);
            _touchPos = _canvasView.IsShapeSelected.Value
                ? Option.Some(new Common.Position<int>(pos.x - _canvasView.ShapeBoundingRect.Value.Left, pos.y - _canvasView.ShapeBoundingRect.Value.Top))
                : Option.None<Common.Position<int>>();
        }

        private void UpdateViewShapePosition()
        {
            Common.Position<int> pos = GetMousePos();
            Common.Position<int> touchPos = _touchPos.ValueOrFailure();
            Common.Size<int> size = _canvasView.ShapeBoundingRect.Value.Size;
            _canvasView.ShapeBoundingRect.Value = new Common.RectangleInt(
                pos.x - touchPos.x, pos.y - touchPos.y, size.width, size.height);
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
            _canvasView.OnFinishMovingEvent(_canvasView.ShapeBoundingRect.Value);
            _touchPos = Option.None<Common.Position<int>>();
        }

        private void resetPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controlView.ResetPositionEvent();
        }

        private void flipSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controlView.FlipSelectionEvent();
        }
    }
}
