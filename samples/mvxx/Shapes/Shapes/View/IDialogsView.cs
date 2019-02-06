using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;

namespace Shapes.View
{
    public interface IDialogsView
    {
        Option<string> ShowOpenFileDialog();
        Option<string> ShowSaveFileDialog();
        Common.ClosingAction ShowUnsavedDocumentClosePrompt();
    }
}
