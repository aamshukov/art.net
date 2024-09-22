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
        UndirectedGraph graph = new(1);

        var vA = graph.CreateVertex(label: "A");
        graph.AddVertex(vA);
        var vB = graph.CreateVertex(label: "B");
        graph.AddVertex(vB);
        var vC = graph.CreateVertex(label: "C");
        graph.AddVertex(vC);
        var vD = graph.CreateVertex(label: "D");
        graph.AddVertex(vD);
        var vE = graph.CreateVertex(label: "E");
        graph.AddVertex(vE);

        var e1 = graph.CreateEdge(u: vA, v: vB);
        graph.AddEdge(e1);

        var e2 = graph.CreateEdge(u: vA, v: vB);
        graph.AddEdge(e2);

        var e3 = graph.CreateEdge(u: vA, v: vB);
        graph.AddEdge(e3);
    }

    [Test]
    public void DirectedGraph_Create_Success()
    {
        DirectedGraph graph = new(1);

        var vA = graph.CreateVertex(label: "A");
        graph.AddVertex(vA);
        var vB = graph.CreateVertex(label: "B");
        graph.AddVertex(vB);
        var vC = graph.CreateVertex(label: "C");
        graph.AddVertex(vC);
        var vD = graph.CreateVertex(label: "D");
        graph.AddVertex(vD);
        var vE = graph.CreateVertex(label: "E");
        graph.AddVertex(vE);

        var e1 = graph.CreateEdge(u: vA, v: vB);
        graph.AddEdge(e1);

        var e2 = graph.CreateEdge(u: vA, v: vB);
        graph.AddEdge(e2);

        var e3 = graph.CreateEdge(u: vA, v: vB);
        graph.AddEdge(e3);
    }
}
