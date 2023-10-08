using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.AddIn.Contract;
using System.Diagnostics;
using Key2Joy.Contracts.Plugins;
using System.Security.Cryptography;
using System.Reflection;
using Key2Joy.Contracts.Mapping;
using System.Windows;

namespace Key2Joy.PluginHost
{
    public class PluginHost : MarshalByRefObject, IPluginHost
    {
        public event RemoteEventHandlerCallback AnyEvent;
        
        private PluginBase loadedPlugin;
        private AppDomain sandboxDomain;
        private string pluginAssemblyPath;
        private string pluginAssemblyName;

        /// <summary>
        /// This method can't just return the PluginBase, since it's created in an AppDomain in the PluginHost process. Passing it 
        /// upwards to the main app would not work, since it has no remote connection to it. Therefor we store the plugin and let
        /// the main app call functions on this loader to interact with it.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="assemblyName"></param>
        /// <param name="loadedChecksum">The checksum after the plugin was loaded</param>
        /// <param name="expectedChecksum">The checksum the plugin must have to be loaded</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="PluginLoadException">Throws when the plugin failed to load</exception>
        public void LoadPlugin(string assemblyPath, string assemblyName, out string loadedChecksum, string expectedChecksum = null)
        {
            if (String.IsNullOrEmpty(assemblyPath)) throw new ArgumentNullException("assemblyPath");
            if (String.IsNullOrEmpty(assemblyName)) throw new ArgumentNullException("assembly");

            var pluginTypeName = $"{assemblyName}.Plugin";

            pluginAssemblyPath = assemblyPath;
            pluginAssemblyName = assemblyName;

            Console.WriteLine("Loading plugin {0},{1}", assemblyName, pluginTypeName);

            // Let plugins specify additional permissions
            var permissionsXml = GetAdditionalPermissionsXml(pluginAssemblyPath);
            loadedChecksum = CalculatePermissionsChecksum(permissionsXml);

            if (expectedChecksum != null && loadedChecksum != expectedChecksum)
            {
                throw new PluginLoadException($"Plugin permissions checksum mismatch. Expected '{expectedChecksum}',  got '{loadedChecksum}'");
            }

            var pluginDirectory = Path.GetDirectoryName(assemblyPath);
            var sandboxDomainSetup = new AppDomainSetup
            {
                ApplicationBase = pluginDirectory,
            };

            var evidence = new Evidence();
            evidence.AddHostEvidence(new Zone(SecurityZone.Internet));

            var permissions = SecurityManager.GetStandardSandbox(evidence);

            if (permissionsXml != null)
            {
                var additionalPermissions = new PermissionSet(PermissionState.None);
                additionalPermissions.FromXml(SecurityElement.FromString(permissionsXml));

                if (additionalPermissions.Count > 0)
                {
                    var intersection = additionalPermissions.Intersect(GetAllowedPermissionsWithDescriptions().AllowedPermissions);

                    if (!intersection.Equals(additionalPermissions))
                    {
                        throw new PluginLoadException($"Some plugin permissions are not allowed: {additionalPermissions}");
                    }

                    permissions = permissions.Union(additionalPermissions);
                }
            }

            // Required to instantiate Controls inside the plugin
            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, pluginDirectory));
            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, pluginDirectory));
            permissions.AddPermission(new UIPermission(UIPermissionWindow.AllWindows));
            permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
            // Needed to serialize objects back to the host app
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.SerializationFormatter));

            // Allow writing to the plugin directory
            var pluginDataDirectory = Path.Combine(pluginDirectory, "data");
            Directory.CreateDirectory(pluginDataDirectory);
            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Write, pluginDataDirectory));

            sandboxDomain = AppDomain.CreateDomain("Sandbox", evidence, sandboxDomainSetup, permissions);
            loadedPlugin = (PluginBase)sandboxDomain.CreateInstanceAndUnwrap(assemblyName, pluginTypeName);
        }

        public string GetPluginName()
        {
            return loadedPlugin.Name;
        }

        public string GetPluginAuthor()
        {
            return loadedPlugin.Author;
        }

        public string GetPluginWebsite()
        {
            return loadedPlugin.Website;
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
            var builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }
            return builder.ToString();
        }

        public PluginActionInsulator CreateAction(string fullTypeName, object[] constructorArguments)
        {
            return new PluginActionInsulator((PluginAction)sandboxDomain.CreateInstanceFromAndUnwrap(
                pluginAssemblyPath,
                fullTypeName,
                false,
                BindingFlags.Default,
                null,
                constructorArguments,
                null,
                null
            ));
        }

        public PluginTriggerInsulator CreateTrigger(string fullTypeName, object[] constructorArguments)
        {
            return new PluginTriggerInsulator((PluginTrigger)sandboxDomain.CreateInstanceFromAndUnwrap(
                pluginAssemblyPath,
                fullTypeName,
                false,
                BindingFlags.Default,
                null,
                constructorArguments,
                null,
                null
            ));
        }

        public void Test_AnyEvent(object sender, RemoteEventArgs e)
        {
            AnyEvent?.Invoke(sender, e);
        }

        public INativeHandleContract CreateFrameworkElementContract(string controlTypeName, SubscriptionInfo[] eventSubscriptions = null)
        {
            var eventHandlers = new Dictionary<string, RemoteEventHandler>();

            if (eventSubscriptions != null)
            {
                foreach (var subscription in eventSubscriptions)
                {
                    eventHandlers.Add(subscription.EventName, new RemoteEventHandler(subscription, Test_AnyEvent));
                }
            }

            var contract = (INativeHandleContract)Program.AppDispatcher.Invoke(CreateOnUiThread, pluginAssemblyName, controlTypeName, sandboxDomain, eventHandlers);
            return contract;
        }

        private static NativeHandleContractInsulator CreateOnUiThread(string assembly, string typeName, AppDomain appDomain, Dictionary<string, RemoteEventHandler> eventHandlers)
        {
            try
            {
                var controlHandle = appDomain.CreateInstance(assembly, typeName);
                if (controlHandle == null) 
                    throw new InvalidOperationException("appDomain.CreateInstance() returned null for " + assembly + "," + typeName);

                var converterHandle = appDomain.CreateInstanceAndUnwrap(
                    typeof(ViewContractConverter).Assembly.FullName,
                    typeof(ViewContractConverter).FullName) as ViewContractConverter;
                
                if (converterHandle == null) 
                    throw new InvalidOperationException("appDomain.CreateInstance() returned null for ViewContractConverter");

                var contract = converterHandle.ConvertToContract(controlHandle, eventHandlers);
                var insulator = new NativeHandleContractInsulator(contract, converterHandle);

                return insulator;
            }
            catch (Exception ex)
            {
                var message = String.Format("Error loading type '{0}' from assembly '{1}'. {2}",
                    assembly, typeName, ex.Message);

                throw new ApplicationException(message, ex);
            }
        }

        public void Terminate()
        {
            System.Environment.Exit(0);
        }
    }
}
