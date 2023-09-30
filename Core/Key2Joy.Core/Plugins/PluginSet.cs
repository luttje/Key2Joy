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
using Key2Joy.Config;
using System.Configuration.Assemblies;
using System.Security.Cryptography;
using System.Text;

namespace Key2Joy.Plugins
{
    public class PluginSet
    {
        private readonly List<AbstractPlugin> loadedPlugins = new();
        public IReadOnlyList<AbstractPlugin> LoadedPlugins => loadedPlugins;

        private readonly Dictionary<string, PluginLoadState> pluginLoadStates = new();
        public IReadOnlyDictionary<string, PluginLoadState> AllPluginLoadStates => pluginLoadStates;

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
            var enabledPlugins = ConfigManager.Config.EnabledPlugins;

            foreach (var pluginDirectoryPath in pluginDirectoriesPaths)
            {
                var pluginAssemblyName = Path.GetFileName(pluginDirectoryPath);
                var pluginAssemblyFileName = $"{pluginAssemblyName}.dll";
                var pluginAssemblyPath = Path.Combine(pluginDirectoryPath, pluginAssemblyFileName);
                var permissionsXml = GetAdditionalPermissionsXml(pluginAssemblyPath);
                var permissionsChecksum = CalculatePermissionsChecksum(permissionsXml);

                if (enabledPlugins.ContainsKey(pluginAssemblyPath) && enabledPlugins[pluginAssemblyPath] == permissionsChecksum)
                {
                    EnablePlugin(pluginAssemblyPath);
                }
                else if (enabledPlugins.ContainsKey(pluginAssemblyPath))
                {
                    // If the checksum didn't match add an error plugin state
                    AddPluginState(
                        PluginLoadStates.FailedToLoad,
                        pluginAssemblyPath,
                        "Plugin disabled for security reasons. Permissions checksum is incorrect."
                    );
                }
                else
                {
                    AddPluginState(
                        PluginLoadStates.NotLoaded,
                        pluginAssemblyPath,
                        "Plugin disabled. Enable it if you trust the author."
                    );
                }
            }
        }

        public static string GetAdditionalPermissionsXml(string pluginAssemblyPath)
        {
            var pluginDirectoryPath = Path.GetDirectoryName(pluginAssemblyPath);
            var permissionsFilePath = Path.Combine(pluginDirectoryPath, "permissions.xml");

            if (File.Exists(permissionsFilePath))
            {
                var permissionsFile = File.ReadAllText(permissionsFilePath);

                return permissionsFile;
            }

            return null;
        }

