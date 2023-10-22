using System;

namespace Key2Joy.Contracts.Mapping;

public struct Message
{
    public IntPtr HWnd { get; private set; }

    public int Msg { get; private set; }

    public IntPtr WParam { get; private set; }

    public IntPtr LParam { get; private set; }

    public Message(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
        this.HWnd = hWnd;
        this.Msg = msg;
        this.WParam = wParam;
        this.LParam = lParam;
    }
}

public interface IWndProcHandler
{
    public IntPtr Handle { get; set; }

    /// <summary>
    /// Called when a message is sent to the window.
    /// </summary>
    /// <param name="message"></param>
    /// <returns>Whether to override default behaviour</returns>
    public bool WndProc(Message message);
}
