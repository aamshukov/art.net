//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Document.History;

public sealed class DocumentHistoryOfflineEntry : DocumentHistoryEntry
{
    public location Location { get; init; }

    public ReadOnlyMemory<codepoint> Content { get; init; }

    public DocumentHistoryOfflineEntry(id group,
                                       location location,
                                       ReadOnlyMemory<codepoint> content,
                                       DocumentEditActionType editActionType,
                                       DocumentHistoryEntryOperation operation,
                                       string? description = default,
                                       bool confirm = false)
            : base(group, editActionType, operation, description, confirm)
    {
        Assert.NonNullReference(content);

        Content = content;
        Location = location;
    }
}
