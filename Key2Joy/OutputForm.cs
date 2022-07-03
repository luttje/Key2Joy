using Key2Joy.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy
{
    public partial class OutputForm : Form
    {
        private static int PREFIX_LENGTH = "[##:##:##]".Length;
        
        public OutputForm()
        {
            InitializeComponent();
        }

        private void DrawLine(string logLine)
        {
            rtbOutput.AppendText(logLine.Substring(0, PREFIX_LENGTH), Color.Red);
            rtbOutput.AppendText(logLine.Substring(PREFIX_LENGTH));

            if(chkAutoScroll.Checked)
                ScrollToBottom();
        }
        
        private void ScrollToBottom()
        {
            rtbOutput.SelectionStart = rtbOutput.Text.Length;
            rtbOutput.ScrollToCaret();
        }

        private void OutputForm_Load(object sender, EventArgs e)
        {
            foreach (var line in File.ReadLines(Output.GetLogPath()))
                DrawLine(line + Environment.NewLine);

            Output.OnNewLogLine += Output_OnNewLogLine;
        }

        private void OutputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Output.OnNewLogLine -= Output_OnNewLogLine;
        }
        
        private void Output_OnNewLogLine(string logLine)
        {
            DrawLine(logLine);
        }
    }
}
