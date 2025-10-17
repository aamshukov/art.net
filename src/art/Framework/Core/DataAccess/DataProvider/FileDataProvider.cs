//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Content.Abstractions;
using UILab.Art.Framework.Core.DataAccess.DataProvider.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.DataAccess.DataProvider;

public sealed class FileDataProvider : IStringDataProvider<codepoint>
{
    public string FileName { get; init; }

    public string? Version { get; init; }

    public FileDataProvider(string fileName, string? version = default)
    {
        Assert.NonEmptyString(fileName);
        Assert.Ensure(File.Exists(fileName), nameof(fileName));

        FileName = fileName;
        Version = version?.Trim();
    }

    public void Load(IContent<codepoint> content)
    {
        Assert.NonNullReference(content);

        count nakedCount = 0;
        count crlfCount = 0;
        count lineCount = 0;

        using var stream = new StreamReader(FileName, detectEncodingFromByteOrderMarks: true);

        while (true)
        {
            string? line = stream.ReadLine();

            if (line is null)
            {
                break;
            }

            nakedCount += line.Length;
            line = $"{line}{Environment.NewLine}";
            crlfCount += line.Length;
            lineCount++;

            ReadOnlyMemory<codepoint> codepoints = UILab.Art.Framework.Core.Text.Text.GetCodepoints(line);

            content.AppendContent(codepoints);
        }
    }

    public async Task LoadAsync(IContent<codepoint> content, CancellationToken cancellationToken)
    {
        Assert.NonNullReference(content);

        Load(content);
        await Task.CompletedTask;
    }
}
