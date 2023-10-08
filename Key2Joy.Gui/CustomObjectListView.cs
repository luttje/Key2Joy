using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Windows.Forms;
using BrightIdeasSoftware;
using BrightIdeasSoftware.Design;

namespace Key2Joy.Gui
{
    [Designer(typeof(ObjectListViewDesigner))]
    public class CustomObjectListView : ObjectListView
    {
        /// <summary>
        /// We need to override WndProc so ObjectListView doesn't fail upon receiving a message from a plugin MessageBox.
        /// To see what this fixes:
        /// 1. Remove the override below
        /// 2. Run the app
        /// 3. Map a Lua script that shows a MessageBox through a plugin (e.g: Hello.World("test")) to a button (e.g: D6)
        /// 4. Hold the mapped button (e.g: D6)
        /// 5. Arm mappings (check enabled checkbox)
        /// 6. Release. You will have gotten an exception inside ObjectListView by now (Use View All Threads) in the Call Stack viewer.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            try
            {
                base.WndProc(ref m);
            }
            catch (SecurityException)
            {
                Debug.WriteLine("SecurityException was thrown in CustomObjectListView.WndProc.");
            }
        }
    }
}
