﻿using System;
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
        private readonly View.CanvasView _canvasView;
        private readonly View.ShapeActionsView _shapeActionsView;

        private class ViewHandlers : View.CanvasView.IViewHandlers
        {
            private class DialogHandlersImpl : View.CanvasView.IDialogHandlers
            {
                private readonly Shapes _shapesForm;

                public DialogHandlersImpl(Shapes shapesForm)
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

            private readonly Shapes _shapesForm;
            private readonly DialogHandlersImpl _dialogHandlers;

            public ViewHandlers(Shapes shapesForm)
            {
                _shapesForm = shapesForm;
                _dialogHandlers = new DialogHandlersImpl(_shapesForm);
            }

            public void InvalidateLayout()
            {
                _shapesForm.Invalidate();
            }

            public View.CanvasView.IDialogHandlers DialogHandlers
            {
                get => _dialogHandlers;
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

        public Shapes(View.CanvasView canvasView, View.ShapeActionsView shapeActionsView)
        {
            InitializeComponent();
            DoubleBuffered = true;

            _canvasView = canvasView;
            _canvasView.ViewHandlers = new ViewHandlers(this);

            _shapeActionsView = shapeActionsView;
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
            _canvasView.ViewEvents.MouseEvents.Down(GetMousePosition());
        }

        private void Shapes_MouseUp(object sender, MouseEventArgs e)
        {
            _canvasView.ViewEvents.MouseEvents.Up(GetMousePosition());
        }

        private void Shapes_MouseMove(object sender, MouseEventArgs e)
        {
            _canvasView.ViewEvents.MouseEvents.Move(GetMousePosition());
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            _canvasView.ViewEvents.HistoryEvents.Undo();
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            _canvasView.ViewEvents.HistoryEvents.Redo();
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
            _canvasView.ViewEvents.DocumentLifecycleEvents.New();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.ViewEvents.DocumentLifecycleEvents.Open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.ViewEvents.DocumentLifecycleEvents.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _canvasView.ViewEvents.DocumentLifecycleEvents.SaveAs();
        }

        private void Shapes_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_canvasView.ViewEvents.FormClosing())
            {
                e.Cancel = true;
            }
        }
    }
}
