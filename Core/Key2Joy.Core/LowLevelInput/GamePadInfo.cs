using System;

namespace Key2Joy.LowLevelInput;

public class GamePadActivityOccurredEventArgs : EventArgs
{ }

public class GamePadInfo : IGamePadInfo
{
    /// <inheritdoc/>
    public event EventHandler<GamePadActivityOccurredEventArgs> ActivityOccurred;

    /// <inheritdoc/>
    public int Index { get; }

    /// <inheritdoc/>
    public string Name { get; }

    public GamePadInfo(int index, string name)
    {
        this.Index = index;
        this.Name = name;
    }

    /// <inheritdoc/>
    public void OnActivityOccurred()
        => this.ActivityOccurred?.Invoke(this, new());
}

public interface IGamePadInfo
{
    /// <summary>
    /// Called when gamepad activity occurs.
    /// </summary>
    event EventHandler<GamePadActivityOccurredEventArgs> ActivityOccurred;

    /// <summary>
    /// The index of the gamepad
    /// </summary>
    int Index { get; }

    /// <summary>
    /// The display name for this gamepad
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Raises the <see cref="ActivityOccurred"/> event.
    /// </summary>
    void OnActivityOccurred();
}
