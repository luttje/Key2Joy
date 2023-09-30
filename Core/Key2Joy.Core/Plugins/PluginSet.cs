using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Security;
using System.Security.Policy;
using Key2Joy.Contracts.Plugins;
using Mono.Cecil;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.PluginSandbox;
using System.Runtime.InteropServices;

namespace Key2Joy.Plugins
{
    public class PluginSet
    {
        private readonly List<AbstractPlugin> loadedPlugins = new();
        public IReadOnlyList<AbstractPlugin> LoadedPlugins => loadedPlugins;

        private readonly List<PluginLoadState> pluginLoadStates = new();
        public IReadOnlyList<PluginLoadState> AllPluginLoadStates => pluginLoadStates;

        private readonly string[] pluginDirectoriesPaths;
        public IReadOnlyList<string> PluginAssemblyPaths => pluginDirectoriesPaths;

        /**
         * Plugin customizations
         */
        private readonly List<MappingTypeFactory<AbstractAction>> actionFactories = new();
        public IReadOnlyList<MappingTypeFactory<AbstractAction>> ActionFactories => actionFactories;

        private readonly List<MappingTypeFactory<AbstractTrigger>> triggerFactories = new();
        public IReadOnlyList<MappingTypeFactory<AbstractTrigger>> TriggerFactories => triggerFactories;

        private readonly List<MappingControlFactory> mappingControlFactories = new();
        public IReadOnlyList<MappingControlFactory> MappingControlFactories => mappingControlFactories;


        public string PluginsFolder { get; private set; }

        /// <summary>
        /// Loads plugins from the specified directory
        /// </summary>
        /// <param name="pluginsPath">The path to the directory containing the plugins</param>
        /// <returns></returns>
        internal PluginSet(string pluginDirectoriesPaths)
        {
            PluginsFolder = pluginDirectoriesPaths;
            this.pluginDirectoriesPaths = Directory.GetDirectories(pluginDirectoriesPaths);

            Start();
        }

