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
            private readonly Document _parent;
            private readonly int _index;

            public Shape(Document parent, int index)
            {
                _parent = parent;
                _index = index;
            }

            private Common.Shape GetShape()
            {
                return _parent._canvas.GetShape(_index);
            }

            public Common.ShapeType GetShapeType()
            {
                return GetShape().type;
            }

            public Common.Rectangle GetBoundingRect()
            {
                return GetShape().boundingRect;
            }

            public void SetBoundingRect(Common.Rectangle rect)
            {
                if (!GetBoundingRect().Equals(rect))
                {
                    _parent._historyCanvas.MoveShape(_index, rect);
                }
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
                _parent._canvas.RemoveAllShapes();
                _parent._history.Clear();
                _parent.CompleteLayoutUpdateEvent();
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

        private class HistoryCanvasDelegate : HistoryCanvas.IDelegate
        {
            private readonly Document _parent;

            public HistoryCanvasDelegate(Document parent)
            {
                _parent = parent;
            }

            void HistoryCanvas.IDelegate.AddCommand(Command.ICommand command)
            {
                _parent._history.AddAndExecuteCommand(command);
            }

            void HistoryCanvas.IDelegate.OnInsertShape(int index)
            {
                _parent._dlc.Modify();
                _parent.ShapeInsertEvent(index);
            }

            void HistoryCanvas.IDelegate.OnRemoveShape(int index)
            {
                _parent._dlc.Modify();
                _parent.ShapeRemoveEvent(index);
            }

            void HistoryCanvas.IDelegate.OnMoveShape(int index)
            {
                _parent._dlc.Modify();
                _parent.ShapeModifyEvent(index);
            }
        }

        private readonly IDelegate _delegate;
        private readonly Canvas _canvas;
        private readonly History _history;
        private readonly HistoryCanvas _historyCanvas;
        private readonly DocumentLifecycleController _dlc;

        public Document(IDelegate delegate_, Canvas canvas)
        {
            _delegate = delegate_;
            _canvas = canvas;
            _history = new History();
            _historyCanvas = new HistoryCanvas(_canvas, new HistoryCanvasDelegate(this));
            _dlc = new DocumentLifecycleController(new DocumentLifecycleControllerDelegate(this));
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            _historyCanvas.AddShape(type, rect);
        }

        public IShape GetShape(int index)
        {
            return new Shape(this, index);
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
        }

        public void Redo()
        {
            _history.Redo();
        }

        public delegate void AddShapeDelegate(Common.ShapeType type, Common.Rectangle boundingRect);
        public delegate void AddShapesDelegate(AddShapeDelegate delegate_);

        public void ReplaceCanvasData(AddShapesDelegate delegate_)
        {
            delegate_((Common.ShapeType type, Common.Rectangle boundingRect) => {
                _canvas.InsertShape(_canvas.ShapeCount, new Common.Shape(type, boundingRect));
            });
            CompleteLayoutUpdateEvent();
        }

        public delegate void VoidDelegate();
        public event VoidDelegate CompleteLayoutUpdateEvent;

        public delegate void IndexDelegate(int index);
        public event IndexDelegate ShapeInsertEvent;
        public event IndexDelegate ShapeModifyEvent;
        public event IndexDelegate ShapeRemoveEvent;
    }
}
