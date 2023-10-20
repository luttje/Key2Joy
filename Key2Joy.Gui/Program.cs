using System;
using System.Drawing;
using System.Windows.Forms;
using Key2Joy.Mapping.Actions.Logic;
using Key2Joy.Plugins;

namespace Key2Joy.Gui;

public static class Program
{
    public static Form ActiveForm { get; set; }
    public static PluginSet Plugins { get; private set; }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Key2JoyManager.InitSafely(
            OnRunAppCommand,
            (plugins) =>
            {
                var args = Environment.GetCommandLineArgs();

                Plugins = plugins;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var shouldStartMinimized = false;

                foreach (var arg in args)
                {
                    if (arg == "--minimized")
                    {
                        shouldStartMinimized = true;
                    }
                }

                ActiveForm = new InitForm(shouldStartMinimized);

                while (ActiveForm != null && !ActiveForm.IsDisposed)
                {
                    Application.Run(ActiveForm);
                }
            }
        );

        Plugins.Dispose();
    }

    internal static void GoToNextForm(Form form)
    {
        var oldForm = ActiveForm;
        ActiveForm = form;

        oldForm.Close();
    }

    internal static Bitmap ResourceBitmapFromName(string name)
    {
        var rm = Properties.Resources.ResourceManager;
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
