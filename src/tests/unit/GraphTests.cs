//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics;
using UILab.Art.Framework.Adt.Graph;

namespace UILab.Art.Tests;

[TestFixture]
internal class GraphTests
{
    private const string DotDirectory = @"d:\tmp\art.graphs.viz";

    [Test]
    public void UndirectedHyperGraph_Create_Vertex_Success()
    {
        UndirectedHyperGraph hyperGraph = new(1);

        UndirectedVertex newVertex = hyperGraph.CreateVertex();
        hyperGraph.AddVertex(newVertex);
        UndirectedVertex? existingVertex = hyperGraph.GetVertex(newVertex.Id);

        Assert.Multiple(() =>
        {
            Assert.That(newVertex, Is.EqualTo(existingVertex));
            Assert.That(hyperGraph.TryGetVertex(newVertex.Id, out UndirectedVertex? existingVertex2), Is.True);
            Assert.That(newVertex, Is.EqualTo(existingVertex2));
        });

        newVertex = hyperGraph.CreateVertex();
        hyperGraph.AddVertex(newVertex);
        existingVertex = hyperGraph.RemoveVertex(newVertex.Id, weak: false);
        Assert.That(newVertex, Is.EqualTo(existingVertex));

        newVertex = hyperGraph.CreateVertex();
        hyperGraph.AddVertex(newVertex);
        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.TryRemoveVertex(newVertex.Id, out UndirectedVertex? existingVertex2, weak: false), Is.True);
            Assert.That(newVertex, Is.EqualTo(existingVertex2));
        });
    }

    [Test]
    public void UndirectedHyperGraph_Create_HyperEdge_Success()
    {
        UndirectedHyperGraph hyperGraph = new(1);

        UndirectedHyperEdge newEdge = hyperGraph.CreateHyperEdge();
        hyperGraph.AddHyperEdge(newEdge);
        UndirectedHyperEdge? existingEdge = hyperGraph.GetHyperEdge(newEdge.Id);

        Assert.Multiple(() =>
        {
            Assert.That(newEdge, Is.EqualTo(existingEdge));
            Assert.That(hyperGraph.TryGetEdge(newEdge.Id, out UndirectedHyperEdge? existingEdge2), Is.True);
            Assert.That(newEdge, Is.EqualTo(existingEdge2));
        });

        newEdge = hyperGraph.CreateHyperEdge();
        hyperGraph.AddHyperEdge(newEdge);
        existingEdge = hyperGraph.RemoveHyperEdge(newEdge.Id);
        Assert.That(newEdge, Is.EqualTo(existingEdge));

        newEdge = hyperGraph.CreateHyperEdge();
        hyperGraph.AddHyperEdge(newEdge);
        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.TryRemoveHyperEdge(newEdge.Id, out UndirectedHyperEdge? existingEdge2), Is.True);
            Assert.That(newEdge, Is.EqualTo(existingEdge2));
        });
    }

    [Test]
    public async Task UndirectedHyperGraph_Create_Success()
    {
        UndirectedHyperGraph hyperGraph = new(1);

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
        DirectedHyperGraph hyperGraph = new(1);

        DirectedVertex newVertex = hyperGraph.CreateVertex();
        hyperGraph.AddVertex(newVertex);
        DirectedVertex? existingVertex = hyperGraph.GetVertex(newVertex.Id);

        Assert.Multiple(() =>
        {
            Assert.That(newVertex, Is.EqualTo(existingVertex));
            Assert.That(hyperGraph.TryGetVertex(newVertex.Id, out DirectedVertex? existingVertex2), Is.True);
            Assert.That(newVertex, Is.EqualTo(existingVertex2));
        });

        newVertex = hyperGraph.CreateVertex();
        hyperGraph.AddVertex(newVertex);
        existingVertex = hyperGraph.RemoveVertex(newVertex.Id, weak: false);
        Assert.That(newVertex, Is.EqualTo(existingVertex));

        newVertex = hyperGraph.CreateVertex();
        hyperGraph.AddVertex(newVertex);
        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.TryRemoveVertex(newVertex.Id, out DirectedVertex? existingVertex2, weak: false), Is.True);
            Assert.That(newVertex, Is.EqualTo(existingVertex2));
        });
    }

    [Test]
    public void DirectedHyperGraph_Create_HyperEdge_Success()
    {
        DirectedHyperGraph hyperGraph = new(1);

        DirectedHyperEdge newEdge = hyperGraph.CreateHyperEdge();
        hyperGraph.AddHyperEdge(newEdge);
        DirectedHyperEdge? existingEdge = hyperGraph.GetHyperEdge(newEdge.Id);

        Assert.Multiple(() =>
        {
            Assert.That(newEdge, Is.EqualTo(existingEdge));
            Assert.That(hyperGraph.TryGetEdge(newEdge.Id, out DirectedHyperEdge? existingEdge2), Is.True);
            Assert.That(newEdge, Is.EqualTo(existingEdge2));
        });

        newEdge = hyperGraph.CreateHyperEdge();
        hyperGraph.AddHyperEdge(newEdge);
        existingEdge = hyperGraph.RemoveHyperEdge(newEdge.Id);
        Assert.That(newEdge, Is.EqualTo(existingEdge));

        newEdge = hyperGraph.CreateHyperEdge();
        hyperGraph.AddHyperEdge(newEdge);
        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.TryRemoveHyperEdge(newEdge.Id, out DirectedHyperEdge? existingEdge2), Is.True);
            Assert.That(newEdge, Is.EqualTo(existingEdge2));
        });
    }

    [Test]
    public async Task DirectedHyperGraph_Create_Success()
    {
        DirectedHyperGraph hyperGraph = new(1);

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
        DirectedHyperGraph hyperGraph = new(1);

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
        DirectedHyperGraph hyperGraph = new(1);

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
}
