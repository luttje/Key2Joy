using System.Windows.Forms;

namespace Key2Joy.Gui.Util;

internal static class DialogUtilities
{
    internal static DialogResult Confirm(string content, string title = null)
        => MessageBox.Show(
            content,
            title ?? "Confirm",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
}
