using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy
{
    public partial class ComplexMappingForm : Form
    {
        public ComplexMappingForm()
        {
            InitializeComponent();

            //create a viewer object 
            var viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            viewer.ToolBarIsVisible = false;

            //create a graph object 
            var graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            graph.AddNode(new Microsoft.Msagl.Drawing.Node("Test")
            {
                DrawNodeDelegate = (node, g) =>
                {
                    var graphics = (Graphics)g;

                    var r = new Rectangle(
                        (int)(node.GeometryNode.Center.X - (node.GeometryNode.BoundingBox.Width * .5)),
                        (int)(node.GeometryNode.Center.Y - (node.GeometryNode.BoundingBox.Height * .5)),
                        (int)node.GeometryNode.BoundingBox.Width,
                        (int)node.GeometryNode.BoundingBox.Height);

                    graphics.SetClip(r);
                    graphics.FillEllipse(Brushes.Red, r);
                    graphics.DrawEllipse(Pens.Black, r);
                    graphics.ResetClip();

                    return true; // override default rendering
                }
            });
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
            graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
            
            var c = graph.FindNode("C");
            c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
            c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
            
            //bind the graph to the viewer 
            viewer.Graph = graph;
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            Controls.Add(viewer);
        }

        private void ComplexMappingForm_Load(object sender, EventArgs e)
        {
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
        }
    }
}
