using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;

namespace Shapes.DomainModel
{
    public class Facade
    {
        public interface IShape
        {
            Common.ShapeType GetShapeType();
            Common.Rectangle GetBoundingRect();
            void SetBoundingRect(Common.Rectangle rect);
        }

        public interface IDelegate
        {
            Option<string> RequestDocumentOpenPath();
            Option<string> RequestDocumentSavePath();
            Common.ClosingAction RequestUnsavedDocumentClosing();
        }

        private class DocumentLifecycleControllerDelegate : DocumentLifecycleController.IDelegate
        {
            private readonly Facade _facade;

            public DocumentLifecycleControllerDelegate(Facade facade)
            {
                _facade = facade;
            }

            public void OnEraseMemoryDocument()
            {
                _facade._canvas.RemoveAllShapes();
                _facade._document.ClearHistory();
                _facade.CompleteLayoutUpdateEvent();
            }

            public void OnOpenDocument(string path)
            {
                Utils.ICanvasLoader loader = new Utils.CanvasLoaderFromFile(path);
                loader.LoadShapes((Common.ShapeType type, Common.Rectangle boundingRect) => {
                    _facade._canvas.InsertShape(_facade._canvas.ShapeCount, new Common.Shape(type, boundingRect));
                });
                _facade.CompleteLayoutUpdateEvent();
            }

            public void OnSaveDocument(string path)
            {
                Utils.IShapeSerializer serializer = new Utils.ShapeToFileSerializer(path);
                serializer.SerializeShapes((Utils.ShapeInfoDelegate shapeInfoDelegate) => {
                    for (int i = 0; i < _facade.ShapeCount; ++i)
                    {
                        var shape = _facade.GetShape(i);
                        shapeInfoDelegate(shape.GetShapeType(), shape.GetBoundingRect());
                    }
                });
            }

            public Option<string> RequestDocumentOpenPath()
            {
                return _facade._delegate.RequestDocumentOpenPath();
            }

            public Option<string> RequestDocumentSavePath()
            {
                return _facade._delegate.RequestDocumentSavePath();
            }

            public Common.ClosingAction RequestUnsavedDocumentClosing()
            {
                return _facade._delegate.RequestUnsavedDocumentClosing();
            }
        }

        private readonly Canvas _canvas;
        private readonly Document _document;
        private readonly DocumentLifecycleController _dlc;
        private IDelegate _delegate = null;

        public Facade()
        {
            _canvas = new Canvas(new Common.Size(640, 480));
            _document = new Document(_canvas);
            _dlc = new DocumentLifecycleController(new DocumentLifecycleControllerDelegate(this));

            _document.ShapeInsertEvent += new Document.IndexDelegate((int index) => {
                _dlc.Modify();
                ShapeInsertEvent(index);
            });
            _document.ShapeModifyEvent += new Document.IndexDelegate((int index) => {
                _dlc.Modify();
                ShapeModifyEvent(index);
            });
            _document.ShapeRemoveEvent += new Document.IndexDelegate((int index) => {
                _dlc.Modify();
                ShapeRemoveEvent(index);
            });
        }

        public void SetDelegate(IDelegate delegate_)
        {
            _delegate = delegate_;
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            _document.AddShape(type, rect);
        }

        public IShape GetShape(int index)
        {
            return _document.GetShape(index);
        }

        public void RemoveShape(int index)
        {
            _document.RemoveShape(index);
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

        public void Undo()
        {
            _document.Undo();
        }

        public void Redo()
        {
            _document.Redo();
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

        public delegate void VoidDelegate();
        public event VoidDelegate CompleteLayoutUpdateEvent;

        public delegate void IndexDelegate(int index);
        public event IndexDelegate ShapeInsertEvent;
        public event IndexDelegate ShapeModifyEvent;
        public event IndexDelegate ShapeRemoveEvent;
    }
}
