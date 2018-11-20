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
        private const int DrawOffset = 50;
        private readonly CanvasView _canvasView;

        private static Rectangle OffsetDrawRect(Common.Rectangle rect)
        {
            return new Rectangle(rect.Left + DrawOffset,
                rect.Top + DrawOffset,
                rect.Width,
                rect.Height);
        }

        private class RenderTarget : CanvasView.IRenderTarget
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

        public Shapes(CanvasView canvasView)
        {
            InitializeComponent();
            DoubleBuffered = true;

            _canvasView = canvasView;

            _canvasView.LayoutUpdatedEvent += new CanvasView.VoidDelegate(() => {
                Invalidate();
            });

            _canvasView.ShowOpenFileDialogEvent += new CanvasView.RequestDocumentPathDelegate(() => {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return Option.Some(openFileDialog.FileName);
                }
                return Option.None<string>();
            });

            _canvasView.ShowSaveFileDialogEvent += new CanvasView.RequestDocumentPathDelegate(() => {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return Option.Some(saveFileDialog.FileName);
                }
                return Option.None<string>();
            });

            _canvasView.ShowUnsavedDocumentClosePrompt += new CanvasView.RequestUnsavedDocumentClosingDelegate(() => {
                DialogResult result = MessageBox.Show("Save document before closing?", "Warning",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    return DomainModel.DocumentLifecycleController.ClosingAction.Save;
                }
                else if (result == DialogResult.No)
                {
                    return DomainModel.DocumentLifecycleController.ClosingAction.DontSave;
                }
                return DomainModel.DocumentLifecycleController.ClosingAction.DontClose;
            });
        }

        private void Shapes_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (!_canvasView.CanvasSize.HasValue)
            {
                return;
            }
            Common.Size canvasSize = _canvasView.CanvasSize.ValueOrFailure();

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
            _canvasView.MouseDownEvent(GetMousePosition());
        }

        private void Shapes_MouseUp(object sender, MouseEventArgs e)
        {
            _canvasView.MouseUpEvent(GetMousePosition());
        }

        private void Shapes_MouseMove(object sender, MouseEventArgs e)
        {
            _canvasView.MouseMoveEvent(GetMousePosition());
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            _canvasView.UndoEvent();
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            _canvasView.RedoEvent();
        }

        private void addRectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.AddRectangleEvent();
        }

        private void addTriangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.AddTriangleEvent();
        }

        private void addCircleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.AddCircleEvent();
        }

        private void removeShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.RemoveShapeEvent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.CreateNewDocumentEvent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.OpenDocumentEvent();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.SaveDocumentEvent();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.SaveAsDocumentEvent();
        }

        private void Shapes_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_canvasView.FormClosingEvent())
            {
                e.Cancel = true;
            }
        }
    }
}
