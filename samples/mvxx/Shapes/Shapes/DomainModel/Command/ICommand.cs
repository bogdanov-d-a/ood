﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel.Command
{
    interface ICommand
    {
        void Execute();
        void Unexecute();
    }
}
