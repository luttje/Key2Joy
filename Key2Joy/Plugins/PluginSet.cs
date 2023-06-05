using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using Key2Joy.Contracts.Plugins;
using Mono.Cecil;
using System.Configuration.Assemblies;
using System.Linq.Expressions;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;
using SimWinInput;

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
                var pluginAssemblyPath = Path.Combine(pluginDirectoryPath, $"{pluginsAssemblyName}.dll");

                var sandboxDomainSetup = new AppDomainSetup
                {
                    ApplicationBase = pluginDirectoryPath,
                };

                var evidence = new Evidence();
                evidence.AddHostEvidence(new Zone(SecurityZone.Internet));

                var permissions = SecurityManager.GetStandardSandbox(evidence);
                permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, pluginDirectoryPath));
                permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, pluginDirectoryPath));
                // Allow writing to a sub directory
                var pluginDataDirectory = Path.Combine(pluginDirectoryPath, "data");
                permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Write, pluginDataDirectory));

                var sandboxDomain = AppDomain.CreateDomain("Sandbox", null, sandboxDomainSetup, permissions);

                try
                {
                    var pluginLoadState = new PluginLoadState(pluginAssemblyPath);
                    var pluginAssembly = AssemblyDefinition.ReadAssembly(pluginAssemblyPath);

                    if (!FindPluginEntrypoint(pluginAssembly, out var pluginEntryTypeReference)) 
                    {
                        AddFailedLoad(pluginAssemblyPath, "Did not find a plugin entrypoint. Please implement IPlugin in your plugin assembly.");
                        continue;
                    }
                    
                    var plugin = (AbstractPlugin)sandboxDomain.CreateInstanceFromAndUnwrap(pluginAssemblyPath, pluginEntryTypeReference.FullName);
                    plugin.pluginDataDirectory = pluginDataDirectory;

                    pluginLoadState.Plugin = plugin;
                    pluginLoadState.LoadState = PluginLoadStates.Loaded;

                    pluginLoadStates.Add(pluginLoadState);
                    loadedPlugins.Add(plugin);

                    // TODO: Clean up (similar as above) + unstable to maintain due to nameof if-else
                    foreach (var actionFullTypeName in plugin.ActionFullTypeNames)
                    {
                        var actionType = pluginAssembly.MainModule.GetType(actionFullTypeName);
                        
                        var cecilMappingAttribute = actionType.CustomAttributes.Single(ca => ca.AttributeType.Name == nameof(ActionAttribute));
                        var actionAttribute = new ActionAttribute();
                        foreach (var property in cecilMappingAttribute.Properties)
                        {
                            if (property.Name == nameof(ActionAttribute.Description))
                            {
                                actionAttribute.Description = (string)property.Argument.Value;
                            }
                            else if (property.Name == nameof(ActionAttribute.NameFormat))
                            {
                                actionAttribute.NameFormat = (string)property.Argument.Value;
                            }
                            else if (property.Name == nameof(ActionAttribute.Visibility))
                            {
                                actionAttribute.Visibility = (MappingMenuVisibility)property.Argument.Value;
                            }
                        }

                        // Iterate all methods on the type and check where they have the ExposesScriptingMethodAttribute attribute
                        var exposedMethods = new List<ExposedMethod>();

                        foreach (var method in actionType.Methods)
                        {
                            var methodAttributes = method.CustomAttributes.Where(ca => ca.AttributeType.Name == nameof(ExposesScriptingMethodAttribute));
                            foreach (var methodAttribute in methodAttributes)
                            {
                                var exposedMethod = new AppDomainExposedMethod(
                                    methodAttribute.ConstructorArguments[0].Value as string,
                                    method.Name,
                                    sandboxDomain);

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

                    // TODO: Clean up (similar as above) + unstable to maintain due to nameof if-else
                    foreach (var triggerFullTypeName in plugin.TriggerFullTypeNames)
                    {
                        var triggerType = pluginAssembly.MainModule.GetType(triggerFullTypeName);
                        
                        var cecilMappingAttribute = triggerType.CustomAttributes.Single(ca => ca.AttributeType.Name == nameof(TriggerAttribute));
                        var triggerAttribute = new TriggerAttribute();
                        foreach (var property in cecilMappingAttribute.Properties)
                        {
                            if (property.Name == nameof(TriggerAttribute.Description))
                            {
                                triggerAttribute.Description = (string)property.Argument.Value;
                            }
                            else if (property.Name == nameof(TriggerAttribute.NameFormat))
                            {
                                triggerAttribute.NameFormat = (string)property.Argument.Value;
                            }
                            else if (property.Name == nameof(TriggerAttribute.Visibility))
                            {
                                triggerAttribute.Visibility = (MappingMenuVisibility)property.Argument.Value;
                            }
                        }

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

                    //var action = (AbstractAction)sandboxDomain.CreateInstanceFromAndUnwrap(pluginAssemblyPath, plugin.ActionFullTypeNames[0]);
                    // To call async methods use:
                    // Source: https://stackoverflow.com/a/63824188
                    //Task.Run(() => action.Execute());
                    //var result = await Task.Run(()=>action.Execute(parameter));

                    plugin.OnLoaded();
                }
                catch (Exception e)
                {
                    AddFailedLoad(pluginAssemblyPath, e.Message);
                    MessageBox.Show(e.Message, "Failed to load plugin " + pluginAssemblyPath, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        
        private bool FindPluginEntrypoint(AssemblyDefinition pluginAssembly, out TypeReference typeReference)
        {
            foreach (TypeDefinition td in pluginAssembly.MainModule.GetTypes())
            {
                if (td.BaseType != null && td.BaseType.FullName == typeof(AbstractPlugin).FullName)
                {
                    typeReference = td;
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
    }
}
