//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core;
using UILab.Art.Framework.Document.Abstractions;

namespace Art.Framework.Document.History;

public sealed class DocumentHistory : Disposable, IDocumentHistory
{
    private Stack<DocumentHistoryEntry> UndoStack { get; init; }

    private Stack<DocumentHistoryEntry> RedoStack { get; init; }

    public const id DefaultGroup = 0;

    private id SeedGroup = DefaultGroup + 1;

    public id NextGroup() => SeedGroup++;

    private size MaxHistorySize { get; init; }

    public DocumentHistory(size historySize = size.MaxValue)
    {
        MaxHistorySize = historySize;

        UndoStack = new();
        RedoStack = new();
    }

    public void Add(DocumentHistoryEntry entry)
    {
        if(UndoStack.Count >= MaxHistorySize || RedoStack.Count >= MaxHistorySize)
        {
            // cheapest for now ...
            ResetUndo();
            ResetRedo();
        }

        UndoStack.Push(entry);
        ResetRedo();
    }

    public bool CanUndo()
    {
        return UndoStack.Count > 0;
    }

    public IEnumerable<DocumentHistoryEntry> Undo()
    {
        if(CanUndo())
        {
            id group;

            DocumentHistoryEntry entry;

            do
            {
                entry = UndoStack.Pop();
                group = entry.Group;

                yield return entry;

                entry.Operation = DocumentHistoryEntryOperation.Redo;

                RedoStack.Push(entry);

                if(CanUndo())
                    entry = UndoStack.Peek();
                else
                    break;
            }
            while(group != DefaultGroup && group == entry.Group);
        }
    }

    public void ResetUndo()
    {
        UndoStack.Clear();
    }

    public bool CanRedo()
    {
        return RedoStack.Count > 0;
    }

    public IEnumerable<DocumentHistoryEntry> Redo()
    {
        if(CanRedo())
        {
            id group;

            DocumentHistoryEntry entry;

            do
            {
                entry = RedoStack.Pop();
                group = entry.Group;

                yield return entry;

                entry.Operation = DocumentHistoryEntryOperation.Undo;

                UndoStack.Push(entry);

                if(CanRedo())
                    entry = RedoStack.Peek();
                else
                    break;
            }
            while(group != DefaultGroup && group == entry.Group);
        }
    }

    public void ResetRedo()
    {
        RedoStack.Clear();
    }

    public IReadOnlyList<DocumentHistoryEntry> CreateSnapshot()
    {
        List<DocumentHistoryEntry> snapshot = new();

        snapshot.AddRange(UndoStack);
        snapshot.AddRange(RedoStack);

        return snapshot;
    }

    /// <summary>
    /// Restores the snapshot from deserialized data.
    /// Must be called after the document fully loaded, to restore internal list of pieces (nodes).
    /// </summary>
    /// <param name="snapshot"></param>
    public void RestoreSnapshot(IEnumerable<DocumentHistoryEntry> snapshot)
    {
    }

    protected override void DisposeManagedResources()
    {
        ResetUndo();
        ResetRedo();
    }

    protected async override ValueTask DisposeManagedResourcesAsync()
    {
        ResetUndo();
        ResetRedo();
        await ValueTask.CompletedTask;
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    protected async override ValueTask DisposeUnmanagedResourcesAsync()
    {
        await ValueTask.CompletedTask;
    }
}