        private void Start()
        {
            foreach (var pluginDirectoryPath in pluginDirectoriesPaths)
            {
                var pluginsAssemblyName = Path.GetFileName(pluginDirectoryPath);
                var pluginAssemblyFileName = $"{pluginsAssemblyName}.dll";
                var pluginAssemblyPath = Path.Combine(pluginDirectoryPath, pluginAssemblyFileName);

                var sandboxDomainSetup = new AppDomainSetup
                {
                    ApplicationBase = pluginDirectoryPath,
                };

                var evidence = new Evidence();
                evidence.AddHostEvidence(new Zone(SecurityZone.Internet));

                var permissions = SecurityManager.GetStandardSandbox(evidence);
                
                // Required to instantiate Controls inside the plugin
                permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, pluginDirectoryPath));
                permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, pluginDirectoryPath));

                // For some reason needed by ActionOptionsChangeListener.
                // TODO: What kind of reflection does this allow? Internal only? Or will it give plugins unsafe access?
                permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
                
                // Allow showing a plugin form (also required SecurityZone.MyComputer above
                permissions.AddPermission(new UIPermission(UIPermissionWindow.AllWindows));

                // Allow writing to the plugin directory
                var pluginDataDirectory = Path.Combine(pluginDirectoryPath, "data");
                Directory.CreateDirectory(pluginDataDirectory);
                permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Write, pluginDataDirectory));

                var pluginSandboxerType = typeof(PluginSandboxer);
                
                // We want the sandboxer assembly's strong name, so that we can add it to the full trust list.
                // Helpful: https://github.com/ghuntley/docs-1/blob/master/docs/framework/misc/how-to-run-partially-trusted-code-in-a-sandbox.md
                var fullTrustAssembly = pluginSandboxerType.Assembly.Evidence.GetHostEvidence<StrongName>();
                var sandboxDomain = AppDomain.CreateDomain("Sandbox", null, sandboxDomainSetup, permissions, fullTrustAssembly);

                try
                {
                    var pluginLoadState = new PluginLoadState(pluginAssemblyPath);
                    var pluginAssembly = AssemblyDefinition.ReadAssembly(pluginAssemblyPath);

                    if (!FindPluginEntryType(pluginAssembly, out var pluginEntryTypeReference)) 
                    {
                        AddFailedLoad(pluginAssemblyPath, "Did not find a plugin entrypoint. Please implement IPlugin in your plugin assembly.");
                        continue;
                    }

                    var manager = (PluginSandboxer)Activator.CreateInstanceFrom(
                        sandboxDomain,
                        pluginSandboxerType.Assembly.ManifestModule.FullyQualifiedName,
                        pluginSandboxerType.FullName).Unwrap();
                    var plugin = manager.LoadPlugin(
                        pluginsAssemblyName, 
                        pluginEntryTypeReference, 
                        pluginDataDirectory
                    );

                    pluginLoadState.Plugin = plugin;
                    pluginLoadState.LoadState = PluginLoadStates.Loaded;

                    pluginLoadStates.Add(pluginLoadState);
                    loadedPlugins.Add(plugin);

                    foreach (var actionFullTypeName in plugin.ActionFullTypeNames)
                    {
                        var actionType = pluginAssembly.MainModule.GetType(actionFullTypeName);

                        var cecilMappingAttribute = actionType.CustomAttributes.Single(ca => ca.AttributeType.Name == nameof(ActionAttribute));
                        var actionAttribute = BuildMappingAttribute<ActionAttribute>(cecilMappingAttribute);

                        // Iterate all methods on the type and check where they have the ExposesScriptingMethodAttribute attribute
                        var exposedMethods = new List<ExposedMethod>();

                        foreach (var method in actionType.Methods)
                        {
                            var methodAttributes = method.CustomAttributes.Where(ca => ca.AttributeType.Name == nameof(ExposesScriptingMethodAttribute));
                            foreach (var methodAttribute in methodAttributes)
                            {
                                var exposedMethod = new AppDomainExposedMethod(
                                    sandboxDomain,
                                    pluginAssemblyPath,
                                    actionType.FullName,
                                    methodAttribute.ConstructorArguments[0].Value as string,
                                    method.Name);

                                exposedMethods.Add(exposedMethod);
                            }
                        }

                        actionFactories.Add(
                            new AppDomainMappingTypeFactory<AbstractAction>(
                                sandboxDomain,
                                pluginAssemblyPath,
                                typeof(AbstractAction),
                                actionFullTypeName,
                                actionAttribute,
                                exposedMethods
                            )
                        );
                    }

                    foreach (var triggerFullTypeName in plugin.TriggerFullTypeNames)
                    {
                        var triggerType = pluginAssembly.MainModule.GetType(triggerFullTypeName);

                        var cecilMappingAttribute = triggerType.CustomAttributes.Single(ca => ca.AttributeType.Name == nameof(TriggerAttribute));
                        var triggerAttribute = BuildMappingAttribute<TriggerAttribute>(cecilMappingAttribute);

                        triggerFactories.Add(
                            new AppDomainMappingTypeFactory<AbstractTrigger>(
                                sandboxDomain,
                                pluginAssemblyPath,
                                typeof(AbstractTrigger),
                                triggerFullTypeName,
                                triggerAttribute
                            )
                        );
                    }

                    foreach (var type in pluginAssembly.MainModule.Types)
                    {
                        var cecilMappingControlAttribute = type.CustomAttributes.SingleOrDefault(ca => ca.AttributeType.Name == nameof(MappingControlAttribute));

                        if (cecilMappingControlAttribute == null)
                        {
                            continue;
                        }

                        string imageResourceName = "error"; // Default value in MappingControlAttribute should make this irrelevant
                        var forTypes = new List<string>();

                        foreach (var property in cecilMappingControlAttribute.Properties)
                        {
                            if (property.Name == nameof(MappingControlAttribute.ImageResourceName))
                            {
                                imageResourceName = (string)property.Argument.Value;
                            }
                            else if (property.Name == nameof(MappingControlAttribute.ForTypes))
                            {
                                foreach (var forType in property.Argument.Value as CustomAttributeArgument[])
                                {
                                    forTypes.Add(((TypeDefinition)forType.Value).FullName);
                                }
                            }
                            else if (property.Name == nameof(MappingControlAttribute.ForType))
                            {
                                forTypes.Add(((TypeDefinition)property.Argument.Value).FullName);
                            }
                        }

                        foreach (var forType in forTypes)
                        {
                            var mappingControlFactory = new AppDomainMappingControlFactory(
                                sandboxDomain,
                                pluginAssemblyPath,
                                type.FullName,
                                forType,
                                imageResourceName
                            );

                            mappingControlFactories.Add(mappingControlFactory);
                        }
                    }
                }
                catch (Exception e)
                {
                    AddFailedLoad(pluginAssemblyPath, e.Message);
                    MessageBox.Show(
                        $"One of your plugins located at {pluginAssemblyPath} failed to load. This was the error: " +
                        e.Message,
                        "Failed to load plugin " + pluginsAssemblyName, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning
                    );
                }
            }
        }

        private bool FindPluginEntryType(AssemblyDefinition pluginAssembly, out string typeReference)
        {
            foreach (TypeDefinition td in pluginAssembly.MainModule.GetTypes())
            {
                if (td.BaseType != null && td.BaseType.FullName == typeof(AbstractPlugin).FullName)
                {
                    typeReference = td.FullName;
                    return true;
                }
            }

            typeReference = null;
            return false;
        }
        
        internal void AddFailedLoad(string assemblyPath, string errorMessage)
        {
            pluginLoadStates.Add(new PluginLoadState(assemblyPath)
            {
                LoadState = PluginLoadStates.FailedToLoad,
                LoadErrorMessage = errorMessage
            });
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
    }
}
