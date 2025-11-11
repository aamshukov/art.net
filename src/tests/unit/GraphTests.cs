//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    [Test]
    public async Task UndirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs_Success()
    {
        UndirectedGraph graph = new(1);

        var v1 = graph.CreateVertex(label: "1");
        graph.AddVertex(v1);
        var v2 = graph.CreateVertex(label: "2");
        graph.AddVertex(v2);
        var v3 = graph.CreateVertex(label: "3");
        graph.AddVertex(v3);

        var e1 = graph.CreateEdge(u: v1, v: v1, label: "v1-v1:0"); // self-loop
        graph.AddHyperEdge(e1);

        var e2 = graph.CreateEdge(u: v1, v: v1, label: "v1-v1:1");
        graph.AddHyperEdge(e2);

        var e3 = graph.CreateEdge(u: v1, v: v2, label: "v1-v2:2");
        graph.AddHyperEdge(e3);

        var e4 = graph.CreateEdge(u: v1, v: v2, label: "v1-v2:3");
        graph.AddHyperEdge(e4);

        var e5 = graph.CreateEdge(u: v1, v: v3, label: "v1-v3:4");
        graph.AddHyperEdge(e5);

        var e6 = graph.CreateEdge(u: v2, v: v2, label: "v2-v2:5");
        graph.AddHyperEdge(e6);

        var e7 = graph.CreateEdge(u: v2, v: v2, label: "v2-v2:6");
        graph.AddHyperEdge(e7);

        var e8 = graph.CreateEdge(u: v2, v: v1, label: "v2-v1:7");
        graph.AddHyperEdge(e8);

        var e9 = graph.CreateEdge(u: v2, v: v1, label: "v2-v1:8");
        graph.AddHyperEdge(e9);

        var e10 = graph.CreateEdge(u: v2, v: v3, label: "v2-v3:9");
        graph.AddHyperEdge(e10);

        var e11 = graph.CreateEdge(u: v3, v: v3, label: "v3-v3:10");
        graph.AddHyperEdge(e11);

        const string fileName = "UndirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs_Success";
        await GraphvizSerialization.SerializeUndirectedHyperGraph(DotDirectory, $"{fileName}.dot", graph);

        for(index k = 0; k < 2; k++)
        {
            bool preorder = k == 0;

            var adj1 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(v1).ToArray();
            var dfs1 = UndirectedHyperGraphAlgorithms.Dfs(graph, v1, preorder).ToArray();
            Array.Sort(dfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs1.SequenceEqual([v1, v2, v3]), Is.True);
            var bfs1 = UndirectedHyperGraphAlgorithms.Bfs(graph, v1, preorder).ToArray();
            Array.Sort(bfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs1.SequenceEqual([v1, v2, v3]), Is.True);

            var adj2 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(v2).ToArray();
            var dfs2 = UndirectedHyperGraphAlgorithms.Dfs(graph, v2, preorder).ToArray();
            Array.Sort(dfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs2.SequenceEqual([v1, v2, v3]), Is.True);
            var bfs2 = UndirectedHyperGraphAlgorithms.Bfs(graph, v2, preorder).ToArray();
            Array.Sort(bfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs2.SequenceEqual([v1, v2, v3]), Is.True);

            var adj3 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(v3).ToArray();
            var dfs3 = UndirectedHyperGraphAlgorithms.Dfs(graph, v3, preorder).ToArray();
            Array.Sort(dfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs3.SequenceEqual([v1, v2, v3]), Is.True);
            var bfs3 = UndirectedHyperGraphAlgorithms.Bfs(graph, v3, preorder).ToArray();
            Array.Sort(bfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs3.SequenceEqual([v1, v2, v3]), Is.True);
        }
    }

    [Test]
    public async Task UndirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs2_Success()
    {
        UndirectedGraph graph = new(1);

        var v0 = graph.CreateVertex(label: "0");
        graph.AddVertex(v0);
        var v1 = graph.CreateVertex(label: "1");
        graph.AddVertex(v1);
        var v2 = graph.CreateVertex(label: "2");
        graph.AddVertex(v2);
        var v3 = graph.CreateVertex(label: "3");
        graph.AddVertex(v3);

        var e1 = graph.CreateEdge(u: v0, v: v1, label: "v0-v1");
        graph.AddHyperEdge(e1);

        var e2 = graph.CreateEdge(u: v0, v: v2, label: "v0-v2");
        graph.AddHyperEdge(e2);

        var e3 = graph.CreateEdge(u: v1, v: v2, label: "v1-v2");
        graph.AddHyperEdge(e3);

        var e4 = graph.CreateEdge(u: v2, v: v0, label: "v2-v0");
        graph.AddHyperEdge(e4);

        var e5 = graph.CreateEdge(u: v2, v: v3, label: "v2-v3");
        graph.AddHyperEdge(e5);

        var e6 = graph.CreateEdge(u: v3, v: v3, label: "v3-v3:10");
        graph.AddHyperEdge(e6);

        const string fileName = "UndirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs2_Success";
        await GraphvizSerialization.SerializeUndirectedHyperGraph(DotDirectory, $"{fileName}.dot", graph);

        for(index k = 0; k < 2; k++)
        {
            bool preorder = k == 0;

            var adj0 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(v0).ToArray();
            var dfs0 = UndirectedHyperGraphAlgorithms.Dfs(graph, v0, preorder).ToArray();
            Array.Sort(dfs0, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs0.SequenceEqual([v0, v1, v2, v3]), Is.True);
            var bfs0 = UndirectedHyperGraphAlgorithms.Bfs(graph, v0, preorder).ToArray();
            Array.Sort(bfs0, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs0.SequenceEqual([v0, v1, v2, v3]), Is.True);

            var adj1 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(v1).ToArray();
            var dfs1 = UndirectedHyperGraphAlgorithms.Dfs(graph, v1, preorder).ToArray();
            Array.Sort(dfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs1.SequenceEqual([v0, v1, v2, v3]), Is.True);
            var bfs1 = UndirectedHyperGraphAlgorithms.Bfs(graph, v1, preorder).ToArray();
            Array.Sort(bfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs1.SequenceEqual([v0, v1, v2, v3]), Is.True);

            var adj2 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(v2).ToArray();
            var dfs2 = UndirectedHyperGraphAlgorithms.Dfs(graph, v2, preorder).ToArray();
            Array.Sort(dfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs2.SequenceEqual([v0, v1, v2, v3]), Is.True);
            var bfs2 = UndirectedHyperGraphAlgorithms.Bfs(graph, v2, preorder).ToArray();
            Array.Sort(bfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs2.SequenceEqual([v0, v1, v2, v3]), Is.True);

            var adj3 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(v3).ToArray();
            var dfs3 = UndirectedHyperGraphAlgorithms.Dfs(graph, v3, preorder).ToArray();
            Array.Sort(dfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs3.SequenceEqual([v0, v1, v2, v3]), Is.True);
            var bfs3 = UndirectedHyperGraphAlgorithms.Bfs(graph, v3, preorder).ToArray();
            Array.Sort(bfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs3.SequenceEqual([v0, v1, v2, v3]), Is.True);
        }
    }

    [Test]
    public async Task UndirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs3_Success()
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
        var vF = graph.CreateVertex(label: "F");
        graph.AddVertex(vF);

        var e1 = graph.CreateEdge(u: vA, v: vB, label: "A-B");
        graph.AddHyperEdge(e1);

        var e2 = graph.CreateEdge(u: vA, v: vC, label: "A-C");
        graph.AddHyperEdge(e2);

        var e3 = graph.CreateEdge(u: vA, v: vD, label: "A-D");
        graph.AddHyperEdge(e3);

        var e4 = graph.CreateEdge(u: vC, v: vE, label: "C-E");
        graph.AddHyperEdge(e4);

        var e5 = graph.CreateEdge(u: vD, v: vF, label: "D-F");
        graph.AddHyperEdge(e5);

        const string fileName = "UndirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs3_Success";
        await GraphvizSerialization.SerializeUndirectedHyperGraph(DotDirectory, $"{fileName}.dot", graph);

        for(index k = 0; k < 2; k++)
        {
            bool preorder = k == 0;

            var adj1 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vA).ToArray();
            var dfs1 = UndirectedHyperGraphAlgorithms.Dfs(graph, vA, preorder).ToArray();
            Array.Sort(dfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs1.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);
            var bfs1 = UndirectedHyperGraphAlgorithms.Bfs(graph, vA, preorder).ToArray();
            Array.Sort(bfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs1.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);

            var adj2 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vB).ToArray();
            var dfs2 = UndirectedHyperGraphAlgorithms.Dfs(graph, vB, preorder).ToArray();
            Array.Sort(dfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs2.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);
            var bfs2 = UndirectedHyperGraphAlgorithms.Bfs(graph, vB, preorder).ToArray();
            Array.Sort(bfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs2.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);

            var adj3 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vC).ToArray();
            var dfs3 = UndirectedHyperGraphAlgorithms.Dfs(graph, vC, preorder).ToArray();
            Array.Sort(dfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs3.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);
            var bfs3 = UndirectedHyperGraphAlgorithms.Bfs(graph, vC, preorder).ToArray();
            Array.Sort(bfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs3.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);

            var adj4 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vD).ToArray();
            var dfs4 = UndirectedHyperGraphAlgorithms.Dfs(graph, vD, preorder).ToArray();
            Array.Sort(dfs4, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs4.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);
            var bfs4 = UndirectedHyperGraphAlgorithms.Bfs(graph, vD, preorder).ToArray();
            Array.Sort(bfs4, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs4.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);

            var adj5 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vE).ToArray();
            var dfs5 = UndirectedHyperGraphAlgorithms.Dfs(graph, vE, preorder).ToArray();
            Array.Sort(dfs5, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs5.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);
            var bfs5 = UndirectedHyperGraphAlgorithms.Bfs(graph, vE, preorder).ToArray();
            Array.Sort(bfs5, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs5.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);

            var adj6 = UndirectedHyperGraphAlgorithms.GetAdjacentVertices(vF).ToArray();
            var dfs6 = UndirectedHyperGraphAlgorithms.Dfs(graph, vF, preorder).ToArray();
            Array.Sort(dfs6, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs6.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);
            var bfs6 = UndirectedHyperGraphAlgorithms.Bfs(graph, vF, preorder).ToArray();
            Array.Sort(bfs6, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs6.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);
        }
    }

    [Test]
    public async Task DirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs_Success()
    {
        DirectedGraph graph = new(1);

        var v1 = graph.CreateVertex(label: "1");
        graph.AddVertex(v1);
        var v2 = graph.CreateVertex(label: "2");
        graph.AddVertex(v2);
        var v3 = graph.CreateVertex(label: "3");
        graph.AddVertex(v3);

        var e1 = graph.CreateEdge(u: v1, v: v1, label: "v1-v1:0"); // self-loop
        graph.AddHyperEdge(e1);

        var e2 = graph.CreateEdge(u: v1, v: v1, label: "v1-v1:1");
        graph.AddHyperEdge(e2);

        var e3 = graph.CreateEdge(u: v1, v: v2, label: "v1-v2:2");
        graph.AddHyperEdge(e3);

        var e4 = graph.CreateEdge(u: v1, v: v2, label: "v1-v2:3");
        graph.AddHyperEdge(e4);

        var e5 = graph.CreateEdge(u: v1, v: v3, label: "v1-v3:4");
        graph.AddHyperEdge(e5);

        var e6 = graph.CreateEdge(u: v2, v: v2, label: "v2-v2:5");
        graph.AddHyperEdge(e6);

        var e7 = graph.CreateEdge(u: v2, v: v2, label: "v2-v2:6");
        graph.AddHyperEdge(e7);

        var e8 = graph.CreateEdge(u: v2, v: v1, label: "v2-v1:7");
        graph.AddHyperEdge(e8);

        var e9 = graph.CreateEdge(u: v2, v: v1, label: "v2-v1:8");
        graph.AddHyperEdge(e9);

        var e10 = graph.CreateEdge(u: v2, v: v3, label: "v2-v3:9");
        graph.AddHyperEdge(e10);

        var e11 = graph.CreateEdge(u: v3, v: v3, label: "v3-v3:10");
        graph.AddHyperEdge(e11);

        const string fileName = "DirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs_Success";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", graph);

        for(index k = 0; k < 2; k++)
        {
            bool preorder = k == 0;

            var adj1 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(v1).ToArray();
            var dfs1 = DirectedHyperGraphAlgorithms.Dfs(graph, v1, preorder).ToArray();
            Array.Sort(dfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs1.SequenceEqual([v1, v2, v3]), Is.True);
            var bfs1 = DirectedHyperGraphAlgorithms.Bfs(graph, v1, preorder).ToArray();
            Array.Sort(bfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs1.SequenceEqual([v1, v2, v3]), Is.True);

            var adj2 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(v2).ToArray();
            var dfs2 = DirectedHyperGraphAlgorithms.Dfs(graph, v2, preorder).ToArray();
            Array.Sort(dfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs2.SequenceEqual([v1, v2, v3]), Is.True);
            var bfs2 = DirectedHyperGraphAlgorithms.Bfs(graph, v2, preorder).ToArray();
            Array.Sort(bfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs2.SequenceEqual([v1, v2, v3]), Is.True);

            var adj3 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(v3).ToArray();
            var dfs3 = DirectedHyperGraphAlgorithms.Dfs(graph, v3, preorder).ToArray();
            Array.Sort(dfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs3.SequenceEqual([v3]), Is.True);
            var bfs3 = DirectedHyperGraphAlgorithms.Bfs(graph, v3, preorder).ToArray();
            Array.Sort(bfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs3.SequenceEqual([v3]), Is.True);
        }
    }

    [Test]
    public async Task DirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs2_Success()
    {
        DirectedGraph graph = new(1);

        var v0 = graph.CreateVertex(label: "0");
        graph.AddVertex(v0);
        var v1 = graph.CreateVertex(label: "1");
        graph.AddVertex(v1);
        var v2 = graph.CreateVertex(label: "2");
        graph.AddVertex(v2);
        var v3 = graph.CreateVertex(label: "3");
        graph.AddVertex(v3);

        var e1 = graph.CreateEdge(u: v0, v: v1, label: "v0-v1");
        graph.AddHyperEdge(e1);

        var e2 = graph.CreateEdge(u: v0, v: v2, label: "v0-v2");
        graph.AddHyperEdge(e2);

        var e3 = graph.CreateEdge(u: v1, v: v2, label: "v1-v2");
        graph.AddHyperEdge(e3);

        var e4 = graph.CreateEdge(u: v2, v: v0, label: "v2-v0");
        graph.AddHyperEdge(e4);

        var e5 = graph.CreateEdge(u: v2, v: v3, label: "v2-v3");
        graph.AddHyperEdge(e5);

        var e6 = graph.CreateEdge(u: v3, v: v3, label: "v3-v3:10");
        graph.AddHyperEdge(e6);

        const string fileName = "DirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs2_Success";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", graph);

        for(index k = 0; k < 2; k++)
        {
            bool preorder = k == 0;

            var adj1 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(v1).ToArray();
            var dfs1 = DirectedHyperGraphAlgorithms.Dfs(graph, v1, preorder).ToArray();
            Array.Sort(dfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs1.SequenceEqual([v0, v1, v2, v3]), Is.True);
            var bfs1 = DirectedHyperGraphAlgorithms.Bfs(graph, v1, preorder).ToArray();
            Array.Sort(bfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs1.SequenceEqual([v0, v1, v2, v3]), Is.True);

            var adj2 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(v2).ToArray();
            var dfs2 = DirectedHyperGraphAlgorithms.Dfs(graph, v2, preorder).ToArray();
            Array.Sort(dfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs2.SequenceEqual([v0, v1, v2, v3]), Is.True);
            var bfs2 = DirectedHyperGraphAlgorithms.Bfs(graph, v2, preorder).ToArray();
            Array.Sort(bfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs2.SequenceEqual([v0, v1, v2, v3]), Is.True);

            var adj3 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(v3).ToArray();
            var dfs3 = DirectedHyperGraphAlgorithms.Dfs(graph, v3, preorder).ToArray();
            Array.Sort(dfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs3.SequenceEqual([v3]), Is.True);
            var bfs3 = DirectedHyperGraphAlgorithms.Bfs(graph, v3, preorder).ToArray();
            Array.Sort(bfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs3.SequenceEqual([v3]), Is.True);
        }
    }

    [Test]
    public async Task DirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs3_Success()
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
        var vF = graph.CreateVertex(label: "F");
        graph.AddVertex(vF);

        var e1 = graph.CreateEdge(u: vA, v: vB, label: "A-B");
        graph.AddHyperEdge(e1);

        var e2 = graph.CreateEdge(u: vA, v: vC, label: "A-C");
        graph.AddHyperEdge(e2);

        var e3 = graph.CreateEdge(u: vA, v: vD, label: "A-D");
        graph.AddHyperEdge(e3);

        var e4 = graph.CreateEdge(u: vC, v: vE, label: "C-E");
        graph.AddHyperEdge(e4);

        var e5 = graph.CreateEdge(u: vD, v: vF, label: "D-F");
        graph.AddHyperEdge(e5);

        const string fileName = "DirectedGraph_Algorithms_PreorderPostorder_Traversal_DfsBfs3_Success";
        await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{fileName}.dot", graph);

        for(index k = 0; k < 2; k++)
        {
            bool preorder = k == 0;

            var adj1 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(vA).ToArray();
            var dfs1 = DirectedHyperGraphAlgorithms.Dfs(graph, vA, preorder).ToArray();
            Array.Sort(dfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs1.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);
            var bfs1 = DirectedHyperGraphAlgorithms.Bfs(graph, vA, preorder).ToArray();
            Array.Sort(bfs1, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs1.SequenceEqual([vA, vB, vC, vD, vE, vF]), Is.True);

            var adj2 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(vB).ToArray();
            var dfs2 = DirectedHyperGraphAlgorithms.Dfs(graph, vB, preorder).ToArray();
            Array.Sort(dfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs2.SequenceEqual([vB]), Is.True);
            var bfs2 = DirectedHyperGraphAlgorithms.Bfs(graph, vB, preorder).ToArray();
            Array.Sort(bfs2, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs2.SequenceEqual([vB]), Is.True);

            var adj3 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(vC).ToArray();
            var dfs3 = DirectedHyperGraphAlgorithms.Dfs(graph, vC, preorder).ToArray();
            Array.Sort(dfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs3.SequenceEqual([vC, vE]), Is.True);
            var bfs3 = DirectedHyperGraphAlgorithms.Bfs(graph, vC, preorder).ToArray();
            Array.Sort(bfs3, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs3.SequenceEqual([vC, vE]), Is.True);

            var adj4 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(vD).ToArray();
            var dfs4 = DirectedHyperGraphAlgorithms.Dfs(graph, vD, preorder).ToArray();
            Array.Sort(dfs4, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs4.SequenceEqual([vD, vF]), Is.True);
            var bfs4 = DirectedHyperGraphAlgorithms.Bfs(graph, vD, preorder).ToArray();
            Array.Sort(bfs4, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs4.SequenceEqual([vD, vF]), Is.True);

            var adj5 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(vE).ToArray();
            var dfs5 = DirectedHyperGraphAlgorithms.Dfs(graph, vE, preorder).ToArray();
            Array.Sort(dfs5, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs5.SequenceEqual([vE]), Is.True);
            var bfs5 = DirectedHyperGraphAlgorithms.Bfs(graph, vE, preorder).ToArray();
            Array.Sort(bfs5, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs5.SequenceEqual([vE]), Is.True);

            var adj6 = DirectedHyperGraphAlgorithms.GetAdjacentVertices(vF).ToArray();
            var dfs6 = DirectedHyperGraphAlgorithms.Dfs(graph, vF, preorder).ToArray();
            Array.Sort(dfs6, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(dfs6.SequenceEqual([vF]), Is.True);
            var bfs6 = DirectedHyperGraphAlgorithms.Bfs(graph, vF, preorder).ToArray();
            Array.Sort(bfs6, (u, v) => u.Id.CompareTo(v.Id));
            Assert.That(bfs6.SequenceEqual([vF]), Is.True);
        }
    }

    [Test]
    public async Task Graph_Algorithms_Nx_Graphs_Success()
    {
        foreach(string fileName in Directory.GetFiles(@"Data\Nx", "Nx--*", SearchOption.TopDirectoryOnly))
        {
            Console.WriteLine($"Nx graph file: {fileName}");

            HyperGraphAlgorithms.ReadNxGraph(Path.GetDirectoryName(fileName)!,
                                             Path.GetFileName(fileName),
                                             out string label,
                                             out bool digraph,
                                             out List<index> vertices,
                                             out List<List<index>> endPoints);
            if(digraph)
            {
                DirectedGraph graph = DirectedGraphAlgorithms.BuildGraph(label, vertices, endPoints);

                await GraphvizSerialization.SerializeDirectedHyperGraph(DotDirectory, $"{label}.dot", graph);


                foreach(var vertex in graph.Vertices.Values)
                {
                    var dfs1 = DirectedHyperGraphAlgorithms.Dfs(graph, vertex, preorder: true).ToArray();
                    Array.Sort(dfs1, (u, v) => u.Id.CompareTo(v.Id));
                    var dfs2 = DirectedHyperGraphAlgorithms.Dfs(graph, vertex, preorder: false).ToArray();
                    Array.Sort(dfs2, (u, v) => u.Id.CompareTo(v.Id));
                    var bfs1 = DirectedHyperGraphAlgorithms.Bfs(graph, vertex, preorder: true).ToArray();
                    Array.Sort(bfs1, (u, v) => u.Id.CompareTo(v.Id));
                    var bfs2 = DirectedHyperGraphAlgorithms.Bfs(graph, vertex, preorder: false).ToArray();
                    Array.Sort(bfs2, (u, v) => u.Id.CompareTo(v.Id));

                    Assert.That(dfs1.SequenceEqual(dfs2), Is.True);
                    Assert.That(dfs1.SequenceEqual(bfs1), Is.True);
                    Assert.That(dfs1.SequenceEqual(bfs2), Is.True);

                    Assert.That(dfs2.SequenceEqual(dfs1), Is.True);
                    Assert.That(dfs2.SequenceEqual(bfs1), Is.True);
                    Assert.That(dfs2.SequenceEqual(bfs2), Is.True);

                    Assert.That(bfs1.SequenceEqual(dfs2), Is.True);
                    Assert.That(bfs1.SequenceEqual(dfs2), Is.True);
                    Assert.That(bfs1.SequenceEqual(bfs2), Is.True);

                    Assert.That(bfs2.SequenceEqual(dfs1), Is.True);
                    Assert.That(bfs2.SequenceEqual(dfs2), Is.True);
                    Assert.That(bfs2.SequenceEqual(bfs1), Is.True);
                }
            }
            else
            {
                UndirectedGraph graph = UndirectedGraphAlgorithms.BuildGraph(label, vertices, endPoints);

                await GraphvizSerialization.SerializeUndirectedHyperGraph(DotDirectory, $"{label}.dot", graph);

                foreach(var vertex in graph.Vertices.Values)
                {
                    var dfs1 = UndirectedHyperGraphAlgorithms.Dfs(graph, vertex, preorder: true).ToArray();
                    Array.Sort(dfs1, (u, v) => u.Id.CompareTo(v.Id));
                    var dfs2 = UndirectedHyperGraphAlgorithms.Dfs(graph, vertex, preorder: false).ToArray();
                    Array.Sort(dfs2, (u, v) => u.Id.CompareTo(v.Id));
                    var bfs1 = UndirectedHyperGraphAlgorithms.Bfs(graph, vertex, preorder: true).ToArray();
                    Array.Sort(bfs1, (u, v) => u.Id.CompareTo(v.Id));
                    var bfs2 = UndirectedHyperGraphAlgorithms.Bfs(graph, vertex, preorder: false).ToArray();
                    Array.Sort(bfs2, (u, v) => u.Id.CompareTo(v.Id));

                    Assert.That(dfs1.SequenceEqual(dfs2), Is.True);
                    Assert.That(dfs1.SequenceEqual(bfs1), Is.True);
                    Assert.That(dfs1.SequenceEqual(bfs2), Is.True);

                    Assert.That(dfs2.SequenceEqual(dfs1), Is.True);
                    Assert.That(dfs2.SequenceEqual(bfs1), Is.True);
                    Assert.That(dfs2.SequenceEqual(bfs2), Is.True);

                    Assert.That(bfs1.SequenceEqual(dfs2), Is.True);
                    Assert.That(bfs1.SequenceEqual(dfs2), Is.True);
                    Assert.That(bfs1.SequenceEqual(bfs2), Is.True);

                    Assert.That(bfs2.SequenceEqual(dfs1), Is.True);
                    Assert.That(bfs2.SequenceEqual(dfs2), Is.True);
                    Assert.That(bfs2.SequenceEqual(bfs1), Is.True);
                }
            }
        }
    }
}
