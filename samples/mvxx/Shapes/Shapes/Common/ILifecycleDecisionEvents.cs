using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;

namespace Shapes.Common
{
    public interface ILifecycleDecisionEvents
    {
        Option<string> RequestOpenPath();
        ClosingAction RequestUnsavedClosing();
        Option<string> RequestSavePath();
    }
}