        public static string CalculatePermissionsChecksum(string permissionsXml)
        {
            var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(permissionsXml ?? string.Empty));
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }
            return builder.ToString();
        }

        public void EnablePlugin(string pluginAssemblyPath)
        {
            var pluginDirectoryPath = Path.GetDirectoryName(pluginAssemblyPath);
            var pluginAssemblyName = Path.GetFileName(pluginAssemblyPath).Replace(".dll", "");
            var pluginLoadState = new PluginLoadState(pluginAssemblyPath);

            try
            {
                var sandboxDomainSetup = new AppDomainSetup
                {
                    ApplicationBase = pluginDirectoryPath,
                };

                var evidence = new Evidence();
                evidence.AddHostEvidence(new Zone(SecurityZone.Internet));

                var permissions = SecurityManager.GetStandardSandbox(evidence);

                // Let plugins specify additional permissions
                var permissionsXml = GetAdditionalPermissionsXml(pluginAssemblyPath);
                var permissionsChecksum = CalculatePermissionsChecksum(permissionsXml);

                ConfigManager.Instance.SetPluginEnabled(pluginAssemblyPath, permissionsChecksum);

                if (permissionsXml != null)
                {
                    var additionalPermissions = new PermissionSet(PermissionState.None);
                    additionalPermissions.FromXml(SecurityElement.FromString(permissionsXml));

                    if (additionalPermissions.Count > 0) 
                    { 
                        var intersection = additionalPermissions.Intersect(GetAllowedPermissionsWithDescriptions().AllowedPermissions);

                        if (!intersection.Equals(additionalPermissions))
                        {
                            throw new SecurityException("Plugin permissions are not allowed.");
                        }

                        permissions = permissions.Union(additionalPermissions);
                    }
                }

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

                var pluginAssembly = AssemblyDefinition.ReadAssembly(pluginAssemblyPath);

                if (!FindPluginEntryType(pluginAssembly, out var pluginEntryTypeReference))
                {
                    AddPluginState(
                        PluginLoadStates.FailedToLoad,
                        pluginAssemblyPath, 
                        "Did not find a plugin entrypoint. Please implement IPlugin in your plugin assembly."
                    );
                    return;
                }

                var manager = (PluginSandboxer)Activator.CreateInstanceFrom(
                    sandboxDomain,
                    pluginSandboxerType.Assembly.ManifestModule.FullyQualifiedName,
                    pluginSandboxerType.FullName).Unwrap();
                var plugin = manager.LoadPlugin(
                    pluginAssemblyName,
                    pluginEntryTypeReference,
                    pluginDataDirectory
                );

                AddPluginState(PluginLoadStates.Loaded, pluginAssemblyPath, null, plugin);

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
                AddPluginState(
                    PluginLoadStates.FailedToLoad,
                    pluginAssemblyPath, 
                    e.Message
                );
            }
        }

        public void DisablePlugin(string pluginAssemblyPath)
        {
            ConfigManager.Instance.SetPluginEnabled(pluginAssemblyPath, null);
            MessageBox.Show("When disabling loaded plugins you have to restart the application for these changes to take effect.");
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
        
        internal void AddPluginState(PluginLoadStates state, string pluginAssemblyPath, string errorMessage, AbstractPlugin loadedPlugin = null)
        {
            if (state == PluginLoadStates.FailedToLoad)
            {
                MessageBox.Show(
                    $"One of your plugins located at {pluginAssemblyPath} failed to load. This was the error: " +
                    errorMessage,
                    "Failed to load plugin!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            
            if (pluginLoadStates.TryGetValue(pluginAssemblyPath, out var loadState))
            {
                loadState.LoadState = state;
                loadState.Plugin = loadedPlugin;
                loadState.LoadErrorMessage = errorMessage ?? string.Empty;
                return;
            }

            pluginLoadStates.Add(
                pluginAssemblyPath,
                new PluginLoadState(pluginAssemblyPath)
                {
                    LoadState = state,
                    Plugin = loadedPlugin,
                    LoadErrorMessage = errorMessage
                }
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

        public struct AllowedPermissionsWithDescriptions
        {
            public PermissionSet AllowedPermissions;
            public List<string> Descriptions;
        }

        public static AllowedPermissionsWithDescriptions GetAllowedPermissionsWithDescriptions()
        {
            var descriptions = new List<string>();
            var allowedPermissions = new PermissionSet(PermissionState.None);

            allowedPermissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
            descriptions.Add("unrestricted file access anywhere on your device");

            allowedPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, ""));
            descriptions.Add("file reading access anywhere on your device");

            allowedPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Write, ""));
            descriptions.Add("file writing access anywhere on your device");

            allowedPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Append, ""));
            descriptions.Add("file appending access anywhere on your device");
            
            allowedPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, ""));
            descriptions.Add("file and folder path discovery access anywhere on your device");
            
            allowedPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, ""));
            descriptions.Add("file full access anywhere on your device");

            // Note: https://github.com/microsoft/referencesource/tree/master/mscorlib/system/security/permissions
            //allowedPermissions.AddPermission(new RegistryPermission(RegistryPermissionAccess.Read, "*")); // Wildcards are not valid for this permission
            //descriptions.Add(...)
            //allowedPermissions.AddPermission(new EnvironmentPermission(EnvironmentPermissionAccess.Read, "*"));  // Wildcards are not valid for this permission
            //descriptions.Add(...)

            return new AllowedPermissionsWithDescriptions
            {
                AllowedPermissions = allowedPermissions,
                Descriptions = descriptions,
            };
        }
    }
}
