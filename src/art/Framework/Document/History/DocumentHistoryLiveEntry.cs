//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Document.History;

public sealed class DocumentHistoryLiveEntry : DocumentHistoryEntry
{
    public id InjectionPoint { get; init; }

    /// <summary>
    /// Gets list of pieces to be added.
    /// Represents a single modification to the document.
    /// Live pieces, valid during editing.
    /// </summary>
    public List<Piece> AddPieces { get; set; }

    /// <summary>
    /// Gets list of pieces to be removed.
    /// Represents a single modification to the document.
    /// Live pieces, valid during editing.
    /// </summary>
    public List<Piece> RemovePieces { get; set; }

    public DocumentHistoryLiveEntry(id group,
                                    id injectionPoint,
                                    List<Piece> addPieces,
                                    List<Piece> removePieces,
                                    DocumentEditActionType editActionType,
                                    DocumentHistoryEntryOperation operation,
                                    string? description = default,
                                    bool confirm = false)
            : base(group, editActionType, operation, description, confirm)
    {
        Assert.NonNullReference(addPieces, nameof(addPieces));
        Assert.NonNullReference(removePieces, nameof(removePieces));

        InjectionPoint = injectionPoint;
        AddPieces = addPieces;
        RemovePieces = removePieces;
    }
}
