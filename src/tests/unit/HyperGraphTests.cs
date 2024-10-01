//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;

namespace UILab.Art.Tests;

[TestFixture]
internal class HyperGraphTests
{
    private const string DotDirectory = @"d:\tmp\art.graphs.viz";

    [Test]
    public void UndirectedHyperGraph_Create_Vertex_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        UndirectedVertex newVertex = hyperGraph.CreateVertex();
        hyperGraph.AddVertex(newVertex);
        UndirectedVertex? existingVertex = hyperGraph.GetVertex(newVertex.Id);

        Assert.Multiple(() =>
        {
            Assert.That(newVertex, Is.EqualTo(existingVertex));
        });

        newVertex = hyperGraph.CreateVertex();
        hyperGraph.AddVertex(newVertex);
        existingVertex = hyperGraph.RemoveVertex(newVertex.Id, RemoveActionType.Weak);
        Assert.That(newVertex, Is.EqualTo(existingVertex));
    }

    [Test]
    public void UndirectedHyperGraph_Create_HyperEdge_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        UndirectedHyperEdge newEdge = hyperGraph.CreateHyperEdge();
        hyperGraph.AddHyperEdge(newEdge);
        UndirectedHyperEdge? existingEdge = hyperGraph.GetHyperEdge(newEdge.Id);

        Assert.Multiple(() =>
        {
            Assert.That(newEdge, Is.EqualTo(existingEdge));
        });

        newEdge = hyperGraph.CreateHyperEdge();
        hyperGraph.AddHyperEdge(newEdge);
        existingEdge = hyperGraph.RemoveHyperEdge(newEdge.Id, RemoveActionType.Weak);
        Assert.That(newEdge, Is.EqualTo(existingEdge));
    }

    [Test]
    public async Task UndirectedHyperGraph_Create_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB, vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexB, vertexC]);
        hyperGraph.AddHyperEdge(edge2);

        const string fileName = "UndirectedHyperGraph_Create_Success";
        await GraphvizSerialization.SerializeUndirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);
    }

    [Test]
    public void DirectedHyperGraph_Create_Vertex_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        DirectedVertex newVertex = hyperGraph.CreateVertex();
        hyperGraph.AddVertex(newVertex);
        DirectedVertex? existingVertex = hyperGraph.GetVertex(newVertex.Id);

        Assert.Multiple(() =>
        {
            Assert.That(newVertex, Is.EqualTo(existingVertex));
        });

        newVertex = hyperGraph.CreateVertex();
        hyperGraph.AddVertex(newVertex);
        existingVertex = hyperGraph.RemoveVertex(newVertex.Id, RemoveActionType.Weak);
        Assert.That(newVertex, Is.EqualTo(existingVertex));
    }

    [Test]
    public void DirectedHyperGraph_Create_HyperEdge_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        DirectedHyperEdge newEdge = hyperGraph.CreateHyperEdge();
        hyperGraph.AddHyperEdge(newEdge);
        DirectedHyperEdge? existingEdge = hyperGraph.GetHyperEdge(newEdge.Id);

        Assert.Multiple(() =>
        {
            Assert.That(newEdge, Is.EqualTo(existingEdge));
        });

        newEdge = hyperGraph.CreateHyperEdge();
        hyperGraph.AddHyperEdge(newEdge);
        existingEdge = hyperGraph.RemoveHyperEdge(newEdge.Id, RemoveActionType.Weak);
        Assert.That(newEdge, Is.EqualTo(existingEdge));
    }

    [Test]
    public async Task DirectedHyperGraph_Create_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        count n = 0;

        hyperGraph.VertexAdded += (DirectedVertex vertex) =>
        {
            n++;
        };

        hyperGraph.VertexAdded += (DirectedVertex vertex) =>
        {
            n++;
        };

        hyperGraph.VertexRemoved += (DirectedVertex vertex) =>
        {
            n++;
        };

        hyperGraph.VertexRemoved += (DirectedVertex vertex) =>
        {
            n++;
        };

        var vertex1 = hyperGraph.CreateVertex(label: "1");
        hyperGraph.AddVertex(vertex1);
        var vertex2 = hyperGraph.CreateVertex(label: "2");
        hyperGraph.AddVertex(vertex2);
        var vertex3 = hyperGraph.CreateVertex(label: "3");
        hyperGraph.AddVertex(vertex3);
        var vertex4 = hyperGraph.CreateVertex(label: "4");
        hyperGraph.AddVertex(vertex4);
        var vertex5 = hyperGraph.CreateVertex(label: "5");
        hyperGraph.AddVertex(vertex5);
        var vertex6 = hyperGraph.CreateVertex(label: "6");
        hyperGraph.AddVertex(vertex6);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertex1], codomain: [vertex2]);
        hyperGraph.AddHyperEdge(edge1);
        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertex2], codomain: [vertex3]);
        hyperGraph.AddHyperEdge(edge2);
        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertex3], codomain: [vertex1]);
        hyperGraph.AddHyperEdge(edge3);
        var edge4 = hyperGraph.CreateHyperEdge(domain: [vertex2, vertex3], codomain: [vertex4, vertex5]);
        hyperGraph.AddHyperEdge(edge4);
        var edge5 = hyperGraph.CreateHyperEdge(domain: [vertex3, vertex5], codomain: [vertex6]);
        hyperGraph.AddHyperEdge(edge5);

        const string fileName = "DirectedHyperGraph_Create_Success";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);
    }

    [Test]
    public async Task DirectedHyperGraph_Neighbors_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA], codomain: [vertexB, vertexC]);
        hyperGraph.AddHyperEdge(edge1);
        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexB], codomain: [vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        const string fileName = "DirectedHyperGraph_Neighbors_Success";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);

        var outNeighbors = DirectedHyperGraphAlgorithms.GetVertexOutNeighbors(hyperGraph, vertexA).ToArray();
        Assert.That(outNeighbors.SequenceEqual([vertexB, vertexC]), Is.True);

        var inNeighbors = DirectedHyperGraphAlgorithms.GetVertexInNeighbors(hyperGraph, vertexB).ToArray();
        Assert.That(inNeighbors.SequenceEqual([vertexA]), Is.True);

        inNeighbors = DirectedHyperGraphAlgorithms.GetVertexInNeighbors(hyperGraph, vertexD).ToArray();
        Assert.That(inNeighbors.SequenceEqual([vertexB]), Is.True);

        var neighbors = DirectedHyperGraphAlgorithms.GetVertexNeighbors(hyperGraph, vertexB).ToArray();
        Assert.That(neighbors.SequenceEqual([vertexD, vertexA]), Is.True);
    }

    [Test]
    public async Task DirectedHyperGraph_Predecessors_Successors_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA], codomain: [vertexB, vertexC]);
        hyperGraph.AddHyperEdge(edge1);
        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexB], codomain: [vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        const string fileName = "DirectedHyperGraph_Predecessors_Successors_Success";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);

        var predecessors = DirectedHyperGraphAlgorithms.CollectPredecessors(hyperGraph, vertexB).ToArray();
        Assert.That(predecessors.SequenceEqual([vertexA]), Is.True);

        predecessors = DirectedHyperGraphAlgorithms.CollectPredecessors(hyperGraph, vertexD).ToArray();
        Assert.That(predecessors.SequenceEqual([vertexB]), Is.True);

        var successors = DirectedHyperGraphAlgorithms.CollectSuccessors(hyperGraph, vertexA).ToArray();
        Assert.That(successors.SequenceEqual([vertexB, vertexC]), Is.True);

        successors = DirectedHyperGraphAlgorithms.CollectSuccessors(hyperGraph, vertexB).ToArray();
        Assert.That(successors.SequenceEqual([vertexD]), Is.True);
    }

    [Test]
    public void UndirectedHyperGraph_WeakVertexDelete1_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexC]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(vertices: [vertexB, vertexC]);
        hyperGraph.AddHyperEdge(edge3);

        var vertex = hyperGraph.RemoveVertex(vertexA.Id, RemoveActionType.Weak);
        Assert.That(vertex, Is.EqualTo(vertexA));

        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Vertices.Values.SequenceEqual([vertexB]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Vertices.Values.SequenceEqual([vertexC]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[2].Vertices.Values.SequenceEqual([vertexB, vertexC]), Is.True);
        });
    }

    [Test]
    public void UndirectedHyperGraph_WeakVertexDelete2_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexC]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(vertices: [vertexB, vertexC, vertexD]);
        hyperGraph.AddHyperEdge(edge3);

        var vertex = hyperGraph.RemoveVertex(vertexA.Id, RemoveActionType.Weak);
        Assert.That(vertex, Is.EqualTo(vertexA));

        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Vertices.Values.SequenceEqual([vertexB]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Vertices.Values.SequenceEqual([vertexC]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[2].Vertices.Values.SequenceEqual([vertexB, vertexC, vertexD]), Is.True);
        });
    }

    [Test]
    public void UndirectedHyperGraph_StrongVertexDelete1_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexC]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(vertices: [vertexB, vertexD]);
        hyperGraph.AddHyperEdge(edge3);

        var edge4 = hyperGraph.CreateHyperEdge(vertices: [vertexC, vertexD]);
        hyperGraph.AddHyperEdge(edge4);

        var vertex = hyperGraph.RemoveVertex(vertexA.Id, RemoveActionType.Strong);
        Assert.That(vertex, Is.EqualTo(vertexA));

        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Vertices.Values.SequenceEqual([vertexB, vertexD]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Vertices.Values.SequenceEqual([vertexC, vertexD]), Is.True);
        });
    }

    [Test]
    public void UndirectedHyperGraph_StrongVertexDelete2_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexC]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(vertices: [vertexB, vertexC, vertexD]);
        hyperGraph.AddHyperEdge(edge3);

        var vertex = hyperGraph.RemoveVertex(vertexA.Id, RemoveActionType.Strong);
        Assert.That(vertex, Is.EqualTo(vertexA));

        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Vertices.Values.SequenceEqual([vertexB, vertexC, vertexD]), Is.True);
        });
    }

    [Test]
    public void DirectedHyperGraph_WeakVertexDelete1_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA], codomain: [vertexB, vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexB], codomain: [vertexC]);
        hyperGraph.AddHyperEdge(edge2);

        var vertex = hyperGraph.RemoveVertex(vertexA.Id, RemoveActionType.Weak);
        Assert.That(vertex, Is.EqualTo(vertexA));

        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Domain.Values.SequenceEqual([vertexB]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Codomain.Values.SequenceEqual([vertexC]), Is.True);
        });
    }

    [Test]
    public void DirectedHyperGraph_WeakVertexDelete2_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA, vertexB], codomain:[vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexB], codomain:[vertexA, vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexC], codomain:[vertexD]);
        hyperGraph.AddHyperEdge(edge3);

        var vertex = hyperGraph.RemoveVertex(vertexA.Id, RemoveActionType.Weak);
        Assert.That(vertex, Is.EqualTo(vertexA));

        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Domain.Values.SequenceEqual([vertexB]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Codomain.Values.SequenceEqual([vertexC]), Is.True);

            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Domain.Values.SequenceEqual([vertexB]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Codomain.Values.SequenceEqual([vertexD]), Is.True);

            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[2].Domain.Values.SequenceEqual([vertexC]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[2].Codomain.Values.SequenceEqual([vertexD]), Is.True);
        });
    }

    [Test]
    public void DirectedHyperGraph_StrongVertexDelete1_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA, vertexB], codomain:[vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexB], codomain:[vertexA, vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexC], codomain:[vertexD]);
        hyperGraph.AddHyperEdge(edge3);

        var vertex = hyperGraph.RemoveVertex(vertexA.Id, RemoveActionType.Strong);
        Assert.That(vertex, Is.EqualTo(vertexA));

        Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(1));
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Domain.Values.SequenceEqual([vertexC]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Codomain.Values.SequenceEqual([vertexD]), Is.True);
    }

    [Test]
    public void DirectedHyperGraph_StrongVertexDelete2_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA, vertexB], codomain:[vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexC], codomain:[vertexA, vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexB, vertexC], codomain:[vertexE]);
        hyperGraph.AddHyperEdge(edge3);

        var edge4 = hyperGraph.CreateHyperEdge(domain: [vertexD], codomain:[vertexA]);
        hyperGraph.AddHyperEdge(edge4);

        var vertex = hyperGraph.RemoveVertex(vertexA.Id, RemoveActionType.Strong);
        Assert.That(vertex, Is.EqualTo(vertexA));

        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(1));
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Domain.Values.SequenceEqual([vertexB, vertexC]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Codomain.Values.SequenceEqual([vertexE]), Is.True);
        });
    }

    [Test]
    public void UndirectedHyperGraph_WeakEdgeDelete_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexC, vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(vertices: [vertexB, vertexC, vertexE]);
        hyperGraph.AddHyperEdge(edge3);

        var edge = hyperGraph.RemoveHyperEdge(edge2.Id, RemoveActionType.Weak);
        Assert.That(edge, Is.EqualTo(edge2));

        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.Vertices.Values.SequenceEqual([vertexA, vertexB, vertexC, vertexD, vertexE]), Is.True);

            Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(2));

            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0], Is.EqualTo(edge1));
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1], Is.EqualTo(edge3));

            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Vertices.Values.SequenceEqual([vertexA, vertexB]), Is.True);
            Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Vertices.Values.SequenceEqual([vertexB, vertexC, vertexE]), Is.True);
        });
    }

    [Test]
    public async Task UndirectedHyperGraph_StrongEdgeDelete_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexC, vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(vertices: [vertexB, vertexC, vertexE]);
        hyperGraph.AddHyperEdge(edge3);

        string fileName = "UndirectedHyperGraph_StrongEdgeDelete_Success";
        await GraphvizSerialization.SerializeUndirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);

        var edge = hyperGraph.RemoveHyperEdge(edge2.Id, RemoveActionType.Strong);
        Assert.That(edge, Is.EqualTo(edge2));

        fileName = "UndirectedHyperGraph_StrongEdgeDelete_Success-2";
        await GraphvizSerialization.SerializeUndirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);

        Assert.That(hyperGraph.Vertices.Values.SequenceEqual([vertexA, vertexB, vertexC, vertexE]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(2));

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0], Is.EqualTo(edge1));
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1], Is.EqualTo(edge3));

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Vertices.Values.SequenceEqual([vertexA, vertexB]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Vertices.Values.SequenceEqual([vertexB, vertexC, vertexE]), Is.True);
    }

    [Test]
    public async Task DirectedHyperGraph_WeakEdgeDelete_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA, vertexB], codomain:[vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexC], codomain:[vertexA, vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexB], codomain:[vertexE]);
        hyperGraph.AddHyperEdge(edge3);

        string fileName = "DirectedHyperGraph_WeakEdgeDelete_Success";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);

        var edge = hyperGraph.RemoveHyperEdge(edge2.Id, RemoveActionType.Weak);
        Assert.That(edge, Is.EqualTo(edge2));

        fileName = "DirectedHyperGraph_WeakEdgeDelete_Success-2";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);

        Assert.That(hyperGraph.Vertices.Values.SequenceEqual([vertexA, vertexB, vertexC, vertexD, vertexE]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(2));

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0], Is.EqualTo(edge1));
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1], Is.EqualTo(edge3));

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Domain.Values.SequenceEqual([vertexA, vertexB]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Codomain.Values.SequenceEqual([vertexC]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Domain.Values.SequenceEqual([vertexB]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Codomain.Values.SequenceEqual([vertexE]), Is.True);
    }

    [Test]
    public async Task DirectedHyperGraph_StrongEdgeDelete_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA, vertexB], codomain:[vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexC], codomain:[vertexA, vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexB], codomain:[vertexE]);
        hyperGraph.AddHyperEdge(edge3);

        string fileName = "DirectedHyperGraph_StrongEdgeDelete_Success";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);

        var edge = hyperGraph.RemoveHyperEdge(edge2.Id, RemoveActionType.Strong);
        Assert.That(edge, Is.EqualTo(edge2));

        fileName = "DirectedHyperGraph_StrongEdgeDelete_Success-2";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);

        Assert.That(hyperGraph.Vertices.Values.SequenceEqual([vertexA, vertexB, vertexC, vertexE]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(2));

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0], Is.EqualTo(edge1));
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1], Is.EqualTo(edge3));

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Domain.Values.SequenceEqual([vertexA, vertexB]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Codomain.Values.SequenceEqual([vertexC]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Domain.Values.SequenceEqual([vertexB]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Codomain.Values.SequenceEqual([vertexE]), Is.True);
    }

    [Test]
    public void UndirectedHyperGraph_Algorithms_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB, vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexB, vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(vertices: [vertexC, vertexE]);
        hyperGraph.AddHyperEdge(edge3);

        var adjacenciesA = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vertexA).ToArray();
        Assert.That(adjacenciesA.SequenceEqual([vertexB, vertexC]), Is.True);

        var adjacenciesB = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vertexB).ToArray();
        Assert.That(adjacenciesB.SequenceEqual([vertexA, vertexC, vertexD]), Is.True);

        var adjacenciesC = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vertexC).ToArray();
        Assert.That(adjacenciesC.SequenceEqual([vertexA, vertexB, vertexE]), Is.True);

        var adjacenciesD = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vertexD).ToArray();
        Assert.That(adjacenciesD.SequenceEqual([vertexB]), Is.True);

        var adjacenciesE = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vertexE).ToArray();
        Assert.That(adjacenciesE.SequenceEqual([vertexC]), Is.True);

        Assert.That(UndirectedHyperGraphAlgorithms.AreVerticesAdjacent(vertexA, vertexB), Is.True);
        Assert.That(UndirectedHyperGraphAlgorithms.AreVerticesAdjacent(vertexA, vertexD), Is.False);

        Assert.That(UndirectedHyperGraphAlgorithms.GetVertexDegree(vertexA), Is.EqualTo(1));
        Assert.That(UndirectedHyperGraphAlgorithms.GetVertexDegree(vertexB), Is.EqualTo(2));
        Assert.That(UndirectedHyperGraphAlgorithms.GetVertexDegree(vertexC), Is.EqualTo(2));
        Assert.That(UndirectedHyperGraphAlgorithms.GetVertexDegree(vertexD), Is.EqualTo(1));
        Assert.That(UndirectedHyperGraphAlgorithms.GetVertexDegree(vertexE), Is.EqualTo(1));

        var incidentEdgesA = UndirectedHyperGraphAlgorithms.GetVertexIncidentHyperEdges(vertexA).ToArray();
        Assert.That(incidentEdgesA.SequenceEqual([edge1]), Is.True);
        var incidentEdgesB = UndirectedHyperGraphAlgorithms.GetVertexIncidentHyperEdges(vertexB).ToArray();
        Assert.That(incidentEdgesB.SequenceEqual([edge1, edge2]), Is.True);
        var incidentEdgesC = UndirectedHyperGraphAlgorithms.GetVertexIncidentHyperEdges(vertexC).ToArray();
        Assert.That(incidentEdgesC.SequenceEqual([edge1, edge3]), Is.True);
        var incidentEdgesD = UndirectedHyperGraphAlgorithms.GetVertexIncidentHyperEdges(vertexD).ToArray();
        Assert.That(incidentEdgesD.SequenceEqual([edge2]), Is.True);
        var incidentEdgesE = UndirectedHyperGraphAlgorithms.GetVertexIncidentHyperEdges(vertexE).ToArray();
        Assert.That(incidentEdgesE.SequenceEqual([edge3]), Is.True);
    }

    [Test]
    public void DirectedHyperGraph_GetVertexInNeighbors_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA, vertexB], codomain:[vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexB], codomain:[vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexC], codomain:[vertexE]);
        hyperGraph.AddHyperEdge(edge3);

        var inNeighborsA = DirectedHyperGraphAlgorithms.GetVertexInNeighbors(hyperGraph, vertexA).ToArray();
        Assert.That(inNeighborsA.SequenceEqual([]), Is.True);

        var inNeighborsB = DirectedHyperGraphAlgorithms.GetVertexInNeighbors(hyperGraph, vertexB).ToArray();
        Assert.That(inNeighborsB.SequenceEqual([]), Is.True);

        var inNeighborsC = DirectedHyperGraphAlgorithms.GetVertexInNeighbors(hyperGraph, vertexC).ToArray();
        Assert.That(inNeighborsC.SequenceEqual([vertexA, vertexB]), Is.True);

        var inNeighborsD = DirectedHyperGraphAlgorithms.GetVertexInNeighbors(hyperGraph, vertexD).ToArray();
        Assert.That(inNeighborsD.SequenceEqual([vertexB]), Is.True);

        var inNeighborsE = DirectedHyperGraphAlgorithms.GetVertexInNeighbors(hyperGraph, vertexE).ToArray();
        Assert.That(inNeighborsE.SequenceEqual([vertexC]), Is.True);

        var outNeighborsA = DirectedHyperGraphAlgorithms.GetVertexOutNeighbors(hyperGraph, vertexA).ToArray();
        Assert.That(outNeighborsA.SequenceEqual([vertexC]), Is.True);

        var outNeighborsB = DirectedHyperGraphAlgorithms.GetVertexOutNeighbors(hyperGraph, vertexB).ToArray();
        Assert.That(outNeighborsB.SequenceEqual([vertexC, vertexD]), Is.True);

        var outNeighborsC = DirectedHyperGraphAlgorithms.GetVertexOutNeighbors(hyperGraph, vertexC).ToArray();
        Assert.That(outNeighborsC.SequenceEqual([vertexE]), Is.True);

        var outNeighborsD = DirectedHyperGraphAlgorithms.GetVertexOutNeighbors(hyperGraph, vertexD).ToArray();
        Assert.That(outNeighborsD.SequenceEqual([]), Is.True);

        var outNeighborsE = DirectedHyperGraphAlgorithms.GetVertexOutNeighbors(hyperGraph, vertexE).ToArray();
        Assert.That(outNeighborsE.SequenceEqual([]), Is.True);
    }

    [Test]
    public void UndirectedHyperGraph_Algorithms_ContractHyperEdge_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB, vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexB, vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(vertices: [vertexC, vertexE]);
        hyperGraph.AddHyperEdge(edge3);

        var contractedVertex = UndirectedHyperGraphAlgorithms.ContractHyperEdge(hyperGraph, edge1);

        Assert.That(hyperGraph.Vertices.Values.SequenceEqual([vertexD, vertexE, contractedVertex]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(2));
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Vertices.Values.SequenceEqual([contractedVertex, vertexD]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Vertices.Values.SequenceEqual([contractedVertex, vertexE]), Is.True);
    }

    [Test]
    public void DirectedHyperGraph_Algorithms_ContractHyperEdge1_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA, vertexB], codomain:[vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexB], codomain:[vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexC], codomain:[vertexE]);
        hyperGraph.AddHyperEdge(edge3);

        var contractedVertex = DirectedHyperGraphAlgorithms.ContractHyperEdge(hyperGraph, edge1);

        Assert.That(hyperGraph.Vertices.Values.SequenceEqual([vertexD, vertexE, contractedVertex]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(2));

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Domain.Values.SequenceEqual([contractedVertex]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Codomain.Values.SequenceEqual([vertexD]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Domain.Values.SequenceEqual([contractedVertex]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Codomain.Values.SequenceEqual([vertexE]), Is.True);
    }

    [Test]
    public void DirectedHyperGraph_Algorithms_ContractHyperEdge2_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA], codomain:[vertexB, vertexC]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexC], codomain:[vertexD]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexB, vertexD], codomain:[vertexE]);
        hyperGraph.AddHyperEdge(edge3);

        var contractedVertex = DirectedHyperGraphAlgorithms.ContractHyperEdge(hyperGraph, edge1);

        Assert.That(hyperGraph.Vertices.Values.SequenceEqual([vertexD, vertexE, contractedVertex]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(2));

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Domain.Values.SequenceEqual([contractedVertex]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Codomain.Values.SequenceEqual([vertexD]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Domain.Values.SequenceEqual([contractedVertex, vertexD]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Codomain.Values.SequenceEqual([vertexE]), Is.True);
    }

    [Test]
    public void DirectedHyperGraph_Algorithms_ContractHyperEdge3_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexP = hyperGraph.CreateVertex(label: "P");
        hyperGraph.AddVertex(vertexP);
        var vertexQ = hyperGraph.CreateVertex(label: "Q");
        hyperGraph.AddVertex(vertexQ);
        var vertexR = hyperGraph.CreateVertex(label: "R");
        hyperGraph.AddVertex(vertexR);
        var vertexS = hyperGraph.CreateVertex(label: "S");
        hyperGraph.AddVertex(vertexS);
        var vertexT = hyperGraph.CreateVertex(label: "T");
        hyperGraph.AddVertex(vertexT);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexP, vertexQ], codomain:[vertexR, vertexS]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexR], codomain:[vertexT]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexS], codomain:[vertexP]);
        hyperGraph.AddHyperEdge(edge3);

        var contractedVertex = DirectedHyperGraphAlgorithms.ContractHyperEdge(hyperGraph, edge1);

        Assert.That(hyperGraph.Vertices.Values.SequenceEqual([vertexT, contractedVertex]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(2));

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Domain.Values.SequenceEqual([contractedVertex]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Codomain.Values.SequenceEqual([vertexT]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Domain.Values.SequenceEqual([contractedVertex]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Codomain.Values.SequenceEqual([contractedVertex]), Is.True);
    }

    [Test]
    public async Task DirectedHyperGraph_Algorithms_ContractHyperEdge4_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1); // disconnected hypergraph

        var vertexX = hyperGraph.CreateVertex(label: "X");
        hyperGraph.AddVertex(vertexX);
        var vertexY = hyperGraph.CreateVertex(label: "Y");
        hyperGraph.AddVertex(vertexY);
        var vertexZ = hyperGraph.CreateVertex(label: "Z");
        hyperGraph.AddVertex(vertexZ);
        var vertexW = hyperGraph.CreateVertex(label: "W");
        hyperGraph.AddVertex(vertexW);
        var vertexV1 = hyperGraph.CreateVertex(label: "V1");
        hyperGraph.AddVertex(vertexV1);
        var vertexV2 = hyperGraph.CreateVertex(label: "V2");
        hyperGraph.AddVertex(vertexV2);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexX, vertexY], codomain:[vertexZ, vertexW]);
        hyperGraph.AddHyperEdge(edge1);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexZ], codomain:[vertexV1]);
        hyperGraph.AddHyperEdge(edge2);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexW], codomain:[vertexV2]);
        hyperGraph.AddHyperEdge(edge3);

        const string fileName = "DirectedHyperGraph_Algorithms_ContractHyperEdge4_Success";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", hyperGraph);

        var contractedVertex = DirectedHyperGraphAlgorithms.ContractHyperEdge(hyperGraph, edge1);

        Assert.That(hyperGraph.Vertices.Values.SequenceEqual([vertexV1, vertexV2, contractedVertex]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Count, Is.EqualTo(2));

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Domain.Values.SequenceEqual([contractedVertex]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[0].Codomain.Values.SequenceEqual([vertexV1]), Is.True);

        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Domain.Values.SequenceEqual([contractedVertex]), Is.True);
        Assert.That(hyperGraph.HyperEdges.Values.ToArray()[1].Codomain.Values.SequenceEqual([vertexV2]), Is.True);
    }

    [Test]
    public void UndirectedHyperGraph_Algorithms_GetSelfLoopVertices1_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);
        var vertexD = hyperGraph.CreateVertex(label: "D");
        hyperGraph.AddVertex(vertexD);
        var vertexE = hyperGraph.CreateVertex(label: "E");
        hyperGraph.AddVertex(vertexE);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB, vertexC]);
        var selfLoopVertices = UndirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge1).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([]), Is.True);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexB, vertexD]);
        selfLoopVertices = UndirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge2).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([]), Is.True);

        var edge3 = hyperGraph.CreateHyperEdge(vertices: [vertexA]);
        selfLoopVertices = UndirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge3).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([vertexA]), Is.True);
    }

    [Test]
    public void UndirectedHyperGraph_Algorithms_GetSelfLoopVertices2_Success()
    {
        using UndirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexAc = hyperGraph.CloneVertex(vertexA); //??
        hyperGraph.AddVertex(vertexAc);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);

        var edge1 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexB, vertexC]);
        var selfLoopVertices = UndirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge1).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([]), Is.True);

        var edge2 = hyperGraph.CreateHyperEdge(vertices: [vertexB]);
        selfLoopVertices = UndirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge2).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([vertexB]), Is.True);

        // vertexA, vertexA is prohibitted, no duplications
        //var edge3 = hyperGraph.CreateHyperEdge(vertices: [vertexA, vertexA]);
        //selfLoopVertices = UndirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge3).ToArray();
        //Assert.That(selfLoopVertices.SequenceEqual([vertexA]), Is.True);
    }

    [Test]
    public void DirectedHyperGraph_Algorithms_GetSelfLoopVertices1_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexA = hyperGraph.CreateVertex(label: "A");
        hyperGraph.AddVertex(vertexA);
        var vertexB = hyperGraph.CreateVertex(label: "B");
        hyperGraph.AddVertex(vertexB);
        var vertexC = hyperGraph.CreateVertex(label: "C");
        hyperGraph.AddVertex(vertexC);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexA], codomain:[vertexA]);
        var selfLoopVertices = DirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge1).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([vertexA]), Is.True);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexB], codomain:[vertexC]);
        selfLoopVertices = DirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge2).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([]), Is.True);
    }

    [Test]
    public void DirectedHyperGraph_Algorithms_GetSelfLoopVertices2_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexX = hyperGraph.CreateVertex(label: "X");
        hyperGraph.AddVertex(vertexX);
        var vertexY = hyperGraph.CreateVertex(label: "Y");
        hyperGraph.AddVertex(vertexY);
        var vertexZ = hyperGraph.CreateVertex(label: "Z");
        hyperGraph.AddVertex(vertexZ);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexX], codomain:[vertexY, vertexZ]);
        var selfLoopVertices = DirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge1).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([]), Is.True);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexZ], codomain:[vertexZ]);
        selfLoopVertices = DirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge2).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([vertexZ]), Is.True);
    }

    [Test]
    public void DirectedHyperGraph_Algorithms_GetSelfLoopVertices3_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexP = hyperGraph.CreateVertex(label: "P");
        hyperGraph.AddVertex(vertexP);
        var vertexQ = hyperGraph.CreateVertex(label: "Q");
        hyperGraph.AddVertex(vertexQ);
        var vertexR = hyperGraph.CreateVertex(label: "R");
        hyperGraph.AddVertex(vertexR);
        var vertexS = hyperGraph.CreateVertex(label: "S");
        hyperGraph.AddVertex(vertexS);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexP], codomain:[vertexQ]);
        var selfLoopVertices = DirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge1).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([]), Is.True);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexQ], codomain:[vertexQ]);
        selfLoopVertices = DirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge2).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([vertexQ]), Is.True);

        var edge3 = hyperGraph.CreateHyperEdge(domain: [vertexR, vertexS], codomain:[vertexS]);
        selfLoopVertices = DirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge3).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([vertexS]), Is.True);

        var edge4 = hyperGraph.CreateHyperEdge(domain: [vertexS], codomain:[vertexR, vertexS]);
        selfLoopVertices = DirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge4).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([vertexS]), Is.True);
    }

    [Test]
    public void DirectedHyperGraph_Algorithms_GetSelfLoopVertices4_Success()
    {
        using DirectedHyperGraph hyperGraph = new(1);

        var vertexX = hyperGraph.CreateVertex(label: "X");
        hyperGraph.AddVertex(vertexX);
        var vertexY = hyperGraph.CreateVertex(label: "Y");
        hyperGraph.AddVertex(vertexY);
        var vertexZ = hyperGraph.CreateVertex(label: "Z");
        hyperGraph.AddVertex(vertexZ);
        var vertexW = hyperGraph.CreateVertex(label: "W");
        hyperGraph.AddVertex(vertexW);

        var edge1 = hyperGraph.CreateHyperEdge(domain: [vertexX, vertexY], codomain:[vertexZ]);
        var selfLoopVertices = DirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge1).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([]), Is.True);

        var edge2 = hyperGraph.CreateHyperEdge(domain: [vertexZ, vertexW], codomain:[vertexZ, vertexW]);
        selfLoopVertices = DirectedHyperGraphAlgorithms.GetSelfLoopVertices(edge2).ToArray();
        Assert.That(selfLoopVertices.SequenceEqual([vertexZ, vertexW]), Is.True);
    }
}
