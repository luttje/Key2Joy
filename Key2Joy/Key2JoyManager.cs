using Esprima.Ast;
using FFMpegCore;
using Key2Joy.Interop;
using Key2Joy.LowLevelInput;
using Key2Joy.Mapping;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy
{
    public delegate bool AppCommandRunner(AppCommand command);
        
    public class Key2JoyManager : IMessageFilter
    {
        private static AppCommandRunner commandRunner;
        private MappingPreset armedPreset;
        private Form mainForm;
        private List<TriggerListener> wndProcListeners = new List<TriggerListener>();

        public static Key2JoyManager instance;
        public static Key2JoyManager Instance
        {
            get
            {
                if (instance == null)
                    throw new Exception("Key2JoyManager not initialized using InitSafely yet!");
                
                return instance;
            }
        }

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        private Key2JoyManager()
        { }

        /// <summary>
        /// Ensures Key2Joy is running and ready to accept commands as long as the main loop does not end.
        /// </summary>
        public static void InitSafely(AppCommandRunner commandRunner, Action mainLoop)
        {
            instance = new Key2JoyManager();
            
            Key2JoyManager.commandRunner = commandRunner;

            GlobalFFOptions.Configure(options => options.BinaryFolder = "./ffmpeg");

            try
            {
                InteropServer.Instance.RestartListening();
                mainLoop();
            }
            finally
            {
                InteropServer.Instance.StopListening();
                SimGamePad.Instance.ShutDown();
            }
        }

        // Run the event on the same thread as the main form
        internal void CallOnUiThread(Action action)
        {
            mainForm.Invoke(action);
        }

        private static List<TriggerListener> GetScriptingListeners()
        {
            var listeners = new List<TriggerListener>
            {
                // Always add these listeners so scripts can ask them if stuff has happened.
                KeyboardTriggerListener.Instance,
                MouseButtonTriggerListener.Instance,
                MouseMoveTriggerListener.Instance
            };

            return listeners;
        }

        internal static bool RunAppCommand(AppCommand command)
        {
            return commandRunner(command);
        }

        public bool PreFilterMessage(ref Message m)
        {
            foreach (var wndProcListener in wndProcListeners)
                wndProcListener.WndProc(ref m);

            return false;
        }

        public void SetMainForm(Form form)
        {
            this.mainForm = form;
            Application.AddMessageFilter(this);
        }

        public bool GetIsArmed(MappingPreset preset = null)
        {
            if (preset == null)
                return armedPreset != null;
            
            return armedPreset == preset;
        }

        public void ArmMappings(MappingPreset preset)
        {
            armedPreset = preset;
                
            var allListeners = GetScriptingListeners();
            var allActions = preset.MappedOptions.Select(m => m.Action).ToList();

            foreach (var mappedOption in preset.MappedOptions)
            {
                if (mappedOption.Trigger == null)
                    continue;

                var listener = mappedOption.Trigger.GetTriggerListener();

                if (!allListeners.Contains(listener))
                    allListeners.Add(listener);

                if (listener.HasWndProcHandle)
                    wndProcListeners.Add(listener);

                mappedOption.Action.OnStartListening(listener, ref allActions);
                listener.AddMappedOption(mappedOption);
            }

            foreach (var listener in allListeners)
            {
                listener.Handle = mainForm.Handle;
                listener.StartListening(ref allListeners);
            }

            StatusChanged?.Invoke(this, new StatusChangedEventArgs
            {
                IsEnabled = true,
                Preset = armedPreset
            });
        }

        public void DisarmMappings()
        {
            var listeners = GetScriptingListeners();
            wndProcListeners.Clear();

            // Clear all intervals
            IdPool.CancelAll();

            foreach (var mappedOption in armedPreset.MappedOptions)
            {
                if (mappedOption.Trigger == null)
                    continue;

                var listener = mappedOption.Trigger.GetTriggerListener();
                mappedOption.Action.OnStopListening(listener);

                if (!listeners.Contains(listener))
                    listeners.Add(listener);
            }

            foreach (var listener in listeners)
                listener.StopListening();

            GamePadManager.Instance.EnsureAllUnplugged();
            armedPreset = null;

            StatusChanged?.Invoke(this, new StatusChangedEventArgs
            {
                IsEnabled = false,
            });
        }
    }
}
