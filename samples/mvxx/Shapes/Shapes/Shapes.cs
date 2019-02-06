using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Optional;
using Optional.Unsafe;

namespace Shapes
{
    public partial class Shapes : Form
    {
        public delegate bool BoolDelegate();

        private const int DrawOffset = 50;
        private readonly View.CanvasView _canvasView;
        private readonly View.ShapeActionsView _shapeActionsView;
        private readonly View.MouseEventsView _mouseEventsView;
        private readonly View.UndoRedoActionsView _undoRedoActionsView;
        private readonly View.DocumentLifecycleActionsView _documentLifecycleActionsView;
        private readonly DialogsViewHandler _dialogsViewHandler;
        private BoolDelegate _formClosingHandler;

        private class DialogsViewHandler : View.IDialogsView
        {
            private readonly Shapes _shapesForm;

            public DialogsViewHandler(Shapes shapesForm)
            {
                _shapesForm = shapesForm;
            }

            public Option<string> ShowOpenFileDialog()
            {
                if (_shapesForm.openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return Option.Some(_shapesForm.openFileDialog.FileName);
                }
                return Option.None<string>();
            }

            public Option<string> ShowSaveFileDialog()
            {
                if (_shapesForm.saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return Option.Some(_shapesForm.saveFileDialog.FileName);
                }
                return Option.None<string>();
            }

            public Common.ClosingAction ShowUnsavedDocumentClosePrompt()
            {
                DialogResult result = MessageBox.Show("Save document before closing?", "Warning",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    return Common.ClosingAction.Save;
                }
                else if (result == DialogResult.No)
                {
                    return Common.ClosingAction.DontSave;
                }
                return Common.ClosingAction.DontClose;
            }
        }

        private class ViewHandlers : View.CanvasView.IViewHandlers
        {
            private readonly Shapes _shapesForm;

            public ViewHandlers(Shapes shapesForm)
            {
                _shapesForm = shapesForm;
            }

            public void InvalidateLayout()
            {
                _shapesForm.Invalidate();
            }
        }

        private static Rectangle OffsetDrawRect(Common.Rectangle rect)
        {
            return new Rectangle(rect.Left + DrawOffset,
                rect.Top + DrawOffset,
                rect.Width,
                rect.Height);
        }

        private class RenderTarget : View.CanvasView.IRenderTarget
        {
            private Graphics g;

            public RenderTarget(Graphics g)
            {
                this.g = g;
            }

            public void DrawRectangle(Common.Rectangle rect)
            {
                var rect2 = OffsetDrawRect(rect);
                g.FillRectangle(new SolidBrush(Color.Yellow), rect2);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rect2);
            }

            public void DrawTriangle(Common.Rectangle rect)
            {
                var rect2 = OffsetDrawRect(rect);
                Point[] points = {
                            new Point((rect2.Left + rect2.Right) / 2, rect2.Top),
                            new Point(rect2.Left, rect2.Bottom),
                            new Point(rect2.Right, rect2.Bottom),
                        };
                g.FillPolygon(new SolidBrush(Color.Yellow), points);
                g.DrawPolygon(new Pen(new SolidBrush(Color.Black)), points);
            }

            public void DrawCircle(Common.Rectangle rect)
            {
                var rect2 = OffsetDrawRect(rect);
                g.FillEllipse(new SolidBrush(Color.Yellow), rect2);
                g.DrawEllipse(new Pen(new SolidBrush(Color.Black)), rect2);
            }

            public void DrawSelectionRectangle(Common.Rectangle rect)
            {
                var rect2 = OffsetDrawRect(rect);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), rect2);
            }
        }

        public Shapes(View.CanvasView canvasView, View.ShapeActionsView shapeActionsView,
            View.MouseEventsView mouseEventsView, View.UndoRedoActionsView undoRedoActionsView,
            View.DocumentLifecycleActionsView documentLifecycleActionsView)
        {
            InitializeComponent();
            DoubleBuffered = true;

            _canvasView = canvasView;
            _canvasView.ViewHandlers = new ViewHandlers(this);

            _shapeActionsView = shapeActionsView;
            _mouseEventsView = mouseEventsView;
            _undoRedoActionsView = undoRedoActionsView;
            _documentLifecycleActionsView = documentLifecycleActionsView;

            _dialogsViewHandler = new DialogsViewHandler(this);
        }

        public View.IDialogsView DialogsView
        {
            get => _dialogsViewHandler;
        }

        public void SetFormClosingHandler(BoolDelegate handler)
        {
            _formClosingHandler = handler;
        }

        private void Shapes_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Common.Size canvasSize = _canvasView.ViewEvents.CanvasSize;

            g.FillRectangle(new SolidBrush(Color.White),
                new Rectangle(DrawOffset,
                    DrawOffset,
                    canvasSize.width,
                    canvasSize.height));

            _canvasView.Paint(new RenderTarget(g));
        }

        private Common.Position GetMousePosition()
        {
            Point rawPos = PointToClient(MousePosition);
            return new Common.Position(rawPos.X - DrawOffset, rawPos.Y - DrawOffset);
        }

        private void Shapes_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseEventsView.OnDown(GetMousePosition());
        }

        private void Shapes_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseEventsView.OnUp(GetMousePosition());
        }

        private void Shapes_MouseMove(object sender, MouseEventArgs e)
        {
            _mouseEventsView.OnMove(GetMousePosition());
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            _undoRedoActionsView.OnUndo();
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            _undoRedoActionsView.OnRedo();
        }

        private void addRectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _shapeActionsView.OnAddRectangle();
        }

        private void addTriangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _shapeActionsView.OnAddTriangle();
        }

        private void addCircleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _shapeActionsView.OnAddCircle();
        }

        private void removeShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _shapeActionsView.OnRemove();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentLifecycleActionsView.OnNew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentLifecycleActionsView.OnOpen();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentLifecycleActionsView.OnSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentLifecycleActionsView.OnSaveAs();
        }

        private void Shapes_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_formClosingHandler())
            {
                e.Cancel = true;
            }
        }
    }
}
