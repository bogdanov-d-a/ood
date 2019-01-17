using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;

namespace Shapes.AppModel
{
    public class Facade
    {
        private class UndoRedoHandlers : Common.IUndoRedo
        {
            private readonly Facade _facade;

            public UndoRedoHandlers(Facade facade)
            {
                _facade = facade;
            }

            public void Redo()
            {
                _facade._appModel.ResetSelection();
                _facade._domainModel.History.Redo();
            }

            public void Undo()
            {
                _facade._appModel.ResetSelection();
                _facade._domainModel.History.Undo();
            }
        }

        private static readonly Common.Rectangle defRect = new Common.Rectangle(new Common.Position(200, 100), new Common.Size(300, 200));

        private readonly DomainModel.Facade _domainModel;
        private readonly AppModel _appModel;
        private readonly UndoRedoHandlers _undoRedoHandlers;

        public Facade()
        {
            _domainModel = new DomainModel.Facade();
            _appModel = new AppModel(_domainModel);
            _undoRedoHandlers = new UndoRedoHandlers(this);

            _domainModel.ShapeInsertEvent += ShapeInsertEvent;

            _appModel.CompleteLayoutUpdateEvent += CompleteLayoutUpdateEvent;
            _appModel.ShapeModifyEvent += ShapeModifyEvent;
            _appModel.ShapeRemoveEvent += ShapeRemoveEvent;
            _appModel.SelectionChangeEvent += SelectionChangeEvent;
        }

        public void SetLifecycleDecisionEvents(Common.ILifecycleDecisionEvents lifecycleDecisionEvents)
        {
            _domainModel.SetLifecycleDecisionEvents(lifecycleDecisionEvents);
        }

        private void AddShape(Common.ShapeType type)
        {
            _domainModel.AddShape(type, defRect);
        }

        public void AddRectangle()
        {
            AddShape(Common.ShapeType.Rectangle);
        }

        public void AddTriangle()
        {
            AddShape(Common.ShapeType.Triangle);
        }

        public void AddCircle()
        {
            AddShape(Common.ShapeType.Circle);
        }

        public Common.Shape GetShape(int index)
        {
            return _appModel.GetShape(index);
        }

        public int SelectedIndex
        {
            get => _appModel.SelectedIndex;
        }

        public void RemoveSelectedShape()
        {
            _appModel.RemoveSelectedShape();
        }

        public void BeginMove(Common.Position pos)
        {
            _appModel.BeginMove(pos);
        }

        public void Move(Common.Position pos)
        {
            _appModel.Move(pos);
        }

        public void EndMove(Common.Position pos)
        {
            _appModel.EndMove(pos);
        }

        public Common.IUndoRedo History
        {
            get => _undoRedoHandlers;
        }

        public Common.IDocumentLifecycle DocumentLifecycle
        {
            get => _domainModel.DocumentLifecycle;
        }

        public Common.Size CanvasSize
        {
            get => _domainModel.CanvasSize;
        }

        public int ShapeCount
        {
            get => _domainModel.ShapeCount;
        }

        public event Common.DelegateTypes.Void CompleteLayoutUpdateEvent;

        public event Common.DelegateTypes.Int ShapeInsertEvent;
        public event Common.DelegateTypes.Int ShapeModifyEvent;
        public event Common.DelegateTypes.Int ShapeRemoveEvent;
        public event Common.DelegateTypes.Int SelectionChangeEvent;
    }
}
