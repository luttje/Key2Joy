using Key2Joy.Mapping;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Gui
{
    public static class Program
    {        
        public static Form ActiveForm { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Key2JoyManager.InitSafely(OnRunAppCommand, () =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                ActiveForm = new InitForm();

                while (ActiveForm != null && !ActiveForm.IsDisposed)
                {
                    Application.Run(ActiveForm);
                }
            });
        }

        internal static void GoToNextForm(Form form)
        {
            var oldForm = ActiveForm;
            ActiveForm = form;

            oldForm.Close();
        }

        private static bool OnRunAppCommand(AppCommand command)
        {
            if(ActiveForm is IAcceptAppCommands form)
            {
                return form.RunAppCommand(command);
            }

            return false;
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
