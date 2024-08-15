//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Content;

namespace UILab.Art.Framework.Document;

public class DocumentContent : Content<codepoint>
{
    public DocumentContent(string id,
                           string source,
                           size bufferSize,
                           string? version = default) : base(id, source, bufferSize, version)
    {
    }
}
