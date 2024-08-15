//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace Art.Framework.Document.PieceTable;

/// <summary>
/// Semi-open integer interval/range/view: [start ... end).
/// </summary>
public sealed class Span : EquatableType
{
    public static readonly Span Sentinel = new(0, 0);

    /// <summary>
    /// System.Range: Represents the inclusive start index of the Span.
    /// </summary>
    public index Start { get; init; }

    /// <summary>
    /// System.Range: Represents the exclusive end index of the Span.
    /// Exclusive means:
    ///     for(int k = start; k < end; k++) ...
    /// </summary>
    public size End => Start + Length;

    /// <summary>
    /// System.Range: Represents the inclusive end index of the Span.
    /// Inclusive means:
    ///     for(int k = start; k <= end; k++) ...
    /// </summary>
    public size EndInclusive => Start + Length - 1;

    public size Length { get; init; }

    public Span(index start, size length)
    {
        Assert.Ensure(start >= 0, nameof(start));
        Assert.Ensure(length >= 0, nameof(length));

        Start = start;
        Length = length;
    }

    public bool IsEmpty() => Length == 0;

    public bool Overlap(Span other, bool includeTouched = true) => includeTouched ? Start <= other.End && other.Start <= End : Start < other.End && other.Start < End;

    public bool Contains(location location) => Start <= location && location <= EndInclusive; // inclusively

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return Length;
    }
}
