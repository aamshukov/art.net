//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Document.History;

namespace UILab.Art.Framework.Document.Abstractions;

public interface IDocumentHistory : IDisposable, IAsyncDisposable
{
    id NextGroup();

    void Add(DocumentHistoryEntry entry);

    bool CanUndo();

    IEnumerable<DocumentHistoryEntry> Undo();

    void ResetUndo();

    bool CanRedo();

    IEnumerable<DocumentHistoryEntry> Redo();

    void ResetRedo();

    IReadOnlyList<DocumentHistoryEntry> CreateSnapshot();

    void RestoreSnapshot(IEnumerable<DocumentHistoryEntry> snapshot);
}
