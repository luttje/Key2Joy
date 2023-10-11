using System;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Contracts.Plugins.Remoting;

namespace Key2Joy.PluginHost;

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
        if (string.IsNullOrEmpty(assemblyPath))
        {
            throw new ArgumentNullException("assemblyPath");
        }

        if (string.IsNullOrEmpty(assemblyName))
        {
            throw new ArgumentNullException("assembly");
        }

        var pluginTypeName = $"{assemblyName}.Plugin";

        this.pluginAssemblyPath = assemblyPath;
        this.pluginAssemblyName = assemblyName;

        Console.WriteLine("Loading plugin {0},{1}", assemblyName, pluginTypeName);

        // Let plugins specify additional permissions
        var permissionsXml = GetAdditionalPermissionsXml(this.pluginAssemblyPath);
        loadedChecksum = CalculatePermissionsChecksum(permissionsXml);

        if (expectedChecksum != null && loadedChecksum != expectedChecksum)
        {
            throw new PluginLoadException($"Plugin permissions checksum mismatch! \n\nExpected '{expectedChecksum}',  got '{loadedChecksum}'.\n\nThis plugin will be disabled for you. If you trust it you can re-enable it later.");
        }

        var pluginDirectory = Path.GetDirectoryName(assemblyPath);

        var pluginDataDirectory = Path.Combine(pluginDirectory, "data");
        Directory.CreateDirectory(pluginDataDirectory);

        AppDomainSetup sandboxDomainSetup = new()
        {
            ApplicationBase = pluginDirectory,
            PrivateBinPath = pluginDirectory,
        };

        Evidence evidence = new();
        evidence.AddHostEvidence(new Zone(SecurityZone.Internet));

        var permissions = SecurityManager.GetStandardSandbox(evidence);
        bool isUnrestricted = false;

        if (permissionsXml != null)
        {
            PermissionSet additionalPermissions = new(PermissionState.None);
            additionalPermissions.FromXml(SecurityElement.FromString(permissionsXml));

            if (additionalPermissions.Count > 0)
            {
                var securityOverride = additionalPermissions.GetPermission(typeof(SecurityOverride));

                if (securityOverride != null)
                {
                    isUnrestricted = ((SecurityOverride)securityOverride).IsUnrestricted;
                }
                else
                {
                    var intersection = additionalPermissions.Intersect(GetAllowedPermissionsWithDescriptions().AllowedPermissions);

                    if (intersection == null || !intersection.Equals(additionalPermissions))
                    {
                        throw new PluginLoadException($"Some plugin permissions are not allowed: {additionalPermissions}");
                    }

                    permissions = permissions.Union(additionalPermissions);
                }
            }
        }

        if (isUnrestricted)
        {
            permissions = new PermissionSet(PermissionState.Unrestricted);
        }
        else
        {
            // Required to instantiate Controls inside the plugin
            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, pluginDirectory));
            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, pluginDirectory));
            permissions.AddPermission(new UIPermission(UIPermissionWindow.AllWindows));
            permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
            // Needed to serialize objects back to the host app
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.SerializationFormatter));

            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, pluginDataDirectory));
        }

        this.sandboxDomain = AppDomain.CreateDomain("Sandbox", evidence, sandboxDomainSetup, permissions);

        var pluginAssemblyResolver = (PluginAssemblyResolver)this.sandboxDomain.CreateInstanceAndUnwrap(
            typeof(PluginAssemblyResolver).Assembly.FullName,
            typeof(PluginAssemblyResolver).FullName);
        pluginAssemblyResolver.SetPluginDirectory(pluginDirectory);

        this.sandboxDomain.AssemblyResolve += pluginAssemblyResolver.ResolveAssembly;

        this.loadedPlugin = (PluginBase)this.sandboxDomain.CreateInstanceAndUnwrap(assemblyName, pluginTypeName);
        this.loadedPlugin.PluginDirectory = pluginDirectory;
        this.loadedPlugin.PluginDataDirectory = pluginDataDirectory;
        this.loadedPlugin.Initialize();
    }

    public string GetPluginName() => this.loadedPlugin.Name;

    public string GetPluginAuthor() => this.loadedPlugin.Author;

    public string GetPluginWebsite() => this.loadedPlugin.Website;

    public static AllowedPermissionsWithDescriptions GetAllowedPermissionsWithDescriptions()
    {
        List<string> descriptions = new();
        PermissionSet allowedPermissions = new(PermissionState.None);

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

        allowedPermissions.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));  // Wildcards are not valid for this permission
        descriptions.Add("unrestricted access to load external assemblies (potentially dangerous)"); // Needed for the test runner and Assembly.LoadFrom (for plugins that want to use external libraries like FFmpeg)

        // Custom override permission to grant unrestricted access
        allowedPermissions.AddPermission(new SecurityOverride(PermissionState.Unrestricted));
        descriptions.Add("unrestricted (potentially dangerous)");

        // Note: https://github.com/microsoft/referencesource/tree/master/mscorlib/system/security/permissions
        //allowedPermissions.AddPermission(new RegistryPermission(RegistryPermissionAccess.Read, "*")); // Wildcards are not valid for this permission
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
        StringBuilder builder = new();
        for (var i = 0; i < hash.Length; i++)
        {
            builder.Append(hash[i].ToString("X2"));
        }
        return builder.ToString();
    }

    public PluginActionInsulator CreateAction(string fullTypeName, object[] constructorArguments)
    {
        var pluginAction = (PluginAction)this.sandboxDomain.CreateInstanceFromAndUnwrap(
            this.pluginAssemblyPath,
            fullTypeName,
            false,
            BindingFlags.Default,
            null,
            constructorArguments,
            null,
            null
        );
        pluginAction.Plugin = this.loadedPlugin;
        return new PluginActionInsulator(pluginAction);
    }

    public PluginTriggerInsulator CreateTrigger(string fullTypeName, object[] constructorArguments)
    {
        var pluginTrigger = (PluginTrigger)this.sandboxDomain.CreateInstanceFromAndUnwrap(
            this.pluginAssemblyPath,
            fullTypeName,
            false,
            BindingFlags.Default,
            null,
            constructorArguments,
            null,
            null
        );
        pluginTrigger.Plugin = this.loadedPlugin;
        return new PluginTriggerInsulator(pluginTrigger);
    }

    public void Test_AnyEvent(object sender, RemoteEventArgs e) => AnyEvent?.Invoke(sender, e);

    public INativeHandleContract CreateFrameworkElementContract(string controlTypeName, SubscriptionTicket[] eventSubscriptions = null)
    {
        Dictionary<string, RemoteEventHandler> eventHandlers = new();

        if (eventSubscriptions != null)
        {
            foreach (var subscription in eventSubscriptions)
            {
                eventHandlers.Add(subscription.EventName, new RemoteEventHandler(subscription, this.Test_AnyEvent));
            }
        }

        var contract = (INativeHandleContract)Program.AppDispatcher.Invoke(CreateOnUiThread, this.pluginAssemblyName, controlTypeName, this.sandboxDomain, eventHandlers);
        return contract;
    }

    private static NativeHandleContractInsulator CreateOnUiThread(string assembly, string typeName, AppDomain appDomain, Dictionary<string, RemoteEventHandler> eventHandlers)
    {
        try
        {
            var controlHandle = appDomain.CreateInstance(assembly, typeName) ?? throw new InvalidOperationException("appDomain.CreateInstance() returned null for " + assembly + "," + typeName);
            var converterHandle = appDomain.CreateInstanceAndUnwrap(
                typeof(ViewContractConverter).Assembly.FullName,
                typeof(ViewContractConverter).FullName) as ViewContractConverter ?? throw new InvalidOperationException("appDomain.CreateInstance() returned null for ViewContractConverter");
            var contract = converterHandle.ConvertToContract(controlHandle, eventHandlers);
            NativeHandleContractInsulator insulator = new(contract, converterHandle);

            return insulator;
        }
        catch (Exception ex)
        {
            var message = string.Format("Error loading type '{0}' from assembly '{1}'. {2}",
                assembly, typeName, ex.Message);

            Output.WriteLine(message);
            throw new ApplicationException(message, ex);
        }
    }

    public void Terminate() => Environment.Exit(0);
}
