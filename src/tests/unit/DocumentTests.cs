//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
#pragma warning disable CS8981
global using codepoint = uint;
#pragma warning restore CS8981

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Art.Framework.Core.Content;
using Art.Framework.Document.History;
using Art.Framework.Document.PieceTable;
using UILab.Art.Framework.Core.Content.Abstractions;
using UILab.Art.Framework.Core.DataAccess;
using UILab.Art.Framework.Core.DataAccess.Abstractions;
using UILab.Art.Framework.Core.Text.Search;
using UILab.Art.Framework.Document;
using UILab.Art.Framework.Document.Abstractions;
using ArtText = UILab.Art.Framework.Core.Text.Text;

namespace UILab.Art.Tests;

internal sealed class ContentComparer : IEqualityComparer<codepoint[]>
{
    public bool Equals(codepoint[]? x, codepoint[]? y)
    {
        return (x != default && y != default) && x.SequenceEqual(y);
    }

    public int GetHashCode([DisallowNull] codepoint[] obj)
    {
        return obj.GetHashCode();
    }
}

[TestFixture]
internal class DocumentTests
{
    private static void CompareContents(ReadOnlyMemory<codepoint> codepoints, Buffer<codepoint> buffer)
    {
        var data1 = buffer.Data.Slice(0, buffer.Size).ToArray();
        var data2 = codepoints.Slice(0, codepoints.Length).ToArray();

        Assert.That(data1, Is.EqualTo(data2).Using(new ContentComparer()));
    }

    private static string GetString(ReadOnlyMemory<codepoint> codepoints)
    {
        var sb = new StringBuilder(codepoints.Length);
        return codepoints.ToArray().Aggregate(sb, (result, codepoint) => sb.Append(Char.ConvertFromUtf32((int)codepoint))).ToString();
    }

    private static IContent<codepoint> LoadContent(string fileName, size bufferSize)
    {
        IContent<codepoint> content = new DocumentContent("Test Content", "Test Source", bufferSize, "1.0");

        FileDataProvider dataProvider = new(fileName, "1.0");
        dataProvider.Load(content);

        return content;
    }

    private static IDocument GetDocument(string data, size bufferSize, IContent<codepoint>? loadedContent = default)
    {
        IContent<codepoint> content = loadedContent ?? new DocumentContent("Test Content", "Test Source", bufferSize, "1.0");

        if(loadedContent is null)
        {
            StringDataProvider dataProvider = new(data, bufferSize, "1.0");
            dataProvider.Load(content);
        }

        IDocumentHistory history = new DocumentHistory();

        Document document = new(id: "Test Document", source: "string provider", content: content, history: history, bufferSize: bufferSize, version: "1.0");

        PropertyInfo? workingContentProperty = typeof(Document).GetProperty("WorkingContent", BindingFlags.Instance | BindingFlags.NonPublic);
        var workingContent = workingContentProperty?.GetValue(document) as IContent<codepoint>;
        Assert.That(workingContent, Is.Not.Null);

        var size = document.Length();

        return document;
    }

