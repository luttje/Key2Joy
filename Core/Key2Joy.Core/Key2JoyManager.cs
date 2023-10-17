using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CommonServiceLocator;
using Key2Joy.Config;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Interop;
using Key2Joy.Interop.Commands;
using Key2Joy.LowLevelInput.GamePad;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Actions.Logic;
using Key2Joy.Mapping.Triggers.Keyboard;
using Key2Joy.Mapping.Triggers.Mouse;
using Key2Joy.Plugins;
using Key2Joy.Util;

namespace Key2Joy;

public delegate bool AppCommandRunner(AppCommand command);

public class Key2JoyManager : IKey2JoyManager, IMessageFilter
{
    /// <summary>
    /// Directory where plugins are located
    /// </summary>
    public const string PluginsDirectory = "Plugins";

    public event EventHandler<StatusChangedEventArgs> StatusChanged;

    public static Key2JoyManager instance;

    public static Key2JoyManager Instance
    {
        get
        {
            if (instance == null)
            {
                throw new Exception("Key2JoyManager not initialized using InitSafely yet!");
            }

            return instance;
        }
    }

    /// <summary>
    /// Trigger listeners that should explicitly loaded. This ensures that they're available for scripts
    /// even if no mapping option is mapped to be triggered by it.
    /// </summary>
    public IList<AbstractTriggerListener> ExplicitTriggerListeners { get; set; }

    private const string READY_MESSAGE = "Key2Joy is ready";
    private static AppCommandRunner commandRunner;
    private MappingProfile armedProfile;
    private IHaveHandleAndInvoke handleAndInvoker;
    private readonly List<IWndProcHandler> wndProcListeners = new();

    private Key2JoyManager()
    { }

    /// <summary>
    /// Ensures Key2Joy is running and ready to accept commands as long as the main loop does not end.
    /// </summary>
    public static void InitSafely(AppCommandRunner commandRunner, Action<PluginSet> mainLoop)
    {
        // Setup dependency injection and services
        var serviceLocator = new DependencyServiceLocator();
        ServiceLocator.SetLocatorProvider(() => serviceLocator);

        instance = new Key2JoyManager
        {
            ExplicitTriggerListeners = new List<AbstractTriggerListener>()
            {
                // Always add these listeners so scripts can ask them if stuff has happened.
                KeyboardTriggerListener.Instance,
                MouseButtonTriggerListener.Instance,
                MouseMoveTriggerListener.Instance
            }
        };
        serviceLocator.Register<IKey2JoyManager>(instance);

        var configManager = new ConfigManager();
        serviceLocator.Register<IConfigManager>(configManager);

        var gamePadService = new SimulatedGamePadService();
        serviceLocator.Register<IGamePadService>(gamePadService);

        var commandRepository = new CommandRepository();
        serviceLocator.Register<ICommandRepository>(commandRepository);

        // Load plugins
        var pluginDirectoriesPaths = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        pluginDirectoriesPaths = Path.Combine(pluginDirectoriesPaths, PluginsDirectory);

        PluginSet plugins = new(pluginDirectoriesPaths);
        plugins.LoadAll();
        plugins.RefreshPluginTypes();

        foreach (var loadState in plugins.AllPluginLoadStates.Values)
        {
            if (loadState.LoadState == PluginLoadStates.FailedToLoad)
            {
                System.Windows.MessageBox.Show(
                    $"One of your plugins located at {loadState.AssemblyPath} failed to load. This was the error: " +
                    loadState.LoadErrorMessage,
                    "Failed to load plugin!",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning
                );
            }
        }

        Key2JoyManager.commandRunner = commandRunner;

        var interopServer = new InteropServer(instance, commandRepository);

        try
        {
            interopServer.RestartListening();
            mainLoop(plugins);
        }
        finally
        {
            interopServer.StopListening();
            gamePadService.ShutDown();
        }
    }

    // Run the event on the same thread as the main control/form
    public void CallOnUiThread(Action action) => this.handleAndInvoker.Invoke(action);

    internal static bool RunAppCommand(AppCommand command) => commandRunner != null && commandRunner(command);

