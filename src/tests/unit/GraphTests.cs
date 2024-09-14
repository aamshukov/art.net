//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;

namespace UILab.Art.Tests;

[TestFixture]
internal class GraphTests
{
    [Test]
    public void Hypergraph_Create_Vertex_Success()
    {
        HyperGraph<Vertex<string>, Edge<string>> hyperGraph = new(1);

        var newVertex = hyperGraph.CreateVertex<string>();
        hyperGraph.AddVertex(newVertex);
        var existingVertex = hyperGraph.GetVertex(newVertex.Id);

        Assert.Multiple(() =>
        {
            Assert.That(newVertex, Is.EqualTo(existingVertex));
            Assert.That(hyperGraph.TryGetVertex(newVertex.Id, out Vertex<string>? existingVertex2), Is.True);
            Assert.That(newVertex, Is.EqualTo(existingVertex2));
        });

        newVertex = hyperGraph.CreateVertex<string>();
        hyperGraph.AddVertex(newVertex);
        existingVertex = hyperGraph.RemoveVertex(newVertex.Id);
        Assert.That(newVertex, Is.EqualTo(existingVertex));

        newVertex = hyperGraph.CreateVertex<string>();
        hyperGraph.AddVertex(newVertex);
        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.TryRemoveVertex(newVertex.Id, out Vertex<string>? existingVertex2), Is.True);
            Assert.That(newVertex, Is.EqualTo(existingVertex2));
        });
    }

    [Test]
    public void Hypergraph_Create_Edge_Success()
    {
        HyperGraph<Vertex<string>, Edge<string>> hyperGraph = new(1);

        var newEdge = hyperGraph.CreateEdge<string>();
        hyperGraph.AddEdge(newEdge);
        var existingEdge = hyperGraph.GetEdge(newEdge.Id);

        Assert.Multiple(() =>
        {
            Assert.That(newEdge, Is.EqualTo(existingEdge));
            Assert.That(hyperGraph.TryGetEdge(newEdge.Id, out Edge<string>? existingEdge2), Is.True);
            Assert.That(newEdge, Is.EqualTo(existingEdge2));
        });

        newEdge = hyperGraph.CreateEdge<string>();
        hyperGraph.AddEdge(newEdge);
        existingEdge = hyperGraph.RemoveEdge(newEdge.Id);
        Assert.That(newEdge, Is.EqualTo(existingEdge));

        newEdge = hyperGraph.CreateEdge<string>();
        hyperGraph.AddEdge(newEdge);
        Assert.Multiple(() =>
        {
            Assert.That(hyperGraph.TryRemoveEdge(newEdge.Id, out Edge<string>? existingEdge2), Is.True);
            Assert.That(newEdge, Is.EqualTo(existingEdge2));
        });
    }

    [Test]
    public void Hypergraph_Create_Undirectional_HyperEdge_Success()
    {
        HyperGraph<Vertex<string>, Edge<string>> hyperGraph = new(1);

        var newEdge = hyperGraph.CreateEdge<string>(label: "Edge");

        var newVertexA = hyperGraph.CreateVertex<string>(label: "A");
        hyperGraph.AddVertex(newVertexA);
        var newVertexB = hyperGraph.CreateVertex<string>(label: "B");
        hyperGraph.AddVertex(newVertexB);
        var newVertexC = hyperGraph.CreateVertex<string>(label: "C");
        hyperGraph.AddVertex(newVertexC);

        var domain = new List<Vertex<string>>() { newVertexA, newVertexB, newVertexC };
        var hyperEdge = hyperGraph.CreateHyperEdge(newEdge, domain);

        Assert.Multiple(() =>
        {
            Assert.That(hyperEdge.U, Is.EqualTo(newVertexA));
            Assert.That(hyperEdge.V, Is.EqualTo(default));
        });
    }
}
