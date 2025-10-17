//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
// See for more details:
//  https://blog.stephencleary.com/2023/09/memory-mapped-files-overlaid-structs.html
//  https://blog.stephencleary.com/2023/10/padding-for-overlaid-structs.html
//
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.DataAccess.Storage.Mmf;

public sealed unsafe class MmfCursor : Disposable
{
    private MemoryMappedViewAccessor View { get; init; }

    private byte* Pointer { get; init; }

    public MmfCursor(MemoryMappedViewAccessor view)
    {
        Assert.NonNullReference(view);

        View = view;

        byte* pointer = default;
        view.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);

        Pointer = pointer;
    }

    public ref T As<T>() where T : struct => ref Unsafe.AsRef<T>(Pointer);

    protected override void DisposeManagedResources()
    {
        View.SafeMemoryMappedViewHandle.ReleasePointer();
    }

    protected override ValueTask DisposeManagedResourcesAsync()
    {
        DisposeManagedResources();
        return default;
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    protected override ValueTask DisposeUnmanagedResourcesAsync()
    {
        return default;
    }
}
