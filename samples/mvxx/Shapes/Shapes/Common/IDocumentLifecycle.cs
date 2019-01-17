using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Common
{
    public interface IDocumentLifecycle
    {
        bool New();
        bool Open();
        bool Save();
        bool SaveAs();
    }
}
