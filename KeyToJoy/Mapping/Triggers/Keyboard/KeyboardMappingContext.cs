using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy
{
    public partial class MappingContext : INodesContext
    {
        [Node(
            menu: "Keyboard",
            name: "A",
            description: "Triggers when the 'A' key is pressed or released",
            isExecutionInitiator: true)]
        public void KeyboardPressA(out bool isDown)
        {
            isDown = true;
        }
        
        [Node(
            menu: "Keyboard",
            name: "B",
            description: "Triggers when the 'B' key is pressed or released",
            isExecutionInitiator: true)]
        public void KeyboardPressB(out bool isDown)
        {
            isDown = true;
        }
    }
}
