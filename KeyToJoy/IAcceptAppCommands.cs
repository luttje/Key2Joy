using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy
{
    internal interface IAcceptAppCommands
    {
        bool RunAppCommand(string command);
    }
}
