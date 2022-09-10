using FFMpegCore;
using Key2Joy.Mapping;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy
{
    public static class Program
    {
        const string APP_DIR = "Key2Joy";
        
        public static Form ActiveForm { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GlobalFFOptions.Configure(options => options.BinaryFolder = "./ffmpeg");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                ActiveForm = new InitForm();

                while (ActiveForm != null && !ActiveForm.IsDisposed)
                {
                    Application.Run(ActiveForm);
                }
            }
            finally
            {
                SimGamePad.Instance.ShutDown();
            }
        }

        internal static void GoToNextForm(Form form)
        {
            var oldForm = ActiveForm;
            ActiveForm = form;

            oldForm.Close();
        }

        internal static bool RunAppCommand(AppCommand command)
        {
            if(ActiveForm is IAcceptAppCommands form)
            {
                return form.RunAppCommand(command);
            }

            return false;
        }

        internal static string GetAppDirectory()
        {
            var directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                APP_DIR);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return directory;
        }

        internal static List<TriggerListener> GetScriptingListeners()
        {
            var listeners = new List<TriggerListener>();
            
            // Always add these listeners so they keep track of triggers. That way scripts can ask them if stuff has happened.
            listeners.Add(KeyboardTriggerListener.Instance);
            listeners.Add(MouseButtonTriggerListener.Instance);
            listeners.Add(MouseMoveTriggerListener.Instance);

            return listeners;
        }
    }
}
