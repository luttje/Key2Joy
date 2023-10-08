﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public enum AppCommand
    {
        /// <summary>
        /// Aborts listening for triggers
        /// </summary>
        Abort = 0,

        /// <summary>
        /// Recreate the scripting environment (loses all variables, functions and other changes scripts made)
        /// </summary>
        ResetScriptEnvironment = 10,
    }
}