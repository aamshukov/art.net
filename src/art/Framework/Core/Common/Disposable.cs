﻿//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;

namespace UILab.Art.Framework.Core;

public abstract class Disposable : IDisposable, IAsyncDisposable
{
    private readonly object syncRoot = new();

    public bool Disposed { get; private set; }

    protected Disposable()
    {
        Disposed = false;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>")]
    [SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "<Pending>")]
    protected virtual void Dispose(bool disposing)
    {
        if(!Disposed)
        {
            lock(syncRoot)
            {
                try
                {
                    if(!Disposed)
                    {
                        // managed resources
                        if(disposing)
                        {
                            DisposeManagedResources();
                        }

                        // unmanaged resources
                        DisposeUnmanagedResources();
                    }
                }
                catch
                {
                }
                finally
                {
                    Disposed = true;
                }
            }
        }

        // base.Dispose(disposing);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>")]
    [SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "<Pending>")]
    protected virtual async ValueTask DisposeAsyncCore()
    {
        try
        {
            if(!Disposed)
            {
                // managed resources
                await DisposeManagedResourcesAsync().ConfigureAwait(false);

                // unmanaged resources
                await DisposeUnmanagedResourcesAsync().ConfigureAwait(false);
            }
        }
        catch
        {
        }
        finally
        {
            Disposed = true;
        }

        await ValueTask.CompletedTask;
        // base.DisposeAsyncCore();
    }

    protected abstract void DisposeManagedResources();

    protected abstract ValueTask DisposeManagedResourcesAsync();

    protected abstract void DisposeUnmanagedResources();

    protected abstract ValueTask DisposeUnmanagedResourcesAsync();
}
