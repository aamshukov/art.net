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
}
