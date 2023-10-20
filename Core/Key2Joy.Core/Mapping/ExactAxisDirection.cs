using System;

namespace Key2Joy.Mapping;

/// <summary>
/// More detailed specification of direction than <see cref="AxisDirection" />.
/// </summary>
public struct ExactAxisDirection : IEquatable<ExactAxisDirection>
{
    /// <summary>
    /// A fraction from -1 to 1, where -1 is left and 1 is right.
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// A fraction from -1 to 1, where -1 is up and 1 is down.
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="ExactAxisDirection"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public ExactAxisDirection(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
        => obj is ExactAxisDirection direction && this.Equals(direction);

    /// <inheritdoc/>
    public readonly bool Equals(ExactAxisDirection other)
        => this.X == other.X && this.Y == other.Y;

    /// <inheritdoc/>
    public override readonly int GetHashCode()
    {
        var hashCode = 1861411795;
        hashCode = hashCode * -1521134295 + this.X.GetHashCode();
        hashCode = hashCode * -1521134295 + this.Y.GetHashCode();
        return hashCode;
    }

    /// <inheritdoc/>
    public override readonly string ToString()
        => $"({this.X}, {this.Y})";
}
