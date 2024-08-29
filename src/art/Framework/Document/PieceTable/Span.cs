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
    public index End => Start + Length;

    /// <summary>
    /// System.Range: Represents the inclusive end index of the Span.
    /// Inclusive means:
    ///     for(int k = start; k <= end; k++) ...
    /// </summary>
    public index EndInclusive => Start + Length - 1;

    public size Length { get; init; }

    public Span(index start, size length)
    {
        Assert.Ensure(start >= 0, nameof(start));
        Assert.Ensure(length >= 0, nameof(length));

        Start = start;
        Length = length;
    }

    public bool IsEmpty() => Length == 0;

    public bool Overlap(Span other, bool includeTouched = true)
        => includeTouched ? Start <= other.End && other.Start <= End : Start < other.End && other.Start < End;

    public bool Contains(location location) => Start <= location && location <= EndInclusive; // inclusively

    /// <summary>
    /// Merges overlapped spans.
    /// </summary>
    /// <param name="spans"></param>
    /// <returns></returns>
    public static List<Span> Merge(List<Span> spans)
    {
        Assert.NonEmptyCollection(spans, nameof(spans));

        List<Span> mergedSpans = new();

        spans.Sort((x, y) =>
        {
            if(x.Start < y.Start)
                return -1;
            else if(x.Start > y.Start)
                return +1;
            else // ==
                return 0;
        });

        index currentSpanStart = spans[0].Start;
        index currentSpanEnd = spans[0].End;

        for(index k = 1, n = spans.Count; k < n; k++)
        {
            if(currentSpanEnd >= spans[k].Start)
            {
                currentSpanEnd = Math.Max(currentSpanEnd, spans[k].End);
            }
            else
            {
                mergedSpans.Add(new Span(currentSpanStart, currentSpanEnd - currentSpanStart));

                currentSpanStart = spans[k].Start;
                currentSpanEnd = spans[k].End;
            }
        }

        mergedSpans.Add(new Span(currentSpanStart, currentSpanEnd - currentSpanStart));

        return mergedSpans;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return Length;
    }
}
