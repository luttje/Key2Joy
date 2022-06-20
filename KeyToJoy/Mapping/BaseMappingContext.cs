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
        // The node that is being actually executed.
        public NodeVisual CurrentProcessingNode { get; set; }

        // Implementation of interface member: event FeedbackInfo
        public event Action<string, NodeVisual, FeedbackType, object, bool> FeedbackInfo;

        // Node with one input (object obj)
        [Node(
            menu: "Debug",
            name: "Display Object", 
            description: "Allows to show any output in popup message box."
        )]
        public void ShowMessage(object anything)
        {
            MessageBox.Show(
                anything.ToString(), 
                "Nodes Debug: " + anything.GetType().Name, 
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
