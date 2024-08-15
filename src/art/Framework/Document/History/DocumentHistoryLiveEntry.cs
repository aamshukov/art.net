//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using Art.Framework.Document.PieceTable;
using UILab.Art.Framework.Core.Diagnostics;

namespace Art.Framework.Document.History;

public sealed class DocumentHistoryLiveEntry : DocumentHistoryEntry
{
    /// <summary>
    /// Gets list of pieces' nodes.
    /// Represents a single modification to the document.
    /// Live pieces, valid during editing.
    /// </summary>
    public List<ListNodeProxy> Pieces { get; init; }

    public DocumentHistoryLiveEntry(id group,
                                    List<ListNodeProxy> pieces,
                                    DocumentEditActionType editActionType,
                                    DocumentHistoryEntryOperation operation,
                                    string? description = default,
                                    bool confirm = false)
            : base(group, editActionType, operation, description, confirm)
    {
        Assert.NonNullReference(pieces, nameof(pieces));
        Pieces = pieces;
    }
}