    [Test]
    public void Document_AddGet_Content_Success()
    {
        size bufferSize = 10;

        IContent<codepoint> content = new DocumentContent("Test Content", "Test Source", bufferSize, "1.0");

        var codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        count count = content.AppendContent(codepoints);
        Assert.That(count, Is.EqualTo(codepoints.Length));
        var sequence = content.GetContent(location: 2, length: 7);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2', '3', '4', '5', '6', '7', '8']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 2, length: 6);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2', '3', '4', '5', '6', '7']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 2, length: 20);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2', '3', '4', '5', '6', '7']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 2);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2', '3', '4', '5', '6', '7']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 2, length: 12);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2', '3', '4', '5', '6', '7', '8', '9']).Span), Is.True);
        Assert.That(sequence[1].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 14, length: 10);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['4', '5', '6', '7', '8', '9']).Span), Is.True);
        Assert.That(sequence[1].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 8, length: 19);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['8', '9']).Span), Is.True);
        Assert.That(sequence[1].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']).Span), Is.True);
        Assert.That(sequence[2].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 0);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']).Span), Is.True);
        Assert.That(sequence[1].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']).Span), Is.True);
        Assert.That(sequence[2].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 0);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']).Span), Is.True);
        Assert.That(sequence[1].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']).Span), Is.True);
        Assert.That(sequence[2].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 0, length: 10);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 0, length: 11);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']).Span), Is.True);
        Assert.That(sequence[1].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 0, length: 0);
        Assert.That(sequence, Has.Count.EqualTo(0));
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 0, length: 1);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 9, length: 1);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['9']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 5, length: 1);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['5']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 0, length: 0);
        Assert.That(sequence.Count == 0, Is.True);
        content.Clear();
    }

    [Test]
    public void Document_AddGet_Content_1_Success()
    {
        size bufferSize = 1;

        IContent<codepoint> content = new DocumentContent("Test Content", "Test Source", bufferSize, "1.0");

        var codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        count count = content.AppendContent(codepoints);
        Assert.That(count, Is.EqualTo(codepoints.Length));
        var sequence = content.GetContent(location: 2, length: 7);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2']).Span), Is.True);
        Assert.That(sequence[1].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['3']).Span), Is.True);
        Assert.That(sequence[2].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['4']).Span), Is.True);
        Assert.That(sequence[3].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['5']).Span), Is.True);
        Assert.That(sequence[4].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['6']).Span), Is.True);
        Assert.That(sequence[5].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['7']).Span), Is.True);
        Assert.That(sequence[6].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['8']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 0);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0']).Span), Is.True);
        Assert.That(sequence[1].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['1']).Span), Is.True);
        Assert.That(sequence[2].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2']).Span), Is.True);
        Assert.That(sequence[3].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['3']).Span), Is.True);
        Assert.That(sequence[4].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['4']).Span), Is.True);
        Assert.That(sequence[5].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['5']).Span), Is.True);
        Assert.That(sequence[6].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['6']).Span), Is.True);
        Assert.That(sequence[7].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['7']).Span), Is.True);
        Assert.That(sequence[8].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['8']).Span), Is.True);
        Assert.That(sequence[9].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['9']).Span), Is.True);
        Assert.That(sequence[10].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0']).Span), Is.True);
        Assert.That(sequence[11].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['1']).Span), Is.True);
        Assert.That(sequence[12].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2']).Span), Is.True);
        Assert.That(sequence[13].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['3']).Span), Is.True);
        Assert.That(sequence[14].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['4']).Span), Is.True);
        Assert.That(sequence[15].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['5']).Span), Is.True);
        Assert.That(sequence[16].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['6']).Span), Is.True);
        Assert.That(sequence[17].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['7']).Span), Is.True);
        Assert.That(sequence[18].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['8']).Span), Is.True);
        Assert.That(sequence[19].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['9']).Span), Is.True);
        Assert.That(sequence[20].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0']).Span), Is.True);
        Assert.That(sequence[21].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['1']).Span), Is.True);
        Assert.That(sequence[22].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2']).Span), Is.True);
        Assert.That(sequence[23].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['3']).Span), Is.True);
        Assert.That(sequence[24].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['4']).Span), Is.True);
        Assert.That(sequence[25].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['5']).Span), Is.True);
        Assert.That(sequence[26].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['6']).Span), Is.True);
        Assert.That(sequence[27].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['7']).Span), Is.True);
        Assert.That(sequence[28].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['8']).Span), Is.True);
        Assert.That(sequence[29].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['9']).Span), Is.True);
        content.Clear();

        codepoints = new ReadOnlyMemory<codepoint>(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        content.AppendContent(codepoints);
        sequence = content.GetContent(location: 8, length: 19);
        Assert.That(sequence[0].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['8']).Span), Is.True);
        Assert.That(sequence[1].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['9']).Span), Is.True);
        Assert.That(sequence[2].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0']).Span), Is.True);
        Assert.That(sequence[3].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['1']).Span), Is.True);
        Assert.That(sequence[4].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2']).Span), Is.True);
        Assert.That(sequence[5].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['3']).Span), Is.True);
        Assert.That(sequence[6].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['4']).Span), Is.True);
        Assert.That(sequence[7].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['5']).Span), Is.True);
        Assert.That(sequence[8].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['6']).Span), Is.True);
        Assert.That(sequence[9].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['7']).Span), Is.True);
        Assert.That(sequence[10].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['8']).Span), Is.True);
        Assert.That(sequence[11].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['9']).Span), Is.True);
        Assert.That(sequence[12].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['0']).Span), Is.True);
        Assert.That(sequence[13].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['1']).Span), Is.True);
        Assert.That(sequence[14].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['2']).Span), Is.True);
        Assert.That(sequence[15].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['3']).Span), Is.True);
        Assert.That(sequence[16].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['4']).Span), Is.True);
        Assert.That(sequence[17].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['5']).Span), Is.True);
        Assert.That(sequence[18].Span.SequenceEqual(new ReadOnlyMemory<codepoint>(['6']).Span), Is.True);
        content.Clear();
    }

    [Test]
    public void Document_AppendContent_Success()
    {
        size bufferSize = 10;

        IContent<codepoint> content = new DocumentContent("Test Content", "Test Source", bufferSize, "1.0");
        
        IDataProvider<codepoint> dataProvider = new StringDataProvider("123456", 10, "1.0");
        dataProvider.Load(content);

        IDocumentHistory history = new DocumentHistory();

        using Document document = new(id: "Test Document", source: "string provider", content: content, history: history, bufferSize: bufferSize, version: "1.0");

        //MethodInfo? appendContent = typeof(Document).GetMethod("AppendContent", BindingFlags.Instance | BindingFlags.NonPublic);

        PropertyInfo? workingContentProperty = typeof(Document).GetProperty("WorkingContent", BindingFlags.Instance | BindingFlags.NonPublic);
        var workingContent = workingContentProperty?.GetValue(document) as IContent<codepoint>;

        if(workingContent != null)
        {
            var codepoints = new ReadOnlyMemory<codepoint>(new codepoint[] { '0', '1', '2', '3', '4', '5' });
            //appendContent?.Invoke(document, [codepoints]);
            var count = workingContent.AppendContent(codepoints);
            Assert.That(count, Is.EqualTo(codepoints.Length));
            CompareContents(new codepoint[] { '0', '1', '2', '3', '4', '5' }, workingContent.Contents[0]);

            codepoints = new ReadOnlyMemory<codepoint>(new codepoint[] { 'a', 'b' });
            count = workingContent.AppendContent(codepoints);
            Assert.That(count, Is.EqualTo(codepoints.Length));
            CompareContents(new codepoint[] { '0', '1', '2', '3', '4', '5', 'a', 'b' }, workingContent.Contents[0]);

            codepoints = new ReadOnlyMemory<codepoint>(new codepoint[] { '0', '1', '2', '3', '4', '5' });
            count = workingContent.AppendContent(codepoints);
            Assert.That(count, Is.EqualTo(codepoints.Length));
            CompareContents(new codepoint[] { '0', '1', '2', '3', '4', '5', 'a', 'b', '0', '1' }, workingContent.Contents[0]);
            CompareContents(new codepoint[] { '2', '3', '4', '5' }, workingContent.Contents[1]);

            codepoints = new ReadOnlyMemory<codepoint>(new codepoint[] { '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            count = workingContent.AppendContent(codepoints);
            Assert.That(count, Is.EqualTo(codepoints.Length));
            CompareContents(new codepoint[] { '2', '3', '4', '5', '4', '5', '6', '7', '8', '9' }, workingContent.Contents[1]);
            CompareContents(new codepoint[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, workingContent.Contents[2]);
            CompareContents(new codepoint[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, workingContent.Contents[3]);
        }
    }

    [Test]
    public void Document_AddAtBoundary_StartEnd_Document_Success()
    {
        size bufferSize = 10;

        IContent<codepoint> content = new DocumentContent("Test Content", "Test Source", bufferSize, "1.0");

        StringDataProvider dataProvider = new("", 10, "1.0");
        dataProvider.Load(content);

        IDocumentHistory history = new DocumentHistory();

        using Document document = new(id: "Test Document", source: "string provider", content: content, history: history, bufferSize: bufferSize, version: "1.0");

        PropertyInfo? workingContentProperty = typeof(Document).GetProperty("WorkingContent", BindingFlags.Instance | BindingFlags.NonPublic);
        var workingContent = workingContentProperty?.GetValue(document) as IContent<codepoint>;

        var size = document.Length();
        Assert.That(size, Is.EqualTo(dataProvider.Data.Length));

        var codepoints = ArtText.GetCodepoints("a");
        var codepointsStr = GetString(codepoints);
        var codepointsLength = codepoints.Length;
        var insertedCount = document.Insert(0, codepoints);
        size = document.Length();
        var docStr = document.GetString(0);

        Assert.Multiple(() =>
        {
            Assert.That(size, Is.EqualTo(codepoints.Length));
            if(workingContent != null)
                CompareContents(codepoints, workingContent.Contents[0]);
        });
    }

    [Test]
    public void Document_GetSequenceAsString_Success()
    {
        size bufferSize = 10;

        string [] text =
        [
            "",
            "0",
            "01",
            "012",
            "0123",
            "01234",
            "012345",
            "0123456",
            "01234567",
            "012345678",
            "0123456789",
            "01234567890123456789",
            "012345678901234567890123456789012345678901234567890123456789012345678901234567892",
        ];

        foreach(var str in text)
        {
            using Document document = (Document)GetDocument(str, bufferSize);
            var codepoints = ArtText.GetCodepoints(str);
            var codepointsStr = GetString(codepoints);
            Assert.That(codepointsStr, Is.Not.Null);
            Assert.That(codepoints.Length, Is.EqualTo(codepointsStr.Length));
            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(codepointsStr));
        }
    }

    [Test]
    public void Document_GetSequenceAsString_Start_Middle_End_OnePiece_Success()
    {
        size bufferSize = 10;

        //             01234567890123456789
        //             ABCDEFGHJK0123456789
        string text = "0123456789";

        using Document document = (Document)GetDocument(text, bufferSize);
        var codepoints = ArtText.GetCodepoints("ABCDEFGHJK");
        var codepointsStr = GetString(codepoints);
        Assert.That(codepointsStr, Is.Not.Null);
        Assert.That(codepoints.Length, Is.EqualTo(codepointsStr.Length));
        var insertedCount = document.Insert(0, codepoints);
        var documentStr = document.GetString(0);

        var sequenceStr = document.GetString(0, 0);
        Assert.That(sequenceStr, Is.EqualTo(""));

        sequenceStr = document.GetString(0, 6);
        Assert.That(sequenceStr, Is.EqualTo("ABCDEF"));

        sequenceStr = document.GetString(2, 6);
        Assert.That(sequenceStr, Is.EqualTo("CDEFGH"));

        sequenceStr = document.GetString(2, 8);
        Assert.That(sequenceStr, Is.EqualTo("CDEFGHJK"));

        sequenceStr = document.GetString(2, 12);
        Assert.That(sequenceStr, Is.EqualTo("CDEFGHJK0123"));

        sequenceStr = document.GetString(0, 1);
        Assert.That(sequenceStr, Is.EqualTo("A"));

        sequenceStr = document.GetString(1, 1);
        Assert.That(sequenceStr, Is.EqualTo("B"));

        sequenceStr = document.GetString(3, 1);
        Assert.That(sequenceStr, Is.EqualTo("D"));

        sequenceStr = document.GetString(5, 12);
        Assert.That(sequenceStr, Is.EqualTo("FGHJK0123456"));

        sequenceStr = document.GetString(14, 2);
        Assert.That(sequenceStr, Is.EqualTo("45"));

        sequenceStr = document.GetString(17, 1);
        Assert.That(sequenceStr, Is.EqualTo("7"));

        sequenceStr = document.GetString(18, 1);
        Assert.That(sequenceStr, Is.EqualTo("8"));

        sequenceStr = document.GetString(18);
        Assert.That(sequenceStr, Is.EqualTo("89"));

        sequenceStr = document.GetString(19);
        Assert.That(sequenceStr, Is.EqualTo("9"));

        sequenceStr = document.GetString(20);
        Assert.That(sequenceStr, Is.EqualTo(""));

        sequenceStr = document.GetString(21);
        Assert.That(sequenceStr, Is.EqualTo(""));

        sequenceStr = document.GetString(22);
        Assert.That(sequenceStr, Is.EqualTo(""));

        sequenceStr = document.GetString(200);
        Assert.That(sequenceStr, Is.EqualTo(""));
    }

    [Test]
    public void Document_GetSequenceAsString_Insert_AtTheBegining_Success()
    {
        size bufferSize = 10;

        string [] text =
        [
            "",
            "0",
            "01",
            "012",
            "0123",
            "01234",
            "012345",
            "0123456",
            "01234567",
            "012345678",
            "0123456789",
            "01234567890123456789",
            "012345678901234567890123456789012345678901234567890123456789012345678901234567892",
        ];

        foreach(var str in text)
        {
            using Document document = (Document)GetDocument(str, bufferSize);

            var codepoints = ArtText.GetCodepoints(str);
            var codepointsStr = GetString(codepoints);
            Assert.That(codepointsStr, Is.Not.Null);
            Assert.That(codepoints.Length, Is.EqualTo(codepointsStr.Length));

            var insertedCount = document.Insert(0, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));

            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(codepointsStr + codepointsStr));
        }
    }

    [Test]
    public void Document_GetSequenceAsString_Random_Success()
    {
        count count = 100;
        size length = 10000;

        for(int k = 0; k < count; k++)
        {
            size bufferSize = RandomNumberGenerator.GetInt32(1, length);
            var randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
            using Document document = (Document)GetDocument(randomStr, bufferSize);
            var codepoints = ArtText.GetCodepoints(randomStr);
            var codepointsStr = GetString(codepoints);
            Assert.That(codepointsStr, Is.Not.Null);
            Assert.That(codepoints.Length, Is.EqualTo(codepointsStr.Length));
            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(codepointsStr));
        }
    }

    [Test]
    public void Document_GetSequenceAsString_FixedBuffer_Random_Success()
    {
        count count = 1000;
        size length = 1000;
        size bufferSize = 10;

        for(int k = 0; k < count; k++)
        {
            var randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
            using Document document = (Document)GetDocument(randomStr, bufferSize);
            var codepoints = ArtText.GetCodepoints(randomStr);
            var codepointsStr = GetString(codepoints);
            Assert.That(codepointsStr, Is.Not.Null);
            Assert.That(codepoints.Length, Is.EqualTo(codepointsStr.Length));
            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(codepointsStr));
        }
    }

    [Test]
    public void Document_AddAtBoundary_StartEnd_Success()
    {
        size bufferSize = 10;

        string[] text =
        [
            "",
                "0",
                "01",
                "012",
                "0123",
                "01234",
                "012345",
                "0123456",
                "01234567",
                "012345678",
                "0123456789",
                "01234567890123456789",
                "012345678901234567890123456789012345678901234567890123456789012345678901234567892",
                "012345678901234567890123456789012345678901234567890123456789012345678901234567892012345678901234567890123456789012345678901234567890123456789012345678901234567892012345678901234567890123456789012345678901234567890123456789012345678901234567892012345678901234567890123456789012345678901234567890123456789012345678901234567892012345678901234567890123456789012345678901234567890123456789012345678901234567892012345678901234567890123456789012345678901234567890123456789012345678901234567892012345678901234567890123456789012345678901234567890123456789012345678901234567892012345678901234567890123456789012345678901234567890123456789012345678901234567892012345678901234567890123456789012345678901234567890123456789012345678901234567892",
            ];

        foreach(var str in text)
        {
            using Document document = (Document)GetDocument(str, bufferSize);
            var codepoints = ArtText.GetCodepoints(str);
            var codepointsStr = GetString(codepoints);
            Assert.That(codepointsStr, Is.Not.Null);
            Assert.That(codepoints.Length, Is.EqualTo(codepointsStr.Length));
            var insertedCount = document.Insert(codepoints.Length, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            insertedCount = document.Insert(codepoints.Length * 2, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(codepointsStr + codepointsStr + codepointsStr));
        }
    }

    [Test]
    public void Document_GetSequenceAsString_StartEnd_Random_Success()
    {
        count count = 100;
        size length = 10000;

        for(int k = 0; k < count; k++)
        {
            size bufferSize = RandomNumberGenerator.GetInt32(1, length);
            var randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
            using Document document = (Document)GetDocument(randomStr, bufferSize);
            var codepoints = ArtText.GetCodepoints(randomStr);
            var codepointsStr = GetString(codepoints);
            var insertedCount = document.Insert(codepoints.Length, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            insertedCount = document.Insert(codepoints.Length * 2, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(codepointsStr + codepointsStr + codepointsStr));
        }
    }

    [Test]
    public void Document_GetSequenceAsString_StartEnd_SmallBuffer_Random_Success()
    {
        count count = 100;
        size length = 10000;

        for(int k = 0; k < count; k++)
        {
            size bufferSize = RandomNumberGenerator.GetInt32(1, 8);
            var randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
            using Document document = (Document)GetDocument(randomStr, bufferSize);
            var codepoints = ArtText.GetCodepoints(randomStr);
            var codepointsStr = GetString(codepoints);
            var insertedCount = document.Insert(codepoints.Length, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            insertedCount = document.Insert(codepoints.Length * 2, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(codepointsStr + codepointsStr + codepointsStr));
        }
    }

    [Test]
    public void Document_InsertText_Success()
    {
        for(int n = 1; n < 100; n++)
        {
            size bufferSize = n;

            string text = "ABCDEFGH";

            using Document document = (Document)GetDocument(text, bufferSize);

            var codepoints = ArtText.GetCodepoints("a");
            var insertedCount = document.Insert(0, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("aABCDEFGH"));

            codepoints = ArtText.GetCodepoints("b");
            insertedCount = document.Insert(documentStr.Length / 2, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("aABCbDEFGH"));

            codepoints = ArtText.GetCodepoints("c");
            insertedCount = document.Insert(documentStr.Length / 2 + 2, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("aABCbDEcFGH"));

            codepoints = ArtText.GetCodepoints("d");
            insertedCount = document.Insert(documentStr.Length, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("aABCbDEcFGHd"));

            codepoints = ArtText.GetCodepoints("z");
            insertedCount = document.Insert(documentStr.Length * 2, ArtText.GetCodepoints("z"));
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("aABCbDEcFGHdz"));
        }
    }

    [Test]
    public void Document_InsertText_Random_Success()
    {
        count count = 1000;
        size length = 10000;

        for(int k = 0; k < count; k++)
        {
            size bufferSize = RandomNumberGenerator.GetInt32(1, length);
            var randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
            using Document document = (Document)GetDocument(randomStr, bufferSize);
            var codepoints = ArtText.GetCodepoints(randomStr);
            var codepointsStr = GetString(codepoints);
            var offset = RandomNumberGenerator.GetInt32(0, length);
            var insertedCount = document.Insert(offset, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            offset = RandomNumberGenerator.GetInt32(0, length);
            insertedCount = document.Insert(offset, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            var documentStr = document.GetString(0);
        }
    }

    [Test]
    public void Document_InsertText_EveryOther_Forward_Success()
    {
        size bufferSize = 10;
        count count = 100;

        var text = "";
        var codepoints = ArtText.GetCodepoints("a");

        using Document document = (Document)GetDocument(text, bufferSize);

        for(var k = 0; k < count; k++)
        {
            var insertedCount = document.Insert(k, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            var data = document.GetString(0);
        }

        var documentStr = document.GetString(0);
        Assert.That(documentStr.Length, Is.EqualTo(count));
    }

    [Test]
    public void Document_InsertText_EveryOther_Backward_Success()
    {
        size bufferSize = 10;
        count count = 100;

        var text = "";
        var codepoints = ArtText.GetCodepoints("a");

        using Document document = (Document)GetDocument(text, bufferSize);

        for(var k = 0; k < count; k++)
        {
            var insertedCount = document.Insert(0, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            var data = document.GetString(0);
        }

        var documentStr = document.GetString(0);
        Assert.That(documentStr.Length, Is.EqualTo(count));
    }

    [Test]
    public void Document_InsertText_EveryOther_Seeded_Forward_Success()
    {
        size bufferSize = 10;
        count count = 100;

        var text = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(1024));

        var codepoints = ArtText.GetCodepoints("a");

        using Document document = (Document)GetDocument(text, bufferSize);

        for(var k = 0; k < count; k++)
        {
            var insertedCount = document.Insert(k, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            var data = document.GetString(0);
        }

        var documentStr = document.GetString(0);
        Assert.That(documentStr.Length, Is.EqualTo(text.Length + count));
    }

    [Test]
    public void Document_InsertText_EveryOther_Seeded_Backward_Success()
    {
        size bufferSize = 10;
        count count = 100;

        var text = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(1024));

        var codepoints = ArtText.GetCodepoints("a");

        using Document document = (Document)GetDocument(text, bufferSize);

        for(var k = 0; k < count; k++)
        {
            var insertedCount = document.Insert(0, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            var data = document.GetString(0);
        }

        var documentStr = document.GetString(0);
        Assert.That(documentStr, Has.Length.EqualTo(text.Length + count));
    }

    [Test]
    public void Document_DeleteText_Original_Success()
    {
        for(int n = 1; n < 100; n++)
        {
            size bufferSize = n;

            string text = "ABCDEFGH";

            using Document document = (Document)GetDocument(text, bufferSize);

            document.Delete(0, 1);
            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("BCDEFGH"));

            document.Delete(0, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("CDEFGH"));

            document.Delete(0, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("DEFGH"));

            document.Delete(1, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("DFGH"));

            document.Delete(2, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("DFH"));

            document.Delete(4, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("DFH"));

            document.Delete(5, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("DFH"));

            document.Delete(50);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("DFH"));

            var codepoints = ArtText.GetCodepoints("a");
            var insertedCount = document.Insert(0, codepoints);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("aDFH"));
        }
    }

    [Test]
    public void Document_DeleteText_Working_Success()
    {
        for(int n = 1; n < 100; n++)
        {
            size bufferSize = n;

            string text = "0123456789ABCDEFGH0123456789ABCDEFGH";

            using Document document = (Document)GetDocument(text, bufferSize);

            var codepoints = ArtText.GetCodepoints(text);
            var insertedCount = document.Insert(0, codepoints);
            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(text + text));
            var text2s = text + text;

            document.Delete(0, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(text2s.Substring(1)));

            document.Delete(0, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(text2s.Substring(2)));

            document.Delete(0, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(text2s.Substring(3)));

            document.Delete(1, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(text2s.Substring(3, 1) + text2s.Substring(5)));

            document.Delete(2, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("35789ABCDEFGH0123456789ABCDEFGH" + text));

            document.Delete(4, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("3578ABCDEFGH0123456789ABCDEFGH" + text));

            document.Delete(5, 1);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("3578ACDEFGH0123456789ABCDEFGH" + text));

            document.Delete(50);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("3578ACDEFGH0123456789ABCDEFGH0123456789ABCDEFGH012"));

            document.Delete(250);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo("3578ACDEFGH0123456789ABCDEFGH0123456789ABCDEFGH012"));
        }
    }

    [Test]
    public void Document_InsertDeleteText_Random_Success()
    {
        count count = 100;
        size length = 100000;

        for(int k = 0; k < count; k++)
        {
            size bufferSize = RandomNumberGenerator.GetInt32(1, length);

            var randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));

            using Document document = (Document)GetDocument(randomStr, bufferSize);

            randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
            var codepoints = ArtText.GetCodepoints(randomStr);

            var offset = RandomNumberGenerator.GetInt32(0, length);

            if((k % 2) == 0)
            {
                var insertedCount = document.Insert(codepoints.Length, codepoints);
                Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
            }
            else
            {
                document.Delete(offset, RandomNumberGenerator.GetInt32(0, length / 2));
            }
        }
    }

    [Test]
    public void Document_InsertDeleteText_Long_Random_Success()
    {
        count count = 100;
        size length = 100000;
        size bufferSize = RandomNumberGenerator.GetInt32(1, length);

        var randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
        var orgContent = randomStr;

        using Document document = (Document)GetDocument(randomStr, bufferSize);

        for(int k = 0; k < count; k++)
        {
            var offset = RandomNumberGenerator.GetInt32(0, length);
            offset = Math.Min(offset, orgContent.Length - 1);

            if((k % 2) == 0)
            {
                var orgContentBefore = orgContent;
                var documentStrBefore = document.GetString(0);
                randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
                var codepoints = ArtText.GetCodepoints(randomStr);
                orgContent = orgContent.Insert(offset, randomStr);
                var insertedCount = document.Insert(offset, codepoints);
                Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
                var documentStr = document.GetString(0);

                if(documentStr != orgContent)
                {
                    break;
                }

                Assert.That(documentStr, Is.EqualTo(orgContent));
            }
            else
            {
                var orgContentBefore = orgContent;
                var documentStrBefore = document.GetString(0);
                var randomLength = RandomNumberGenerator.GetInt32(0, Math.Max(1, length / 2));
                randomLength = Math.Min(0, orgContent.Length - offset);
                orgContent = orgContent.Remove(offset, randomLength);
                document.Delete(offset, randomLength);
                var documentStr = document.GetString(0);

                if(documentStr != orgContent)
                {
                    break;
                }

                Assert.That(documentStr, Is.EqualTo(orgContent));
            }
        }
    }

    [Test]
    public void Document_LoadContentFromFile_Success()
    {
        IContent<codepoint> content = LoadContent(@"d:\tmp\UnicodeData.txt", 10);
        Assert.That(content, Is.Not.Null);
    }

    [Test]
    public void Document_InsertDeleteText_FromFile_Random_Success()
    {
        count count = 10;
        size length = 1;

        List<Func<size, size>> actions = new()
            {
                (size length) => length,
                (size length) => length + 1,
                (size length) => length + 7,
                (size length) => length + 8,
                (size length) => length + 9,
                (size length) => length + 11,
                (size length) => length + 17,
                (size length) => length + 23,
                (size length) => length + 56
            };

        string fileName = @"Data\Document_InsertDeleteText_FromFile_Test.txt";
        string loadedContent = File.ReadAllText(fileName);
        loadedContent += Environment.NewLine;

        for(int j = 0; j < actions.Count; j++)
        {
            var action = actions[j];

            length = 1;

            for(int i = 1; i < count; i++)
            {
                size bufferSize = i;

                length = action(length);

                //IContent<codepoint> content = LoadContent(@"d:\tmp\Document_InsertDeleteText_FromFile_Test.txt", bufferSize);
                var orgContent = loadedContent;
                using Document document = (Document)GetDocument(orgContent, bufferSize, loadedContent: default /*content*/);

                for(int k = 0; k < count; k++)
                {
                    var offset = RandomNumberGenerator.GetInt32(0, length);

                    if((k % 2) == 0)
                    {
                        var orgContentBefore = orgContent;
                        var documentStrBefore = document.GetString(0);
                        var randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
                        var codepoints = ArtText.GetCodepoints(randomStr);
                        orgContent = orgContent.Insert(codepoints.Length, randomStr);
                        var insertedCount = document.Insert(codepoints.Length, codepoints);
                        Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
                        var documentStr = document.GetString(0);
                        Assert.That(documentStr, Is.EqualTo(orgContent));
                    }
                    else
                    {
                        var orgContentBefore = orgContent;
                        var documentStrBefore = document.GetString(0);
                        var randomLength = RandomNumberGenerator.GetInt32(0, Math.Max(1, length / 2));
                        orgContent = orgContent.Remove(offset, randomLength);
                        document.Delete(offset, randomLength);
                        var documentStr = document.GetString(0);

                        if(documentStr != orgContent)
                        {
                            break;
                        }

                        Assert.That(documentStr, Is.EqualTo(orgContent));
                    }
                }
            }
        }
    }

    [Test]
    public void Document_InsertDeleteText_Codepoints_Success()
    {
        size bufferSize = 1;

        string text = "ABCDEFGHJK";

        using Document document = (Document)GetDocument(text, bufferSize);
        var documentStr = document.GetString(0);

        var codepoints = ArtText.GetCodepoints("xyz");
        var insertedCount = document.Insert(1, codepoints);
        Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
        documentStr = document.GetString(0);

        codepoints = ArtText.GetCodepoints("opqrst");
        insertedCount = document.Insert(3, codepoints);
        documentStr = document.GetString(0);
        Assert.That(documentStr, Is.EqualTo("AxyopqrstzBCDEFGHJK"));

        for(int k = 0, n = documentStr.Length; k < n; k++)
        {
            var dch = documentStr[k];
            var cch = document.GetCodepoint(k);
            if(dch != cch)
            {
                Console.WriteLine($"dch:{dch} - cch:{cch}, k:{k}");
            }
            Assert.That(dch, Is.EqualTo(cch));
        }
    }


    [Test]
    public void Document_DeleteText_Debug_Success()
    {
        //                            012345678901  
        const string loadedContent = "ABCDEFJHKLMN";

        for(int n = 1; n < loadedContent.Length + 1; n++)
        {
            size bufferSize = n;

            var orgContent = loadedContent;
            using Document document = (Document)GetDocument(orgContent, bufferSize, loadedContent: default);
            var documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(orgContent));

            var offset = 2;
            var lengthToDelete = 1;
            var orgContentBeforeDelete = orgContent;
            var documentStrBeforeDelete = document.GetString(0);
            Assert.That(documentStrBeforeDelete, Is.EqualTo(orgContentBeforeDelete));
            Console.Write($"orgContentBeforeDelete: {orgContentBeforeDelete}");
            Console.Write($"documentStrBeforeDelete:{documentStrBeforeDelete}");
            Console.WriteLine($"offset:{offset}, lengthToDelete:{lengthToDelete}");
            orgContent = orgContent.Remove(offset, lengthToDelete);
            document.Delete(offset, lengthToDelete);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(orgContent));
            // 0 1 2 3 4 5 6 7 8 9 0 1
            // A B   D E F J H K L M N
            // 0:2   3:9
            Console.Write($"orgContent: {orgContent}");
            Console.Write($"documentStr:{documentStr}");
            Console.WriteLine("----");

            offset = 3;
            lengthToDelete = 1;
            orgContentBeforeDelete = orgContent;
            documentStrBeforeDelete = document.GetString(0);
            Assert.That(documentStrBeforeDelete, Is.EqualTo(orgContentBeforeDelete));
            Console.Write($"orgContentBeforeDelete: {orgContentBeforeDelete}");
            Console.Write($"documentStrBeforeDelete:{documentStrBeforeDelete}");
            Console.WriteLine($"offset:{offset}, lengthToDelete:{lengthToDelete}");
            orgContent = orgContent.Remove(offset, lengthToDelete);
            document.Delete(offset, lengthToDelete);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(orgContent));
            // 0 1 2 3 4 5 6 7 8 9 0 1
            // A B D F J H K L M N
            //
            // A B   D   F J H K L M N
            // 0:2   3:1 5:7
            Console.Write($"orgContent: {orgContent}");
            Console.Write($"documentStr:{documentStr}");
            Console.WriteLine("----");

            offset = 2;
            lengthToDelete = 1;
            orgContentBeforeDelete = orgContent;
            documentStrBeforeDelete = document.GetString(0);
            Assert.That(documentStrBeforeDelete, Is.EqualTo(orgContentBeforeDelete));
            Console.Write($"orgContentBeforeDelete: {orgContentBeforeDelete}");
            Console.Write($"documentStrBeforeDelete:{documentStrBeforeDelete}");
            Console.WriteLine($"offset:{offset}, lengthToDelete:{lengthToDelete}");
            orgContent = orgContent.Remove(offset, lengthToDelete);
            document.Delete(offset, lengthToDelete);
            documentStr = document.GetString(0);
            Assert.That(documentStr, Is.EqualTo(orgContent));
            // 0 1 2 3 4 5 6 7 8 9 0 1
            // A B       F J H K L M N
            // 0:2       5:7
            Console.Write($"orgContent: {orgContent}");
            Console.Write($"documentStr:{documentStr}");
            Console.WriteLine("----");
        }
    }

    [Test]
    public void Document_InsertDeleteText_FromFile_Codepoints_Random_Success()
    {
        count count = 10;
        size length = 101;

        string fileName = @"Data\Document_InsertDeleteText_FromFile_Test.txt";
        string loadedContent = File.ReadAllText(fileName);
        loadedContent += Environment.NewLine;
        var orgBytes = File.ReadAllBytes(fileName);

        List<Func<size, size>> actions = new()
            {
                (size length) => length,
                (size length) => length + 1,
                (size length) => length + 7,
                (size length) => length + 8,
                (size length) => length + 9,
                (size length) => length + 11,
                (size length) => length + 17,
                (size length) => length + 23,
                (size length) => length + 56
            };

        for(int j = 0; j < actions.Count; j++)
        {
            var action = actions[j];

            length = 1;

            for(int n = 1; n < count; n++)
            {
                size bufferSize = n;
                string randomStr = string.Empty;
                string operation = "init";
                length = action(length);
                offset offset = 0;

                try
                {
                    var orgContent = loadedContent;
                    //IContent<codepoint> content = LoadContent(fileName, bufferSize);
                    using Document document = (Document)GetDocument(orgContent, bufferSize, loadedContent: default);
                    var documentStr = document.GetString(0);
                    Assert.That(documentStr, Is.EqualTo(orgContent));

                    for(int k = 0; k < count; k++)
                    {
                        if((k % 2) == 0)
                        {
                            operation = "insert";
                            randomStr = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
                            var codepoints = ArtText.GetCodepoints(randomStr);
                            orgContent = orgContent.Insert(randomStr.Length, randomStr);
                            var insertedCount = document.Insert(codepoints.Length, codepoints);
                            Assert.That(insertedCount, Is.EqualTo(codepoints.Length));
                            documentStr = document.GetString(0);
                            Assert.That(documentStr, Is.EqualTo(orgContent));
                        }
                        else
                        {
                            operation = "delete";
                            offset = RandomNumberGenerator.GetInt32(0, length);
                            var lengthToDelete = RandomNumberGenerator.GetInt32(0, Math.Max(0, length / 2));
                            var orgContentBeforeDelete = orgContent;
                            var documentStrBeforeDelete = document.GetString(0);
                            Assert.That(documentStrBeforeDelete, Is.EqualTo(orgContentBeforeDelete));

                            Console.Write($"orgContentBeforeDelete: {orgContentBeforeDelete}");
                            Console.Write($"documentStrBeforeDelete:{documentStrBeforeDelete}");
                            Console.WriteLine($"offset:{offset}, lengthToDelete:{lengthToDelete}");
                            orgContent = orgContent.Remove(offset, lengthToDelete);
                            document.Delete(offset, lengthToDelete);
                            documentStr = document.GetString(0);
                            Console.Write($"orgContent: {orgContent}");
                            Console.Write($"documentStr:{documentStr}");
                            Console.WriteLine("----");

                            try
                            {
                                Assert.That(documentStr, Is.EqualTo(orgContent));
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine($"FIRST: {ex.Message}");
                                throw;
                            }
                        }
                    }

                    documentStr = document.GetString(0);

                    for(int k = 0, m = documentStr.Length; k < m; k++)
                    {
                        var dch = documentStr[k];
                        var cch = document.GetCodepoint(k);
                        if(dch != cch)
                        {
                            Console.WriteLine($"dch:{dch} - cch:{cch}, k:{k}, n:{m}");
                        }
                        Assert.That(dch, Is.EqualTo(cch));
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine($"operation:{operation}, bufferSize:{bufferSize}, var offset:{offset}, length:{length}, randomStr:{randomStr}, randomStr.Length:{randomStr.Length}");
                    j = actions.Count;
                    break;
                }
            }
        }
    }

    [Test]
    public void Document_BuildLineMap_Success()
    {
        size bufferSize = 100;

        IContent<codepoint> content = new DocumentContent("Test Content", "Test Source", bufferSize, "1.0");

        string spanText = "A\nB\r\nC\rD";

        StringDataProvider dataProvider = new(spanText, bufferSize, "1.0");
        dataProvider.Load(content);

        PieceTable pieceTable = new(content, content);

        Piece piece = new(id: 0, new(start: 0, length: spanText.Length), ContentType.Original);

        MethodInfo? buildLineMap = typeof(PieceTable).GetMethod("BuilLineMap", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.That(buildLineMap, Is.Not.Null);

        buildLineMap.Invoke(pieceTable, [piece]);

        var crlf = Regex.Matches(spanText, "\r\n").Count;
        spanText = spanText.Replace("\r\n", "");

        var cr = Regex.Matches(spanText, "\r").Count;
        spanText = spanText.Replace("\r", "");

        var lf = Regex.Matches(spanText, "\n").Count;

        LineMappings lineMappings = piece.LineMappings;
        Assert.That(lineMappings, Is.Not.Null);

        Assert.That(lineMappings.Cr, Is.EqualTo(cr));
        Assert.That(lineMappings.Lf, Is.EqualTo(lf));
        Assert.That(lineMappings.CrLf, Is.EqualTo(crlf));
    }

    [Test]
    public void Document_BuildLineMap_Random_Success()
    {
        count count = 100;
        size length = 10000;

        for(int k = 0; k < count; k++)
        {
            var spanText = ArtText.GetRandomTextWithCrLf(RandomNumberGenerator.GetInt32(0, length));

            var rawText = spanText;

            var crlf = Regex.Matches(rawText, "\r\n").Count;
            rawText = rawText.Replace("\r\n", "");

            var cr = Regex.Matches(rawText, "\r").Count;
            rawText = rawText.Replace("\r", "");

            var lf = Regex.Matches(rawText, "\n").Count;
            rawText = rawText.Replace("\n", "");

            size bufferSize = spanText.Length;

            IContent<codepoint> content = new DocumentContent("Test Content", "Test Source", bufferSize, "1.0");

            StringDataProvider dataProvider = new(spanText, bufferSize, "1.0");
            dataProvider.Load(content);

            PieceTable pieceTable = new(content, content);

            Piece piece = new(id: 0, new(start: 0, length: spanText.Length), ContentType.Original);

            MethodInfo? buildLineMap = typeof(PieceTable).GetMethod("BuilLineMap", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(buildLineMap, Is.Not.Null);

            buildLineMap.Invoke(pieceTable, [piece]);

            LineMappings lineMappings = piece.LineMappings;
            Assert.That(lineMappings, Is.Not.Null);

            Assert.That(lineMappings.Cr, Is.EqualTo(cr));
            Assert.That(lineMappings.Lf, Is.EqualTo(lf));
            Assert.That(lineMappings.CrLf, Is.EqualTo(crlf));
        }
    }

    // search
    private static List<index> GetMatches(string text, string pattern)
    {
        List<index> matches = new();

        if(string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
            return matches;

        for(int index = 0; ; index += pattern.Length)
        {
            index = text.IndexOf(pattern, index);

            if(index == -1)
                break;

            matches.Add(index);
        }

        return matches;
    }

    [Test]
    public void Document_Search_BoyerMoore_Success()
    {
        var bufferSize = 1;

        var content = "";
        var pattern = "";
        Document document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        var documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        var contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "a";
        pattern = "";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "";
        pattern = "a";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "a";
        pattern = "a";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "aa";
        pattern = "a";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "aaa";
        pattern = "a";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "a";
        pattern = "b";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "ab";
        pattern = "aa";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "aba";
        pattern = "a";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "cabdabdab";
        pattern = "abd";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "cabdabdab";
        pattern = "dab";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "XcabdabdabYcabdabdab";
        pattern = "cabdabdab";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "AABAAABCEDBABCDDEBC";
        pattern = "ABC";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "THIS IS A TEST TEXT";
        pattern = "TEST";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        //         0123456789012345
        content = "AABAACAADAABAABA";
        pattern = "AABA";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "ANPANMAN";
        pattern = "PAN";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "AAAAAAAAAAAAAAAAAA";
        pattern = "AAAAA";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "nyoo nyoo";
        pattern = "noyo";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "i";
        pattern = "jlh9f";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "jlh9f";
        pattern = "i";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "i";
        pattern = "ilh9f";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "ilh9f";
        pattern = "i";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "NC4C";
        pattern = "C";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "ababxababyabaca";
        pattern = "ababxababyabaca";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "aabxaayaab";
        pattern = "aabxaayaab";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "aabxaabxcaabxaabxay";
        pattern = "aabxaabxcaabxaabxay";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);

        content = "GuB3W0RwdVW5LhgiHI4BiulDMygwoKoXm0UHmI5WSVDvqUs37yTiO4RZCTxD3J5YWbFqYF8m1gihOh3UcT92NA8bDCUGR0cgE7UqsE30dkUoXJJ6NIyQzR4va1fYnJpS92mSBg3ycW3r22fSCjV3bFk4fNU3RL6GoKpM0i7aA7hpstVymfn9MYLT9McQ3L4wnF7JaQ7eOT1zt06VOdh0oumeuiRLTn05huYRjDvC";
        pattern = "T1zt06VOdh0oumeu";
        document = (Document)GetDocument(content, bufferSize, loadedContent: default);
        documentMatches = document.Find(ArtText.GetCodepoints(pattern));
        contentMatches = GetMatches(content, pattern);
        Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);
    }

    static readonly string[] Texts =
    [
        "",
            "a",
            "",
            "a",
            "aa",
            "aaa",
            "a",
            "ab",
            "aba",
            "cabdabdab",
            "cabdabdab",
            "XcabdabdabYcabdabdab",
            "AABAAABCEDBABCDDEBC",
            "THIS IS A TEST TEXT”",
            "THIS IS A TEST TEXT",
            "AABAACAADAABAABA",
            "AABAACAADAABAABA",
            "ANPANMAN",
            "AAAAAAAAAAAAAAAAAA",
            "nyoo nyoo",
            "jlh9f",
            "i",
            "NC4C",
            "NC4CC",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
            "DAELYMMAKRINAAMRSQ",
            "SWERTAPABCAAPPLOC",
            "AABBCAABBGCAABBAA",
            "needle noodle nargle",
            "ATATATATATATATATAT",
            "There would have been a time for such a word",
            "GCTTCTGCTACCTTTTGCGCGCGCGCGGAA",
            "AYRRQMGRPCRQ",
            "AABAAABCEDBABCDDEBC",
            "AABAAABCEDBABCDDEBC",
            "aababcababbababbaba",
            "aababcababbababbaba",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            "CAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGT",
            "AGGTGTGGAAACAAGCACCTAGATGTGCTGAACCCGGGGCACACGTTCAGTCAGCGACTC",
            "ggccgggccctgtgaccacagtccacatcacaccaggacacagaggaagggccgggccctgtgaccacagtccacatcacaccaggacacagaggaagggccgggcctcatgaccacagt",
            "CABACAABAC"
    ];

    static readonly string[] Patterns =
    [
        "",
            "",
            "a",
            "a",
            "a",
            "a",
            "b",
            "aa",
            "a",
            "abd",
            "dab",
            //            . mismatch
            // 01234567890123456789
            // XcabdabdabYcabdabdab
            //      cabdabdab       need to shift for 6 positions (n - L' - 1)
            //      012345678
            "cabdabdab",
            "ABC",
            "TEST",
            "TEST",
            "AABA",
            "ABAA",
            "PAN",
            "AAAAA",
            "noyo",
            "jlh9f",
            "C",
            "aaaaab",
            "baaaaa",
            "cccccc",
            "pisci",
            "RINA",
            "ABCAAPPLO",
            "AABBAA",
            "needle",
            "ATATATATATATAT",
            "word",
            "CCTTTTGC",
            "RPCRQ",
            "ABC",
            "AC",
            "abbab",
            "abbababbab",
            "alfalfa",
            "aaaaaaaaaaaaaaaaaaaa",
            "CAGTCAG",
            "gtccacatcaca",
            "AABAC"
    ];

    [Test]
    public void Document_Search_BoyerMoore_Naive_Success()
    {
        var maxBufferSize = 100;

        for(size n = 1; n < maxBufferSize; n++)
        {
            var bufferSize = n;

            foreach(var (text, pattern) in Texts.Zip(Patterns))
            {
                try
                {
                    var contentMatches = GetMatches(text, pattern);

                    using Document document = (Document)GetDocument(text, bufferSize, loadedContent: default);
                    var documentMatches = document.Find(ArtText.GetCodepoints(pattern), algorithm: BoyerMoore.ZAlgorithm.Naive);
                    Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);
                }
                catch(Exception ex)
                {
                    var m = ex.Message;
                    Console.WriteLine($"text: {text}");
                    Console.WriteLine($"pattern: {pattern}");
                    break;
                }
            }
        }
    }

    [Test]
    public void Document_Search_BoyerMoore_Gusfield_Success()
    {
        var maxBufferSize = 100;

        for(size n = 1; n < maxBufferSize; n++)
        {
            var bufferSize = n;

            foreach(var (text, pattern) in Texts.Zip(Patterns))
            {
                try
                {
                    var contentMatches = GetMatches(text, pattern);

                    using Document document = (Document)GetDocument(text, bufferSize, loadedContent: default);
                    var documentMatches = document.Find(ArtText.GetCodepoints(pattern), algorithm: BoyerMoore.ZAlgorithm.Gusfield);
                    Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);
                }
                catch(Exception ex)
                {
                    var m = ex.Message;
                    Console.WriteLine($"text: {text}");
                    Console.WriteLine($"pattern: {pattern}");
                    break;
                }
            }
        }
    }

    [Test]
    public void Document_Search_BoyerMoore_Naive_Random_Success()
    {
        count count = 100;
        size length = 100000;

        for(int k = 1; k < count; k++)
        {
            var bufferSize = k;

            var randomText = string.Empty;
            var randomPattern = string.Empty;

            try
            {
                randomText = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
                randomPattern = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(1, 2 + randomText.Length / (k + 1)));
                var contentMatches = GetMatches(randomText, randomPattern);
                using Document document = (Document)GetDocument(randomText, bufferSize, loadedContent: default);
                var documentMatches = document.Find(ArtText.GetCodepoints(randomPattern), algorithm: BoyerMoore.ZAlgorithm.Naive);
                Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);
            }
            catch(Exception ex)
            {
                var m = ex.Message;
                Console.WriteLine($"text: {randomText}");
                Console.WriteLine($"pattern: {randomPattern}");
                break;
            }
        }
    }

    [Test]
    public void Document_Search_BoyerMoore_Gusfield_Random_Success()
    {
        count count = 100;
        size length = 100000;

        for(int k = 1; k < count; k++)
        {
            var bufferSize = k;

            var randomText = string.Empty;
            var randomPattern = string.Empty;

            try
            {
                randomText = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, length));
                randomPattern = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(1, 2 + randomText.Length / (k + 1)));
                var contentMatches = GetMatches(randomText, randomPattern);
                using Document document = (Document)GetDocument(randomText, bufferSize, loadedContent: default);
                var documentMatches = document.Find(ArtText.GetCodepoints(randomPattern), algorithm: BoyerMoore.ZAlgorithm.Gusfield);
                Assert.That(documentMatches.SequenceEqual(contentMatches), Is.True);
            }
            catch(Exception ex)
            {
                var m = ex.Message;
                Console.WriteLine($"text: {randomText}");
                Console.WriteLine($"pattern: {randomPattern}");
                break;
            }
        }
    }

    [Test]
    public void Document_UndoRedo_Success()
    {
        var text = "ABCDEFGH";
        var bufferSize = 1;

        using Document document = (Document)GetDocument(text, bufferSize, loadedContent: default);
    }

    [Test]
    public void Document_UndoRedo_Grouped_Success()
    {
        var text = "ABCDEFGHJ";
        var bufferSize = 1;

        using Document document = (Document)GetDocument(text, bufferSize, loadedContent: default);
    }
}
