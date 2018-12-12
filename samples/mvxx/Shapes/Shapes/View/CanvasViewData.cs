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

        public VoidDelegate CreateNewDocumentEvent;
        public VoidDelegate OpenDocumentEvent;
        public VoidDelegate SaveDocumentEvent;
        public VoidDelegate SaveAsDocumentEvent;

        public VoidDelegate AddRectangleEvent;
        public VoidDelegate AddTriangleEvent;
        public VoidDelegate AddCircleEvent;
        public VoidDelegate RemoveShapeEvent;

        public VoidDelegate UndoEvent;
        public VoidDelegate RedoEvent;

        public delegate void MouseDelegate(Common.Position pos);
        public MouseDelegate MouseDownEvent;
        public MouseDelegate MouseUpEvent;
        public MouseDelegate MouseMoveEvent;

        public delegate Option<string> RequestDocumentPathDelegate();
        public RequestDocumentPathDelegate ShowOpenFileDialogEvent;
        public RequestDocumentPathDelegate ShowSaveFileDialogEvent;

        public delegate DomainModel.DocumentLifecycleController.ClosingAction RequestUnsavedDocumentClosingDelegate();
        public RequestUnsavedDocumentClosingDelegate ShowUnsavedDocumentClosePrompt;

        public delegate bool BoolDelegate();
        public BoolDelegate FormClosingEvent;
    }
}
