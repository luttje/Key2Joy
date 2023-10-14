using System;
using System.Security;
using System.Security.Permissions;

namespace Key2Joy.Contracts.Plugins;

public class SecurityOverride : CodeAccessPermission, IPermission, ICloneable
{
    public bool IsUnrestricted { get; set; } = false;

    // This constructor creates and initializes a permission with generic access.
    public SecurityOverride(PermissionState state) => this.IsUnrestricted = state == PermissionState.Unrestricted;

    public override IPermission Copy()
    {
        var copiedPermission = (SecurityOverride)this.Clone();
        copiedPermission.IsUnrestricted = this.IsUnrestricted;
        return copiedPermission;
    }

    public override void FromXml(SecurityElement e)
    {
        this.IsUnrestricted = false;

        // If XML indicates an unrestricted permission, make this permission unrestricted.
        var s = (string)e.Attributes["Unrestricted"];

        if (s != null)
        {
            this.IsUnrestricted = Convert.ToBoolean(s);
        }
    }

    public override IPermission Intersect(IPermission target) => throw new NotImplementedException();

    private SecurityOverride VerifyTypeMatch(IPermission target)
    {
        if (this.GetType() != target.GetType())
        {
            throw new ArgumentException(string.Format("target must be of the {0} type",
                this.GetType().FullName));
        }

        return (SecurityOverride)target;
    }

    public override bool IsSubsetOf(IPermission target)
    {
        if (target == null)
        {
            return false;
        }

        var permission = this.VerifyTypeMatch(target);

        return permission.IsUnrestricted == this.IsUnrestricted;
    }

    public override SecurityElement ToXml()
    {
        // These first three lines create an element with the required format.
        var e = new SecurityElement("IPermission");

        // Replace the double quotation marks ("") with single quotation marks ('')
        // to remain XML compliant when the culture is not neutral.
        e.AddAttribute("class", this.GetType().AssemblyQualifiedName.Replace('\"', '\''));
        e.AddAttribute("version", "1");

        if (this.IsUnrestricted)
        {
            e.AddAttribute("Unrestricted", "true");
        }

        return e;
    }

    public IPermission Union(IPermission target) => throw new NotImplementedException();

    public object Clone() => this.MemberwiseClone();
}
