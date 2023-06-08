using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy
{
    public interface IAcceptAppCommands
    {
        bool RunAppCommand(AppCommand command);
    }
}
