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
        private readonly View _view;
        private Option<Common.Position<int>> _touchPos = Option.None<Common.Position<int>>();

        public ShapesLiteForm(View view, InfoView infoView)
        {
            InitializeComponent();
            DoubleBuffered = true;

            _view = view;
            _view.InvalidateEvent += () => Invalidate();

            infoTextBox.Text = infoView.GetText();
            infoView.ModelRect.Event += (Common.Rectangle<double> rect) => {
                infoTextBox.Text = infoView.GetText();
            };
        }

        private void ShapesLiteForm_Paint(object sender, PaintEventArgs e)
        {
            _view.Draw(e.Graphics);
        }

        private bool IsInsideShape(Common.Position<int> position)
        {
            return _view.ShapeBoundingRect.Value.Contains(position);
        }

        private Common.Position<int> GetMousePos()
        {
            Point rawPos = PointToClient(MousePosition);
            return new Common.Position<int>(rawPos.X - View.DrawOffset, rawPos.Y - View.DrawOffset);
        }

        private void ShapesLiteForm_MouseDown(object sender, MouseEventArgs e)
        {
            Common.Position<int> pos = GetMousePos();
            _view.IsShapeSelected.Value = IsInsideShape(pos);
            _touchPos = _view.IsShapeSelected.Value
                ? Option.Some(new Common.Position<int>(pos.x - _view.ShapeBoundingRect.Value.Left, pos.y - _view.ShapeBoundingRect.Value.Top))
                : Option.None<Common.Position<int>>();
        }

        private void UpdateViewShapePosition()
        {
            Common.Position<int> pos = GetMousePos();
            Common.Position<int> touchPos = _touchPos.ValueOrFailure();
            Common.Size<int> size = _view.ShapeBoundingRect.Value.Size;
            _view.ShapeBoundingRect.Value = new Common.RectangleInt(
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
            _view.OnFinishMovingEvent(_view.ShapeBoundingRect.Value);
            _touchPos = Option.None<Common.Position<int>>();
        }
    }
}
