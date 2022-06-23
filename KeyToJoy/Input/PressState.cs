using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Input
{
    public enum PressState
    {
        /// <summary>
        /// Full click/press (first down, then up)
        /// </summary>
        PressAndRelease,
        
        /// <summary>
        /// Pressed down
        /// </summary>
        Press,

        /// <summary>
        /// Release (after pressed down)
        /// </summary>
        Release,
    }
}
