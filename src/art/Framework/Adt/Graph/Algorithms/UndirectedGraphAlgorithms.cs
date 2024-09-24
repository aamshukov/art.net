//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public static class UndirectedGraphAlgorithms
{
    public static UndirectedGraph BuildGraph(List<id> verticesIds, List<List<id>> edgesId)
    {
        Assert.NonEmptyCollection<id>(verticesIds, nameof(verticesIds));
        Assert.NonNullReference(edgesId, nameof(edgesId));

        UndirectedGraph graph = new(1);

        foreach(id vertexId in verticesIds)
        {
            UndirectedVertex vertex = graph.CreateVertex(label: $"V:Nx:{vertexId}");
            graph.AddVertex(vertex);
        }

        return graph;
    }
}
