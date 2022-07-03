using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy
{
    internal interface IAcceptAppCommands
    {
        bool RunAppCommand(string command);
    }
}
