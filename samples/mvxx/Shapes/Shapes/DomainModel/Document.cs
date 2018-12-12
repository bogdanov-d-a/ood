using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;

namespace Shapes.DomainModel
{
    public class Document
    {
        public interface IShape
        {
            Common.ShapeType GetShapeType();
            Common.Rectangle GetBoundingRect();
            void SetBoundingRect(Common.Rectangle rect);
        }

        public interface IDelegate
        {
            void OnOpenDocument(string path);
            void OnSaveDocument(string path);

            Option<string> RequestDocumentOpenPath();
            DocumentLifecycleController.ClosingAction RequestUnsavedDocumentClosing();
            Option<string> RequestDocumentSavePath();
        }

        private class Shape : IShape
        {
            private class Movable : MoveShapeCommand.IMovable
            {
                private readonly Canvas _canvas;
                private readonly int _index;

                public Movable(Canvas canvas, int index)
                {
                    _canvas = canvas;
                    _index = index;
                }

                public Common.Shape GetShape()
                {
                    return _canvas.GetShape(_index);
                }

                public Common.Rectangle GetRect()
                {
                    return GetShape().boundingRect;
                }

                public void SetRect(Common.Rectangle rect)
                {
                    GetShape().boundingRect = rect;
                }
            }

            private readonly Movable _movable;
            private readonly Document _parent;

            public Shape(Canvas canvas, int index, Document parent)
            {
                _movable = new Movable(canvas, index);
                _parent = parent;
            }

            public Common.ShapeType GetShapeType()
            {
                return _movable.GetShape().type;
            }

            public Common.Rectangle GetBoundingRect()
            {
                return _movable.GetRect();
            }

            public void SetBoundingRect(Common.Rectangle rect)
            {
                if (!GetBoundingRect().Equals(rect))
                {
                    _parent._history.AddAndExecuteCommand(new MoveShapeCommand(_movable, rect));
                    _parent._dlc.Modify();
                }
            }
        }

        private delegate void VoidDelegate();

        private void ExecuteWithLayoutUpdatedEventSuspended(VoidDelegate delegate_)
        {
            if (_suspendLayoutUpdatedEvent)
            {
                throw new Exception();
            }
            _suspendLayoutUpdatedEvent = true;

            try
            {
                delegate_();
            }
            finally
            {
                _suspendLayoutUpdatedEvent = false;
            }
        }

        private class DocumentLifecycleControllerDelegate : DocumentLifecycleController.IDelegate
        {
            private readonly Document _parent;

            public DocumentLifecycleControllerDelegate(Document parent)
            {
                _parent = parent;
            }

            public void OnEraseMemoryDocument()
            {
                _parent.ExecuteWithLayoutUpdatedEventSuspended(() => {
                    _parent._canvas.RemoveAllShapes();
                    _parent._history.Clear();
                });
                _parent.LayoutUpdatedEvent();
            }

            public void OnOpenDocument(string path)
            {
                _parent._delegate.OnOpenDocument(path);
            }

            public void OnSaveDocument(string path)
            {
                _parent._delegate.OnSaveDocument(path);
            }

            public Option<string> RequestDocumentOpenPath()
            {
                return _parent._delegate.RequestDocumentOpenPath();
            }

            public Option<string> RequestDocumentSavePath()
            {
                return _parent._delegate.RequestDocumentSavePath();
            }

            public DocumentLifecycleController.ClosingAction RequestUnsavedDocumentClosing()
            {
                return _parent._delegate.RequestUnsavedDocumentClosing();
            }
        }

        private readonly IDelegate _delegate;
        private readonly Canvas _canvas;
        private readonly History _history;
        private readonly HistoryCanvas _historyCanvas;
        private readonly DocumentLifecycleController _dlc;
        private bool _suspendLayoutUpdatedEvent = false;

        public Document(IDelegate delegate_, Canvas canvas)
        {
            _delegate = delegate_;
            _canvas = canvas;
            _history = new History();
            _historyCanvas = new HistoryCanvas(_canvas, (ICommand command) => {
                _history.AddAndExecuteCommand(command);
                _dlc.Modify();
            });
            _canvas.LayoutUpdatedEvent += new Canvas.LayoutUpdatedDelegate(() => {
                if (!_suspendLayoutUpdatedEvent)
                {
                    LayoutUpdatedEvent();
                }
            });
            _dlc = new DocumentLifecycleController(new DocumentLifecycleControllerDelegate(this));
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            _historyCanvas.AddShape(type, rect);
        }

        public IShape GetShape(int index)
        {
            return new Shape(_canvas, index, this);
        }

        public void RemoveShape(int index)
        {
            _historyCanvas.RemoveShape(index);
        }

        public Common.Size CanvasSize
        {
            get
            {
                return _canvas.CanvasSize;
            }
        }

        public int ShapeCount
        {
            get
            {
                return _canvas.ShapeCount;
            }
        }

        public bool New()
        {
            return _dlc.New();
        }

        public void Open()
        {
            _dlc.Open();
        }

        public void Save()
        {
            _dlc.Save(false);
        }

        public void SaveAs()
        {
            _dlc.Save(true);
        }

        public void Undo()
        {
            _history.Undo();
            _dlc.Modify();
            LayoutUpdatedEvent();
        }

        public void Redo()
        {
            _history.Redo();
            _dlc.Modify();
            LayoutUpdatedEvent();
        }

        public delegate void AddShapeDelegate(Common.ShapeType type, Common.Rectangle boundingRect);
        public delegate void AddShapesDelegate(AddShapeDelegate delegate_);

        public void ReplaceCanvasData(AddShapesDelegate delegate_)
        {
            ExecuteWithLayoutUpdatedEventSuspended(() => {
                delegate_((Common.ShapeType type, Common.Rectangle boundingRect) => {
                    _canvas.InsertShape(_canvas.ShapeCount, new Common.Shape(type, boundingRect));
                });
            });
            LayoutUpdatedEvent();
        }

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
