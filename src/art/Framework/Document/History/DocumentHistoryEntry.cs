//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Document.History;

public abstract class DocumentHistoryEntry
{
    /// <summary>
    /// Gets group id.
    /// All entries with the same group id (excluding DefaultGroup) are treated as one operation.
    /// </summary>
    public id Group { get; init; }

    public DocumentEditActionType EditActionType { get; init; }

    public DocumentHistoryEntryOperation Operation { get; set; }

    public string? Description { get; init; }

    public bool Confirm { get; init; }

    /// <summary>
    /// Gets timestamp.
    /// For travel machine abstraction.
    /// </summary>
    public string Timestamp { get; init; }

    protected DocumentHistoryEntry(id group,
                                   DocumentEditActionType editActionType,
                                   DocumentHistoryEntryOperation operation,
                                   string? description = default,
                                   bool confirm = false)
    {
        Group = group;

        EditActionType = editActionType;
        Operation = operation;

        Description = description;
        Confirm = confirm;

        Timestamp = DomainHelper.Timestamp();
    }
}
