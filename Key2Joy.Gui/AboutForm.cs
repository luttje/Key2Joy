using System;
using System.Reflection;
using System.Windows.Forms;

namespace Key2Joy.Gui;

internal partial class AboutForm : Form
{
    public AboutForm()
    {
        this.InitializeComponent();
        this.Text = string.Format("About {0}", this.AssemblyTitle);
        this.labelProductName.Text = this.AssemblyProduct;
        this.labelVersion.Text = string.Format("Version {0}", this.Version);
        this.labelCopyright.Text = this.AssemblyCopyright;

        this.CalculateLogoSize();
    }

    private static T GetAttributeValue<T>(Func<Assembly, object[]> getAttributesFunc) where T : Attribute
    {
        var attributes = getAttributesFunc(Assembly.GetExecutingAssembly());
        if (attributes.Length > 0)
        {
            return attributes[0] as T;
        }
        return null;
    }

    public string AssemblyTitle
        => GetAttributeValue<AssemblyTitleAttribute>(
                asm => asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)
           )?.Title
        ?? System.IO.Path.GetFileNameWithoutExtension(
            Assembly.GetExecutingAssembly().CodeBase
        );

    public string Version
    {
        get
        {
            var versionFile = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "VERSION"
            );

            if (System.IO.File.Exists(versionFile))
            {
                return System.IO.File.ReadAllText(versionFile);
            }
            else
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
    }

    public string AssemblyDescription
        => GetAttributeValue<AssemblyDescriptionAttribute>(
                asm => asm.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
            )?.Description ?? "";

    public string AssemblyProduct
        => GetAttributeValue<AssemblyProductAttribute>(
                asm => asm.GetCustomAttributes(typeof(AssemblyProductAttribute), false)
            )?.Product ?? "";

    public string AssemblyCopyright
        => GetAttributeValue<AssemblyCopyrightAttribute>(
                asm => asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)
            )?.Copyright ?? "";

    public string AssemblyCompany
        => GetAttributeValue<AssemblyCompanyAttribute>(
                asm => asm.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)
            )?.Company ?? "";

    private void AboutForm_Resize(object sender, EventArgs e)
        => this.CalculateLogoSize();

    private void CalculateLogoSize()
        => this.pctLogo.Height = this.pctLogo.Width;
}
