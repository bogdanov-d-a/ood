using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;

namespace Shapes.Presenter
{
    public class LifecycleDecisionPresenter
    {
        private class LifecycleDecisionEventHandlers : Common.ILifecycleDecisionEvents
        {
            private readonly View.IDialogsView _view;

            public LifecycleDecisionEventHandlers(View.IDialogsView view)
            {
                _view = view;
            }

            public Option<string> RequestOpenPath()
            {
                return _view.ShowOpenFileDialog();
            }

            public Option<string> RequestSavePath()
            {
                return _view.ShowSaveFileDialog();
            }

            public Common.ClosingAction RequestUnsavedClosing()
            {
                return _view.ShowUnsavedDocumentClosePrompt();
            }
        }

        private LifecycleDecisionEventHandlers _handlers;

        public LifecycleDecisionPresenter(View.IDialogsView view)
        {
            _handlers = new LifecycleDecisionEventHandlers(view);
        }

        public Common.ILifecycleDecisionEvents EventHandlers
        {
            get => _handlers;
        }
    }
}
