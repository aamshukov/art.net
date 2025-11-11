//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public static class UndirectedGraphAlgorithms
{
    public static UndirectedGraph BuildGraph(string label, List<id> verticesIds, List<List<id>> endPoints)
    {
        Assert.NonNullReference(verticesIds);
        Assert.NonNullReference(endPoints);

        UndirectedGraph graph = new(1, label: label);

        foreach(id vertexId in verticesIds)
        {
            UndirectedVertex vertex = graph.CreateVertex(label: $"V:Nx:{vertexId}");
            graph.AddVertex(vertex);
        }

        foreach(List<id> endPoint in endPoints)
        {
            if(endPoint.Count == 0)
                continue;

            id uId = endPoint[0];
            id vId = endPoint.Count == 2 ? endPoint[1] : uId;

            if(uId == vId)
            {
                UndirectedVertex u = graph.Vertices[uId];

                UndirectedEdge edge = graph.CreateEdge(u: u, v: u, label: $"{u.Label}-{u.Label}");
                graph.AddEdge(edge);
            }
            else
            {
                UndirectedVertex u = graph.Vertices[uId];
                UndirectedVertex v = graph.Vertices[vId];

                UndirectedEdge edge = graph.CreateEdge(u: u, v: v, label: $"{u.Label}-{v.Label}");
                graph.AddEdge(edge);
            }
        }

        return graph;
    }
}
