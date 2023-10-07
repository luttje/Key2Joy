using System.Collections.Generic;
using System.Security;

namespace Key2Joy.PluginHost
{
    public struct AllowedPermissionsWithDescriptions
    {
        public PermissionSet AllowedPermissions;
        public List<string> Descriptions;
    }
}