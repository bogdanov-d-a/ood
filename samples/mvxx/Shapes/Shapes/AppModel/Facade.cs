using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;

namespace Shapes.AppModel
{
    public class Facade
    {
        private static readonly Common.Rectangle defRect = new Common.Rectangle(new Common.Position(200, 100), new Common.Size(300, 200));

        private readonly DomainModel.Facade _domainModel;
        private readonly AppModel _appModel;

        public Facade()
        {
            _domainModel = new DomainModel.Facade();
            _appModel = new AppModel(_domainModel);

            _domainModel.ShapeInsertEvent += new DomainModel.Facade.IndexDelegate((int index) => {
                ShapeInsertEvent(index);
            });

            _appModel.CompleteLayoutUpdateEvent += new AppModel.VoidDelegate(() => {
                CompleteLayoutUpdateEvent();
            });
            _appModel.ShapeModifyEvent += new AppModel.IndexDelegate((int index) => {
                ShapeModifyEvent(index);
            });
            _appModel.ShapeRemoveEvent += new AppModel.IndexDelegate((int index) => {
                ShapeRemoveEvent(index);
            });
            _appModel.SelectionChangeEvent += new AppModel.IndexDelegate((int index) => {
                SelectionChangeEvent(index);
            });
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

        public void Undo()
        {
            _appModel.ResetSelection();
            _domainModel.Undo();
        }

        public void Redo()
        {
            _appModel.ResetSelection();
            _domainModel.Redo();
        }

        public bool New()
        {
            return _domainModel.New();
        }

        public void Open()
        {
            _domainModel.Open();
        }

        public void Save()
        {
            _domainModel.Save();
        }

        public void SaveAs()
        {
            _domainModel.SaveAs();
        }

        public Common.Size CanvasSize
        {
            get => _domainModel.CanvasSize;
        }

        public int ShapeCount
        {
            get => _domainModel.ShapeCount;
        }

        public delegate void VoidDelegate();
        public event VoidDelegate CompleteLayoutUpdateEvent;

        public delegate void IndexDelegate(int index);
        public event IndexDelegate ShapeInsertEvent;
        public event IndexDelegate ShapeModifyEvent;
        public event IndexDelegate ShapeRemoveEvent;
        public event IndexDelegate SelectionChangeEvent;
    }
}
