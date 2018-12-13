using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.View
{
    public class CanvasViewData
    {
        public Option<Common.Size> CanvasSize
        {
            get; set;
        }

        public delegate void VoidDelegate();

        public delegate Option<string> RequestDocumentPathDelegate();
        public RequestDocumentPathDelegate ShowOpenFileDialogEvent;
        public RequestDocumentPathDelegate ShowSaveFileDialogEvent;

        public delegate DomainModel.DocumentLifecycleController.ClosingAction RequestUnsavedDocumentClosingDelegate();
        public RequestUnsavedDocumentClosingDelegate ShowUnsavedDocumentClosePrompt;
    }
}