    public bool PreFilterMessage(ref System.Windows.Forms.Message m)
    {
        for (var i = 0; i < this.wndProcListeners.Count; i++)
        {
            // Check if the proc listeners haven't changed (this can happen when a plugin opens a MessageBox, the user aborts, and we then close the messagebox)
            if (i >= this.wndProcListeners.Count)
            {
                Debug.WriteLine("Key2JoyManager.PreFilterMessage: wndProcListeners changed while processing message!");
                break;
            }

            var wndProcListener = this.wndProcListeners[i];

            wndProcListener.WndProc(new Contracts.Mapping.Message(m.HWnd, m.Msg, m.WParam, m.LParam));
        }

        return false;
    }

    public void SetHandlerWithInvoke(IHaveHandleAndInvoke handleAndInvoker)
    {
        this.handleAndInvoker = handleAndInvoker;
        Application.AddMessageFilter(this);

        Console.WriteLine(READY_MESSAGE);
    }

    public bool GetIsArmed(MappingProfile profile = null)
    {
        if (profile == null)
        {
            return this.armedProfile != null;
        }

        return this.armedProfile == profile;
    }

    public void ArmMappings(MappingProfile profile)
    {
        this.armedProfile = profile;

        var allListeners = new List<AbstractTriggerListener>();
        allListeners.AddRange(this.ExplicitTriggerListeners);

        var allActions = (IList<AbstractAction>)profile.MappedOptions.Select(m => m.Action).ToList();

        foreach (var mappedOption in profile.MappedOptions)
        {
            if (mappedOption.Trigger == null)
            {
                continue;
            }

            var listener = mappedOption.Trigger.GetTriggerListener();

            if (!allListeners.Contains(listener))
            {
                allListeners.Add(listener);
            }

            if (listener is IWndProcHandler listenerWndProcHAndler)
            {
                this.wndProcListeners.Add(listenerWndProcHAndler);
            }

            mappedOption.Action.OnStartListening(listener, ref allActions);
            listener.AddMappedOption(mappedOption);
        }

        var allListenersForSharing = (IList<AbstractTriggerListener>)allListeners;

        foreach (var listener in allListeners)
        {
            if (listener is IWndProcHandler listenerWndProcHAndler)
            {
                listenerWndProcHAndler.Handle = this.handleAndInvoker.Handle;
            }

            listener.StartListening(ref allListenersForSharing);
        }

        StatusChanged?.Invoke(this, new StatusChangedEventArgs
        {
            IsEnabled = true,
            Profile = this.armedProfile
        });
    }

    public void DisarmMappings()
    {
        var listeners = this.ExplicitTriggerListeners;
        this.wndProcListeners.Clear();

        // Clear all intervals
        IdPool.CancelAll();

        foreach (var mappedOption in this.armedProfile.MappedOptions)
        {
            if (mappedOption.Trigger == null)
            {
                continue;
            }

            var listener = mappedOption.Trigger.GetTriggerListener();
            mappedOption.Action.OnStopListening(listener);

            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        foreach (var listener in listeners)
        {
            listener.StopListening();
        }

        var gamePadService = ServiceLocator.Current.GetInstance<IGamePadService>();
        gamePadService.EnsureAllUnplugged();

        this.armedProfile = null;

        StatusChanged?.Invoke(this, new StatusChangedEventArgs
        {
            IsEnabled = false,
        });
    }

    /// <summary>
    /// Starts Key2Joy, pausing until it's ready
    /// </summary>
    public static void StartKey2Joy(bool startMinimized = true, bool pauseUntilReady = true)
    {
        var configManager = ServiceLocator.Current.GetInstance<IConfigManager>();
        var executablePath = configManager.GetConfigState().LastInstallPath;

        if (executablePath == null)
        {
            Console.WriteLine("Error! Key2Joy executable path is not known, please start Key2Joy at least once!");
            return;
        }

        if (!File.Exists(executablePath))
        {
            Console.WriteLine("Error! Key2Joy executable path is invalid, please start Key2Joy at least once (and don't move the executable)!");
            return;
        }

        Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                Arguments = startMinimized ? "--minimized" : "",
                UseShellExecute = false,
                RedirectStandardOutput = true
            }
        };

        process.Start();

        if (!pauseUntilReady)
        {
            return;
        }

        while (!process.StandardOutput.EndOfStream)
        {
            if (process.StandardOutput.ReadLine() == READY_MESSAGE)
            {
                break;
            }
        }
    }
}
