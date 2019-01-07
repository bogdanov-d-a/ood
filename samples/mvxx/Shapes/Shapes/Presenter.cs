using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes
{
    public class Presenter
    {
        public class ModelDelegate : DomainModel.Facade.IDelegate
        {
            private readonly Presenter _parent;

            public ModelDelegate(Presenter parent)
            {
                _parent = parent;
            }

            public Option<string> RequestDocumentOpenPath()
            {
                return _parent._view.ViewHandlers.ShowOpenFileDialog();
            }

            public Option<string> RequestDocumentSavePath()
            {
                return _parent._view.ViewHandlers.ShowSaveFileDialog();
            }

            public Common.ClosingAction RequestUnsavedDocumentClosing()
            {
                return _parent._view.ViewHandlers.ShowUnsavedDocumentClosePrompt();
            }
        }

        private class ViewCommands : View.CanvasView.IViewCommands
        {
            private readonly Presenter _parent;

            public ViewCommands(Presenter parent)
            {
                _parent = parent;
            }

            public void AddCircle()
            {
                _parent._appModel.AddCircle();
            }

            public void AddRectangle()
            {
                _parent._appModel.AddRectangle();
            }

            public void AddTriangle()
            {
                _parent._appModel.AddTriangle();
            }

            public void CreateNewDocument()
            {
                _parent._appModel.New();
            }

            public bool FormClosing()
            {
                return _parent._appModel.New();
            }

            public Common.Size GetCanvasSize()
            {
                return _parent._appModel.CanvasSize;
            }

            public void MouseDown(Common.Position pos)
            {
                _parent._appModel.BeginMove(pos);
            }

            public void MouseMove(Common.Position pos)
            {
                _parent._appModel.Move(pos);
            }

            public void MouseUp(Common.Position pos)
            {
                _parent._appModel.EndMove(pos);
            }

            public void OpenDocument()
            {
                _parent._appModel.Open();
            }

            public void Redo()
            {
                _parent._appModel.Redo();
            }

            public void RemoveShape()
            {
                _parent._appModel.RemoveSelectedShape();
            }

            public void SaveAsDocument()
            {
                _parent._appModel.SaveAs();
            }

            public void SaveDocument()
            {
                _parent._appModel.Save();
            }

            public void Undo()
            {
                _parent._appModel.Undo();
            }
        }

        private readonly AppModel.Facade _appModel;
        private readonly View.CanvasView _view;

        public Presenter(AppModel.Facade appModel, View.CanvasView view)
        {
            _appModel = appModel;
            _appModel.SetDelegate(new ModelDelegate(this));

            _view = view;
            _view.ViewCommands = new ViewCommands(this);

            Initialize();
        }

        private void Initialize()
        {
            _appModel.CompleteLayoutUpdateEvent += new AppModel.Facade.VoidDelegate(() => {
                _view.SetSelectionIndex(-1);

                while (_view.ShapeCount > 0)
                {
                    _view.RemoveShape(0);
                }

                for (int i = 0; i < _appModel.ShapeCount; ++i)
                {
                    _view.AddShape(i, _appModel.GetShape(i));
                }

                _view.SetSelectionIndex(_appModel.GetSelectedIndex());

                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.ShapeInsertEvent += new AppModel.Facade.IndexDelegate((int index) => {
                _view.AddShape(index, _appModel.GetShape(index));
                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.ShapeModifyEvent += new AppModel.Facade.IndexDelegate((int index) => {
                _view.GetShape(index).boundingRect = _appModel.GetShape(index).boundingRect;
                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.ShapeRemoveEvent += new AppModel.Facade.IndexDelegate((int index) => {
                _view.RemoveShape(index);
                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.SelectionChangeEvent += new AppModel.Facade.IndexDelegate((int index) => {
                _view.SetSelectionIndex(index);
                _view.ViewHandlers.InvalidateLayout();
            });
        }
    }
}
