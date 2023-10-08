using System.Collections.Generic;
using System.Security;

namespace Key2Joy.PluginHost;

public struct AllowedPermissionsWithDescriptions
{
    public PermissionSet AllowedPermissions { get; set; }
    public List<string> Descriptions { get; set; }
}
