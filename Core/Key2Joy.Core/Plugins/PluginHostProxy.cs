using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using Key2Joy.PluginHost;
using Mono.Cecil;
using System;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using System.Windows.Forms;

namespace Key2Joy.Plugins
{
    public class PluginHostProxy
    {
        public event EventHandler Disposing;
        public bool IsDisposing { get; internal set; }

        private string name;
        private EventWaitHandle readyEvent;
        private Process process;
        private IPluginHost pluginHost;
        private NamedPipeServerStream pipeServerStream;

        private string pluginAssemblyPath;
        private string pluginAssemblyName;

        /**
         * Plugin customizations
         */
        private readonly List<MappingTypeFactory<AbstractAction>> actionFactories = new();
        private readonly List<MappingTypeFactory<AbstractTrigger>> triggerFactories = new();
        private readonly List<MappingControlFactory> mappingControlFactories = new();

        public PluginHostProxy(string pluginAssemblyPath, string pluginAssemblyName)
        {
            this.pluginAssemblyPath = pluginAssemblyPath;
            this.pluginAssemblyName = pluginAssemblyName;
        }

        class IpcChannelRegistration
        {
            private static object _lock = new object();
            private static bool _registered;

            public static void RegisterChannel()
            {
                lock (_lock)
                {
                    if (_registered) return;
                    var channel = new IpcChannel();
                    ChannelServices.RegisterChannel(channel, false);
                    _registered = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadedChecksum"></param>
        /// <param name="expectedChecksum"></param>
        /// <exception cref="PluginLoadException"></exception>
        public void LoadPlugin(out string loadedChecksum, string expectedChecksum = null)
        {
            Start();
            OpenPluginHost();

            pluginHost.LoadPlugin(pluginAssemblyPath, pluginAssemblyName, out loadedChecksum, expectedChecksum);

            DiscoverPluginTypes();
        }

        private void DiscoverPluginTypes()
        {
            var pluginAssembly = AssemblyDefinition.ReadAssembly(pluginAssemblyPath);

            foreach (var type in pluginAssembly.MainModule.Types)
            {
                if (type.Namespace != pluginAssemblyName && !type.Namespace.StartsWith(pluginAssemblyName + "."))
                    continue;

                foreach (var customAttribute in type.CustomAttributes)
                {
                    if (customAttribute.AttributeType.Name == nameof(MappingControlAttribute))
                    {
                        DiscoverPluginType_MappingControl(type, customAttribute);
                    }
                    else if (customAttribute.AttributeType.Name == nameof(ActionAttribute))
                    {
                        DiscoverPluginType_Action(type, customAttribute);
                    }
                    else if (customAttribute.AttributeType.Name == nameof(TriggerAttribute))
                    {
                        DiscoverPluginType_Trigger(type, customAttribute);
                    }
                }
            }
        }

        private void DiscoverPluginType_MappingControl(TypeDefinition controlType, CustomAttribute customAttribute)
        {
            string imageResourceName = "error"; // Default value in MappingControlAttribute should make this irrelevant
            var controlTargetTypes = new List<string>();

            foreach (var property in customAttribute.Properties)
            {
                if (property.Name == nameof(MappingControlAttribute.ImageResourceName))
                {
                    imageResourceName = (string)property.Argument.Value;
                }
                else if (property.Name == nameof(MappingControlAttribute.ForTypes))
                {
                    foreach (var forType in property.Argument.Value as CustomAttributeArgument[])
                    {
                        controlTargetTypes.Add(((TypeDefinition)forType.Value).FullName);
                    }
                }
                else if (property.Name == nameof(MappingControlAttribute.ForType))
                {
                    controlTargetTypes.Add(((TypeDefinition)property.Argument.Value).FullName);
                }
            }

            foreach (var targetActionType in controlTargetTypes)
            {
                mappingControlFactories.Add(new PluginMappingControlFactory(
                    targetActionType,
                    imageResourceName,
                    this,
                    controlType.FullName
                ));
            }
        }

        private void DiscoverPluginType_Trigger(TypeDefinition type, CustomAttribute customAttribute)
        {
            var triggerAttribute = BuildMappingAttribute<TriggerAttribute>(customAttribute);

            triggerFactories.Add(
                new PluginMappingTypeFactory<AbstractTrigger>(
                    this,
                    typeof(AbstractTrigger),
                    type.FullName,
                    triggerAttribute
                )
            );
        }

        private void DiscoverPluginType_Action(TypeDefinition type, CustomAttribute customAttribute)
        {
            var actionAttribute = BuildMappingAttribute<ActionAttribute>(customAttribute);
            var exposedMethods = new List<ExposedMethod>();

            foreach (var method in type.Methods)
            {
                var methodAttributes = method.CustomAttributes.Where(ca => ca.AttributeType.Name == nameof(ExposesScriptingMethodAttribute));

                foreach (var methodAttribute in methodAttributes)
                {
                    var exposedMethod = new PluginExposedMethod(
                        this,
                        type.FullName,
                        methodAttribute.ConstructorArguments[0].Value as string,
                        method.Name);

                    exposedMethods.Add(exposedMethod);
                }
            }

            actionFactories.Add(
                new PluginMappingTypeFactory<AbstractAction>(
                    this,
                    typeof(AbstractAction),
                    type.FullName,
                    actionAttribute,
                    exposedMethods
                )
            );
        }

        public static T BuildMappingAttribute<T>(CustomAttribute attributeData) where T : new()
        {
            var attributeInstance = new T();

            foreach (var property in attributeData.Properties)
            {
                SetPropertyValue(attributeInstance, property.Name, property.Argument.Value);
            }

            return attributeInstance;
        }

        private static void SetPropertyValue(object obj, string propertyName, object value)
        {
            var property = obj.GetType().GetProperty(propertyName);

            if (property != null)
            {
                property.SetValue(obj, value);
            }
        }

        /// <summary>
        /// Asks the plugin to construct the WPF FrameworkElement. Then places it inside an ElementHost for use in WinForms.
        /// Can return null if the plugin crashes during creation.
        /// </summary>
        /// <param name="controlTypeName"></param>
        /// <returns></returns>
        internal Control CreateControl(string controlTypeName)
        {
            try
            {
                var subscribeOptionsChanged = RemoteEventSubscriber.SubscribeEvent("OptionsChanged", PluginHost_OptionsChanged);
                var subscribedEvents = new SubscriptionInfo[]
                {
                    subscribeOptionsChanged.Subscription,
                };
                var contract = (NativeHandleContractInsulator)pluginHost.CreateFrameworkElementContract(controlTypeName, subscribedEvents);
                var remoteControl = FrameworkElementAdapters.ContractToViewAdapter(contract);
                var control = new ElementHostProxy(remoteControl, contract);

                subscribeOptionsChanged.CustomSender = control;
                pluginHost.AnyEvent += PluginHost_AnyEvent;

                // We need to unsubscribe, otherwise the plugin view will freeze up.
                control.Disposed += (s, e) =>
                {
                    pluginHost.AnyEvent -= PluginHost_AnyEvent;
                    RemoteEventSubscriber.UnsubscribeEvent(subscribeOptionsChanged.Subscription.Id);
                };
                return control;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating plugin control!", $"Error creating plugin control: {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private static void PluginHost_AnyEvent(object sender, RemoteEventArgs e)
        {
            RemoteEventSubscriber.ClientInstance.AskServerToInvoke(e);
        }

        private static void PluginHost_OptionsChanged(object sender, RemoteEventArgs e)
        {
            var control = (ElementHostProxy)sender;

            if (control.InvokeRequired)
            {
                control.Invoke(new Action(control.InvokeOptionsChanged));
                return;
            }

            control.InvokeOptionsChanged();
        }

        /// <summary>
        /// Asks the plugin to construct the given type.
        /// </summary>
        /// <param name="fullTypeName"></param>
        /// <param name="constructorArguments"></param>
        /// <returns></returns>
        public T CreateAspectInstance<T>(string fullTypeName, object[] constructorArguments) where T : AbstractMappingAspect
        {
            var nameFormat = (string)(constructorArguments?[0] ?? fullTypeName);
            var type = typeof(T);

            // When loading all we get is AbstractMappingAspect and the full name
            // Look through the factories for one that matches the type, then set to that type
            if (type == typeof(AbstractMappingAspect))
            {
                if (actionFactories.Any(factory => factory.FullTypeName == fullTypeName))
                {
                    type = typeof(AbstractAction);
                }
                else if (triggerFactories.Any(factory => factory.FullTypeName == fullTypeName))
                {
                    type = typeof(AbstractTrigger);
                }
            }

            switch (type)
            {
                case Type actionType when actionType == typeof(AbstractAction):
                    var action = pluginHost.CreateAction(fullTypeName, new object[0]);
                    return new PluginActionProxy(nameFormat, action) as T;

                case Type actionType when actionType == typeof(AbstractTrigger):
                    var trigger = pluginHost.CreateTrigger(fullTypeName, new object[0]);
                    return new PluginTriggerProxy(nameFormat, trigger) as T;
            }

            throw new NotImplementedException($"Cannot create aspect of type {typeof(T).FullName}");
        }

        private static string GetTitle(string typeName)
        {
            int dot = typeName.IndexOf('.');
            if (dot < 0 || dot >= typeName.Length - 1) return typeName;
            return typeName.Substring(dot + 1);
        }

        private void Start()
        {
            if (process != null)
                return;

            name = "Key2Joy.PluginHost." + Guid.NewGuid().ToString();

            var eventName = $"{name}.Ready";
            readyEvent = new EventWaitHandle(false, EventResetMode.ManualReset, eventName);

            var processName = "Key2Joy.PluginHost.exe";

            var path = System.IO.Path.GetFullPath(processName);

            if (!System.IO.File.Exists(path))
            {
                System.Windows.Forms.MessageBox.Show("Key2Joy.PluginHost.exe not found at " + path);
            }

            SetupEventPipe();

            var info = new ProcessStartInfo
            {
                Arguments = name,
#if !DEBUG
                CreateNoWindow = true,
#endif
                UseShellExecute = false,
                FileName = processName
            };

            process = Process.Start(info);
        }

        private void SetupEventPipe()
        {
            // Expose a named pipe endpoint corresponding to the portName parameter for events
            // TODO: Clean up pipeServerStream when this proxy disposes
            pipeServerStream = new NamedPipeServerStream(
                RemotePipe.GetAbsolutePipeName(name),
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Message);

            // Start a new thread to listen for incoming connections on the named pipe
            var t = new Thread(() =>
            {
                pipeServerStream.WaitForConnection();
                Debug.WriteLine($"Pipe connection with plugin established");

                var reader = new StreamReader(pipeServerStream);

                while (true)
                {
                    try
                    {
                        var subscriptionId = RemotePipe.ReadMessage(pipeServerStream);
                        Debug.WriteLine($"ReadMessage: {subscriptionId}");
                        RemoteEventSubscriber.HandleInvoke(subscriptionId);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"-------------> Exception: {ex.Message}");
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        private void OpenPluginHost()
        {
            if (pluginHost != null) return;

            if (!readyEvent.WaitOne(5000))
            {
                throw new InvalidOperationException("PluginHost process not ready");
            }

            IpcChannelRegistration.RegisterChannel();

            var url = "ipc://" + name + "/" + nameof(PluginHost);
            pluginHost = (IPluginHost)Activator.GetObject(typeof(IPluginHost), url);

            var eventName = $"{name}.Exit";
            var exitEvent = new EventWaitHandle(false, EventResetMode.ManualReset, eventName);

            // Wait for the exit event in another thread in the background.
            Thread closeEventListeningThread = null;
            closeEventListeningThread = new Thread(() =>
            {
                while (true)
                {
                    exitEvent.WaitOne();

                    this?.Dispose();

                    // End and dispose this signal thread.
                    closeEventListeningThread.Abort();
                    closeEventListeningThread = null;
                }
            });
            closeEventListeningThread.IsBackground = true;
            closeEventListeningThread.Start();
        }

        public void Dispose()
        {
            IsDisposing = true;
            Disposing?.Invoke(this, EventArgs.Empty);

            pluginHost.Terminate();
            pluginHost = null;
            process = null;
        }

        internal string GetPluginName()
        {
            return pluginHost.GetPluginName();
        }

        internal string GetPluginAuthor()
        {
            return pluginHost.GetPluginAuthor();
        }

        internal string GetPluginWebsite()
        {
            return pluginHost.GetPluginWebsite();
        }

        internal IEnumerable<MappingTypeFactory<AbstractAction>> GetActionFactories()
        {
            return actionFactories;
        }

        internal IEnumerable<MappingTypeFactory<AbstractTrigger>> GetTriggerFactories()
        {
            return triggerFactories;
        }

        internal IEnumerable<MappingControlFactory> GetMappingControlFactories()
        {
            return mappingControlFactories;
        }
    }
}