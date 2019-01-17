using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;

namespace Shapes.DomainModel
{
    public class Facade
    {
        private class DocumentLifecycleControllerEvents : DocumentLifecycleController.IEvents
        {
            private class LifecycleActionEventsHandler : DocumentLifecycleController.ILifecycleActionEvents
            {
                private readonly Facade _facade;

                public LifecycleActionEventsHandler(Facade facade)
                {
                    _facade = facade;
                }

                public void OnEraseDocument()
                {
                    _facade._canvas.RemoveAllShapes();
                    _facade._document.ClearHistory();
                    _facade.CompleteLayoutUpdateEvent();
                }

                public void OnExportDocumentData(string path)
                {
                    _facade.SerializeShapes(new Utils.ShapeToFileSerializer(path));
                }

                public void OnFillDocumentData(string path)
                {
                    _facade.LoadCanvas(new Utils.CanvasLoaderFromFile(path));
                }
            }

            private class SynchronizationEventsHandler : DocumentLifecycleController.ISynchronizationEvents
            {
                private readonly Facade _facade;

                public SynchronizationEventsHandler(Facade facade)
                {
                    _facade = facade;
                }

                public bool IsDocumentSynced()
                {
                    return _facade._savedCommand == _facade._document.GetLastExecutedCommand();
                }

                public void OnSyncDocument()
                {
                    _facade._savedCommand = _facade._document.GetLastExecutedCommand();
                }
            }

            private readonly Facade _facade;
            private readonly LifecycleActionEventsHandler _lifecycleActionEvents;
            private readonly SynchronizationEventsHandler _synchronizationEvents;

            public DocumentLifecycleControllerEvents(Facade facade)
            {
                _facade = facade;
                _lifecycleActionEvents = new LifecycleActionEventsHandler(facade);
                _synchronizationEvents = new SynchronizationEventsHandler(facade);
            }

            public Common.ILifecycleDecisionEvents LifecycleDecisionEvents
            {
                get => _facade._lifecycleDecisionEvents;
            }

            public DocumentLifecycleController.ILifecycleActionEvents LifecycleActionEvents
            {
                get => _lifecycleActionEvents;
            }

            public DocumentLifecycleController.ISynchronizationEvents SynchronizationEvents
            {
                get => _synchronizationEvents;
            }
        }

        private class DocumentLifecycleHandlers : Common.IDocumentLifecycle
        {
            private readonly DocumentLifecycleController _dlc;

            public DocumentLifecycleHandlers(DocumentLifecycleController dlc)
            {
                _dlc = dlc;
            }

            public bool New()
            {
                return _dlc.New();
            }

            public bool Open()
            {
                return _dlc.Open();
            }

            public bool Save()
            {
                return _dlc.Save(false);
            }

            public bool SaveAs()
            {
                return _dlc.Save(true);
            }
        }

        private readonly Canvas _canvas;
        private readonly Document _document;
        private readonly DocumentLifecycleHandlers _documentLifecycleHandlers;
        private Command.ICommand _savedCommand = null;
        private Common.ILifecycleDecisionEvents _lifecycleDecisionEvents = null;

        public Facade()
        {
            _canvas = new Canvas(new Common.Size(640, 480));
            _document = new Document(_canvas);
            _documentLifecycleHandlers = new DocumentLifecycleHandlers(
                new DocumentLifecycleController(new DocumentLifecycleControllerEvents(this)));

            _document.ShapeInsertEvent += (int index) => ShapeInsertEvent(index);
            _document.ShapeModifyEvent += (int index) => ShapeModifyEvent(index);
            _document.ShapeRemoveEvent += (int index) => ShapeRemoveEvent(index);
        }

        public void SetLifecycleDecisionEvents(Common.ILifecycleDecisionEvents lifecycleDecisionEvents)
        {
            _lifecycleDecisionEvents = lifecycleDecisionEvents;
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
            get => _canvas.CanvasSize;
        }

        public int ShapeCount
        {
            get => _canvas.ShapeCount;
        }

        public Common.IUndoRedo History
        {
            get => _document.History;
        }

        public Common.IDocumentLifecycle DocumentLifecycle
        {
            get => _documentLifecycleHandlers;
        }

        private void LoadCanvas(Utils.ICanvasLoader loader)
        {
            loader.LoadShapes((Common.ShapeType type, Common.Rectangle boundingRect) => {
                _canvas.InsertShape(_canvas.ShapeCount, new Common.Shape(type, boundingRect));
            });
            CompleteLayoutUpdateEvent();
        }

        private void SerializeShapes(Utils.IShapeSerializer serializer)
        {
            serializer.SerializeShapes((Utils.ShapeInfoDelegate shapeInfoDelegate) => {
                for (int i = 0; i < ShapeCount; ++i)
                {
                    var shape = GetShape(i);
                    shapeInfoDelegate(shape.ShapeType, shape.BoundingRect);
                }
            });
        }

        public event Common.DelegateTypes.Void CompleteLayoutUpdateEvent;

        public event Common.DelegateTypes.Int ShapeInsertEvent;
        public event Common.DelegateTypes.Int ShapeModifyEvent;
        public event Common.DelegateTypes.Int ShapeRemoveEvent;
    }
}
