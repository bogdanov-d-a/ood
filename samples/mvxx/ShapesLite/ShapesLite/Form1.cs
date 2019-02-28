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
    public partial class Form1 : Form
    {
        private readonly View _view;
        private Option<Common.Position<int>> _touchPos = Option.None<Common.Position<int>>();

        public Form1(View view)
        {
            InitializeComponent();
            DoubleBuffered = true;

            _view = view;
            _view.InvalidateEvent += () => Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            _view.Draw(e.Graphics);
        }

        private bool IsInsideShape(Common.Position<int> position)
        {
            return _view.Position.Value.Contains(position);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Point rawPos = PointToClient(MousePosition);
            Common.Position<int> pos = new Common.Position<int>(rawPos.X - View.DrawOffset, rawPos.Y - View.DrawOffset);
            _view.IsSelected.Value = IsInsideShape(pos);
            _touchPos = Option.Some(new Common.Position<int>(pos.x - _view.Position.Value.Left, pos.y - _view.Position.Value.Top));
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_touchPos.HasValue)
            {
                return;
            }

            Point rawPos = PointToClient(MousePosition);
            Common.Position<int> pos = new Common.Position<int>(rawPos.X - View.DrawOffset, rawPos.Y - View.DrawOffset);
            Common.Position<int> touchPos = _touchPos.ValueOrFailure();
            Common.Size<int> size = _view.Position.Value.Size;
            _view.Position.Value = new Common.RectangleInt(
                pos.x - touchPos.x, pos.y - touchPos.y, size.width, size.height);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Point rawPos = PointToClient(MousePosition);
            Common.Position<int> pos = new Common.Position<int>(rawPos.X - View.DrawOffset, rawPos.Y - View.DrawOffset);
            Common.Position<int> touchPos = _touchPos.ValueOrFailure();
            Common.Size<int> size = _view.Position.Value.Size;
            _view.Position.Value = new Common.RectangleInt(
                pos.x - touchPos.x, pos.y - touchPos.y, size.width, size.height);
            _view.OnFinishMovingEvent(_view.Position.Value);
            _touchPos = Option.None<Common.Position<int>>();
        }
    }
}
