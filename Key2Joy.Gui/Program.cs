using Key2Joy.Mapping;
using Key2Joy.Plugins;
using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace Key2Joy.Gui
{
    public static class Program
    {
        public static Form ActiveForm { get; set; }
        public static PluginSet Plugins { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Key2JoyManager.InitSafely(
                OnRunAppCommand,
                (plugins) =>
                {
                    string[] args = Environment.GetCommandLineArgs();

                    Plugins = plugins;

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    var shouldStartMinimized = false;

                    foreach (string arg in args)
                    {
                        if (arg == "--minimized")
                        {
                            shouldStartMinimized = true;
                        }
                    }

                    ActiveForm = new InitForm(shouldStartMinimized);

                    while (ActiveForm != null && !ActiveForm.IsDisposed)
                        Application.Run(ActiveForm);
                });
        }

        internal static void GoToNextForm(Form form)
        {
            var oldForm = ActiveForm;
            ActiveForm = form;

            oldForm.Close();
        }

        internal static Bitmap ResourceBitmapFromName(string name)
        {
            ResourceManager rm = Properties.Resources.ResourceManager;
            return (Bitmap)rm.GetObject(name);
        }

        private static bool OnRunAppCommand(AppCommand command)
        {
            if (ActiveForm is IAcceptAppCommands form)
            {
                return form.RunAppCommand(command);
            }

            return false;
        }
    }
}
