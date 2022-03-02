using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy
{
    internal static class Program
    {
        internal static Form NextForm { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NextForm = new InitForm();

            while(NextForm != null && !NextForm.IsDisposed)
            {
                Application.Run(NextForm);
            }
        }

        internal static void GoToNextForm(Form form)
        {
            var oldForm = NextForm;
            NextForm = form;

            oldForm.Close();
        }
    }
}
