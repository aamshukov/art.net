//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using Art.Framework.Document.PieceTable;

namespace UILab.Art.Tests;

[TestFixture]
public partial class PieceTableTests
{
    [Test]
    public void PieceTable_SpansMerge_Success()
    {
        // [ (8, 10), (1, 3), (15, 18), (2, 6) ] => [ (1, 6), (8, 10), (15, 18) ]
        var mergedSpans = Span.Merge([new Span(8, 2), new Span(1, 2), new Span(15, 3), new Span(2, 4)]);
        Assert.That(mergedSpans.SequenceEqual([new(1, 5), new(8, 2), new(15, 3)]), Is.True);

        // [ (1, 5), (5, 11) ] => ( (1, 11) ]
        mergedSpans = Span.Merge([new Span(1, 4), new Span(5, 6)]);
        Assert.That(mergedSpans.SequenceEqual([new(1, 10)]), Is.True);

        // [ (8, 10), (2, 4), (11, 11), (1, 3), (5, 9) ] => [ (1, 4), (5, 10), (11, 11) ]
        mergedSpans = Span.Merge([new Span(8, 2), new Span(2, 2), new Span(11, 0), new Span(1, 2), new Span(5, 4)]);
        Assert.That(mergedSpans.SequenceEqual([new(1, 3), new Span(5, 5), new Span(11, 0)]), Is.True);

        // [ (8, 10) ] => [ (8, 10) ]
        mergedSpans = Span.Merge([new Span(8, 2)]);
        Assert.That(mergedSpans.SequenceEqual([new(8, 2)]), Is.True);

        // [ (0, 0), (0, 0) ] => [ (0, 0) ]
        mergedSpans = Span.Merge([new Span(0, 0), new Span(0, 0)]);
        Assert.That(mergedSpans.SequenceEqual([new(0, 0)]), Is.True);

        // [ (0, 1), (1, 1) ] => [ (0, 1) ]
        mergedSpans = Span.Merge([new Span(0, 1), new Span(1, 1)]);
        Assert.That(mergedSpans.SequenceEqual([new(0, 2)]), Is.True);

        // [ (0, 1), (1, 1), (2, 1) ] => [ (0, 3) ]
        mergedSpans = Span.Merge([new Span(0, 1), new Span(1, 1), new Span(2, 1)]);
        Assert.That(mergedSpans.SequenceEqual([new(0, 3)]), Is.True);
    }
}
