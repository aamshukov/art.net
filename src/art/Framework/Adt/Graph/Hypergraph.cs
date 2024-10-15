//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;
using UILab.Art.Framework.Core.Counter;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Graph;

public abstract class HyperGraph<TVertex, TEdge> : EntityType<id>, IDisposable, IAsyncDisposable
    where TVertex : Vertex
    where TEdge : HyperEdge<TVertex>
{
    protected readonly object syncRoot = new();

    public string Label { get; init; }

    public Flags Flags { get; set; }

    public Color Color { get; set; }

    public Dictionary<string, object> Attributes { get; init; }

    public Direction Direction { get; init; }

    public Dictionary<id, TVertex> Vertices { get; init; }

    protected Counter VertexCounter { get; init; }

    public event VertexEventHandler<TVertex>? VertexAdded;

    public event VertexEventHandler<TVertex>? VertexRemoved;

    public bool Disposed { get; private set; }

    public HyperGraph(id id,
                      string? label = default,
                      Flags flags = Flags.Clear,
                      Color color = Color.Unknown,
                      Direction direction = Direction.Undirectional,
                      Dictionary<string, object>? attributes = default,
                      string? version = default) : base(id, version)
    {
        Label = label?.Trim() ?? $"G:{id.ToString()}";
        Flags = flags;
        Color = color;
        Attributes = attributes ?? new();
        Direction = direction;
        Vertices = new();
        VertexCounter = new();
    }

    public Type VertexType => typeof(TVertex);

    public TVertex? GetVertex(id id)
    {
        Assert.NonDisposed(Disposed);

        if(Vertices.TryGetValue(id, out TVertex? vertex))
            return vertex;
        return default;
    }

    public void AddVertex(TVertex vertex)
    {
        Assert.NonDisposed(Disposed);
        Assert.NonNullReference(vertex, nameof(vertex));
        Assert.Ensure(!Vertices.ContainsKey(vertex.Id), nameof(vertex));

        Vertices.Add(vertex.Id, vertex);
        OnVertexAdded(vertex);
    }

    protected virtual void OnVertexAdded(TVertex vertex)
    {
        Assert.NonDisposed(Disposed);
        Assert.NonNullReference(vertex, nameof(vertex));

        VertexAdded?.Invoke(vertex);
    }

    public TVertex? RemoveVertex(id id)
    {
        Assert.NonDisposed(Disposed);

        if(Vertices.TryGetValue(id, out TVertex? vertex))
        {
            if(vertex.CanRelease())
            {
                Vertices.Remove(id);
                OnVertexRemoved(vertex);
            }
        }

        return vertex;
    }

    protected virtual void OnVertexRemoved(TVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        VertexRemoved?.Invoke(vertex);
    }

    public void Cleanup()
    {
        Assert.NonDisposed(Disposed);

        var verticesToRemove = Vertices.Values.Where(vertex => vertex.CanRelease()).Select(vertex => vertex.Id);

        foreach(id id in verticesToRemove)
        {
            Vertices.Remove(id);
        }
    }

    public void ResetFlags(Flags add = Flags.Clear, Flags remove = Flags.Clear)
    {
        Assert.NonDisposed(Disposed);

        foreach(TVertex vertex in Vertices.Values)
        {
            vertex.Flags = HyperGraphAlgorithms.ModifyFlags(vertex.Flags, add, remove);
        }
    }

    public void ResetColor()
    {
        Assert.NonDisposed(Disposed);

        foreach(TVertex vertex in Vertices.Values)
        {
            vertex.Color = Color.Unknown;
        }
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        Assert.NonDisposed(Disposed);

        foreach(var component in base.GetEqualityComponents())
            yield return component;

        yield return Label;
        yield return Vertices;
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    [SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "<Pending>")]
    protected virtual async ValueTask DisposeAsyncCore()
    {
        try
        {
            if(!Disposed)
            {
                // managed resources
                if(VertexAdded is not null)
                {
                    Delegate.RemoveAll(VertexAdded, VertexAdded);
                    VertexAdded = null;
                }

                if(VertexRemoved is not null)
                {
                    Delegate.RemoveAll(VertexRemoved, VertexRemoved);
                    VertexRemoved = null;
                }

                // unmanaged resources
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
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    [SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S1066:Mergeable \"if\" statements should be combined", Justification = "<Pending>")]
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
                            if(VertexAdded is not null)
                            {
                                Delegate.RemoveAll(VertexAdded, VertexAdded);
                                VertexAdded = null;
                            }

                            if(VertexRemoved is not null)
                            {
                                Delegate.RemoveAll(VertexRemoved, VertexRemoved);
                                VertexRemoved = null;
                            }
                        }

                        // unmanaged resources
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
    }
}
