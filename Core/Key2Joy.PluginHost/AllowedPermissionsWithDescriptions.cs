using System.Collections.Generic;
using System.Security;
using Key2Joy.Contracts.Plugins;
using System.Security.Permissions;

namespace Key2Joy.PluginHost;

public struct AllowedPermissionsWithDescriptions
{
    public PermissionSet AllowedPermissions { get; private set; }
    public List<string> Descriptions { get; private set; }

    public AllowedPermissionsWithDescriptions(
        PermissionSet allowedPermissions,
        List<string> descriptions
    )
    {
        this.AllowedPermissions = allowedPermissions;
        this.Descriptions = descriptions;
    }

    public static string[] GetRelevantPermissions(
        AllowedPermissionsWithDescriptions allPermissionsWithDescriptions,
        PermissionSet desiredPermissions
    )
    {
        var relevantDescriptions = new string[desiredPermissions.Count];

        var index = 0;
        foreach (var permission in allPermissionsWithDescriptions.AllowedPermissions)
        {
            foreach (var additionalPermission in desiredPermissions)
            {
                if (permission.Equals(additionalPermission))
                {
                    relevantDescriptions[index] = allPermissionsWithDescriptions.Descriptions[index];
                    index++;
                    break;
                }
            }
        }

        return relevantDescriptions;
    }

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

        return new AllowedPermissionsWithDescriptions(allowedPermissions, descriptions);
    }
}
