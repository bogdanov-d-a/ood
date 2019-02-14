using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class DocumentKeeper
    {
        private class DocumentLifecycleControllerEvents : DocumentLifecycleController.IEvents
        {
            private class LifecycleActionEventsHandler : DocumentLifecycleController.ILifecycleActionEvents
            {
                private readonly DocumentKeeper _documentKeeper;

                public LifecycleActionEventsHandler(DocumentKeeper documentKeeper)
                {
                    _documentKeeper = documentKeeper;
                }

                public void OnEraseDocument()
                {
                    _documentKeeper.ResetData();
                    _documentKeeper.CompleteLayoutUpdateEvent();
                }

                public void OnExportDocumentData(string path)
                {
                    _documentKeeper.SerializeShapes(new Utils.ShapeToFileSerializer(path));
                }

                public void OnFillDocumentData(string path)
                {
                    _documentKeeper.LoadCanvas(new Utils.CanvasLoaderFromFile(path));
                }
            }

            private class SynchronizationEventsHandler : DocumentLifecycleController.ISynchronizationEvents
            {
                private readonly DocumentKeeper _documentKeeper;

                public SynchronizationEventsHandler(DocumentKeeper documentKeeper)
                {
                    _documentKeeper = documentKeeper;
                }

                public bool IsDocumentSynced()
                {
                    return _documentKeeper._savedCommand == _documentKeeper._document.GetLastExecutedCommand();
                }

                public void OnSyncDocument()
                {
                    _documentKeeper._savedCommand = _documentKeeper._document.GetLastExecutedCommand();
                }
            }

            private readonly DocumentKeeper _documentKeeper;
            private readonly LifecycleActionEventsHandler _lifecycleActionEvents;
            private readonly SynchronizationEventsHandler _synchronizationEvents;

            public DocumentLifecycleControllerEvents(DocumentKeeper documentKeeper)
            {
                _documentKeeper = documentKeeper;
                _lifecycleActionEvents = new LifecycleActionEventsHandler(documentKeeper);
                _synchronizationEvents = new SynchronizationEventsHandler(documentKeeper);
            }

            public Common.ILifecycleDecisionEvents LifecycleDecisionEvents
            {
                get => _documentKeeper._lifecycleDecisionEvents;
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

        private readonly DocumentLifecycleHandlers _documentLifecycleHandlers;
        private Canvas _canvas;
        private Document _document;
        private Command.ICommand _savedCommand = null;
        private Common.ILifecycleDecisionEvents _lifecycleDecisionEvents = null;

        public DocumentKeeper()
        {
            _documentLifecycleHandlers = new DocumentLifecycleHandlers(
                new DocumentLifecycleController(new DocumentLifecycleControllerEvents(this)));
        }

        public void SetLifecycleDecisionEvents(Common.ILifecycleDecisionEvents lifecycleDecisionEvents)
        {
            _lifecycleDecisionEvents = lifecycleDecisionEvents;
        }

        public void ResetData()
        {
            _canvas = new Canvas(new Common.Size(640, 480));
            _document = new Document(_canvas);

            _document.ShapeInsertEvent += (int index) => ShapeInsertEvent(index);
            _document.ShapeModifyEvent += (int index) => ShapeModifyEvent(index);
            _document.ShapeRemoveEvent += (int index) => ShapeRemoveEvent(index);
        }

        public Canvas Canvas
        {
            get => _canvas;
        }

        public Document Document
        {
            get => _document;
        }

        public Common.IDocumentLifecycle DocumentLifecycle
        {
            get => _documentLifecycleHandlers;
        }

        private void LoadCanvas(Utils.ICanvasLoader loader)
        {
            loader.LoadShapes((Common.ShapeType type, Common.Rectangle boundingRect) =>
                _canvas.InsertShape(_canvas.ShapeCount, new Common.Shape(type, boundingRect)));
            CompleteLayoutUpdateEvent();
        }

        private void SerializeShapes(Utils.IShapeSerializer serializer)
        {
            serializer.SerializeShapes((Utils.ShapeInfoDelegate shapeInfoDelegate) => {
                for (int i = 0; i < _canvas.ShapeCount; ++i)
                {
                    var shape = _canvas.GetShapeAt(i);
                    shapeInfoDelegate(shape.type, shape.boundingRect);
                }
            });
        }

        public event Common.DelegateTypes.Void CompleteLayoutUpdateEvent;

        public event Common.DelegateTypes.Int ShapeInsertEvent;
        public event Common.DelegateTypes.Int ShapeModifyEvent;
        public event Common.DelegateTypes.Int ShapeRemoveEvent;
    }
}
