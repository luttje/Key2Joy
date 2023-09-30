using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Gui.Mapping
{
    public partial class ActionPluginHostControl : UserControl, IActionOptionsControl
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public event EventHandler OptionsChanged;
        
        private IActionOptionsControl abstractPluginFormWithOptionsEvent;

        public ActionPluginHostControl()
        {
            InitializeComponent();
        }

        public ActionPluginHostControl(AbstractPluginForm abstractPluginForm)
            : this()
        {
            if (!(abstractPluginForm is IActionOptionsControl abstractPluginFormWithOptionsEvent))
            {
                throw new ArgumentException("The given plugin form does not implement IActionOptionsControl!");
            }
            
            this.abstractPluginFormWithOptionsEvent = abstractPluginFormWithOptionsEvent;

            Padding = new Padding(0, 5, 0, 5);
            Height = abstractPluginForm.DesiredHeight + this.Padding.Vertical;

            // We use this hack to embed the form which is actually in a different AppDomain (and thus supposed to be quite unreachable)
            abstractPluginForm.Show();
            SetParent(abstractPluginForm.Handle, this.Handle);

            // Give the form a moment to get to know the parent size
            abstractPluginForm.Visible = false;

            Task.Run(() =>
            {
                // Then maximize it, so it fills the parent
                Task.Delay(100).Wait();
                this.Invoke((MethodInvoker)delegate
                {
                    abstractPluginForm.WindowState = FormWindowState.Maximized;
                    abstractPluginForm.Visible = true;
                });
            });

            var listener = new ActionOptionsChangeListener(abstractPluginFormWithOptionsEvent);
            listener.OptionsChanged += (s, e) => OptionsChanged?.Invoke(s, e);
        }

        public bool CanMappingSave(AbstractAction action)
        {
            return abstractPluginFormWithOptionsEvent.CanMappingSave(action);
        }

        public void Select(AbstractAction action)
        {
            abstractPluginFormWithOptionsEvent.Select(action);
        }

        public void Setup(AbstractAction action)
        {
            abstractPluginFormWithOptionsEvent.Setup(action);
        }
    }
}
