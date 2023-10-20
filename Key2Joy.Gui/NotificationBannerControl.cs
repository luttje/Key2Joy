using System;
using System.Drawing;
using System.Windows.Forms;

namespace Key2Joy.Gui;

public enum NotificationBannerStyle
{
    Information = 0,
    Warning = 1,
    Error = 2,
}

public partial class NotificationBannerControl : UserControl
{
    private const int BORDER_WIDTH = 2;
    private const int BORDER_MARGIN = 2;

    private readonly string message;
    private readonly NotificationBannerStyle style;
    private readonly Timer timeLeftTimer;

    public NotificationBannerControl(
        string message,
        NotificationBannerStyle style = NotificationBannerStyle.Information,
        TimeSpan? duration = null
    )
    {
        this.InitializeComponent();

        this.message = message;
        this.style = style;

        this.lblMessage.Text = message;

        duration ??= TimeSpan.FromSeconds(10);

        this.timeLeftTimer = new Timer
        {
            Interval = (int)Math.Min(1000, duration.Value.TotalMilliseconds)
        };
        this.timeLeftTimer.Tick += (s, e) =>
        {
            // Show the remaining time in the btnClose every second, close when times up
            duration -= TimeSpan.FromSeconds(1);
            this.btnClose.Text = $"✖ {duration.Value.TotalSeconds:0}s";

            if (duration.Value.TotalSeconds <= 0)
            {
                this.Close();
            }
        };
        this.timeLeftTimer.Start();

        switch (this.style)
        {
            case NotificationBannerStyle.Information:
                this.lblMessage.ForeColor = Color.White;
                this.BackColor = Color.DarkBlue;
                break;

            case NotificationBannerStyle.Warning:
                this.lblMessage.ForeColor = Color.Black;
                this.BackColor = Color.Gold;
                break;

            case NotificationBannerStyle.Error:
                this.lblMessage.ForeColor = Color.White;
                this.BackColor = Color.Crimson;
                break;
        }
    }

    private void Close()
    {
        this.timeLeftTimer.Stop();
        this.timeLeftTimer.Dispose();
        this.Parent?.Controls.Remove(this);
    }

    private void BtnClose_Click(object sender, System.EventArgs e)
        => this.Close();

    private void NotificationBannerControl_Paint(object sender, PaintEventArgs e)
    {
        const ButtonBorderStyle borderStyle = ButtonBorderStyle.Dashed;
        var borderColor = Color.WhiteSmoke;

        if (this.style == NotificationBannerStyle.Warning)
        {
            borderColor = Color.Black;
        }

        ControlPaint.DrawBorder(
            e.Graphics,
            new Rectangle(
                this.ClientRectangle.X + BORDER_MARGIN,
                this.ClientRectangle.Y + BORDER_MARGIN,
                this.ClientRectangle.Width - (BORDER_MARGIN * 2),
                this.ClientRectangle.Height - (BORDER_MARGIN * 2)
            ),
            borderColor,
            BORDER_WIDTH,
            borderStyle,
            borderColor,
            BORDER_WIDTH,
            borderStyle,
            borderColor,
            BORDER_WIDTH,
            borderStyle,
            borderColor,
            BORDER_WIDTH,
            borderStyle
        );
    }
}
