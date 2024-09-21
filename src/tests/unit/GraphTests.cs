//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;

namespace UILab.Art.Tests;

[TestFixture]
internal class GraphTests
{
    private const string DotDirectory = @"d:\tmp\art.graphs.viz";

    [Test]
    public void UndirectedGraph_Create_Success()
    {
        Graph<UndirectedVertex, UndirectedEdge> graph = new(1);

        UndirectedVertex undirectedVertex = graph.CreateVertex(digraph: false);
    }

    [Test]
    public void DirectedGraph_Create_Success()
    {
        Graph<DirectedVertex, DirectedEdge> graph = new(1);

        DirectedVertex directedVertex = graph.CreateVertex(digraph: true);
    }
}
