﻿//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Text;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Graph;

public static class GraphvizSerialization
{
    public async static Task SerializeUndirectedHyperGraph(string filePath, string fileName, UndirectedHyperGraph graph)
    {
        Assert.NonEmptyString(filePath, nameof(filePath));
        Assert.NonEmptyString(fileName, nameof(fileName));
        Assert.NonNullReference(graph, nameof(graph));

        string content = GenerateGraphvizContent(graph);

        if(!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        using StreamWriter outputFile = new(Path.Combine(filePath, fileName));

        await outputFile.WriteAsync(content).ConfigureAwait(false);
        await outputFile.FlushAsync().ConfigureAwait(false);

        outputFile.Close();
    }

    public async static Task SerializeDirectedHyperGraph(string filePath, string fileName, DirectedHyperGraph graph)
    {
        Assert.NonEmptyString(filePath, nameof(filePath));
        Assert.NonEmptyString(fileName, nameof(fileName));
        Assert.NonNullReference(graph, nameof(graph));

        string content = GenerateDigraphvizContent(graph);

        if(!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        using StreamWriter outputFile = new(Path.Combine(filePath, fileName));

        await outputFile.WriteAsync(content).ConfigureAwait(false);
        await outputFile.FlushAsync().ConfigureAwait(false);

        outputFile.Close();
    }

    private static string GenerateGraphvizContent(UndirectedHyperGraph graph)
    {
        string indent = DomainHelper.GetIndent(4);

        string graphType = "graph";
        string edgeType = "--";
        string edgeDir = string.Empty;

        StringBuilder sb = new();

        sb.Append($"{graphType}{Environment.NewLine}");
        sb.Append($"{{{Environment.NewLine}");
        sb.Append($"{indent}edge [dir=\"{edgeDir}\"];{Environment.NewLine}");

        foreach(UndirectedVertex vertex in graph.Vertices.Values)
        {
            sb.Append($"{indent}\"{vertex.Label}\" node [shape = circle];{Environment.NewLine}");
        }

        foreach(UndirectedHyperEdge edge in graph.HyperEdges.Values)
        {
            if(edge.Vertices.Count == 1)
            {
                sb.Append($"{indent}\"{edge.Vertices.Values.First().Label}\";{Environment.NewLine}");
            }
            else
            {
                List<(UndirectedVertex, UndirectedVertex)> pairs = DomainHelper.CollectPairs<UndirectedVertex>(edge.Vertices.Values.ToList());

                foreach(var pair in pairs)
                {
                    sb.Append($"{indent}\"{pair.Item1.Label}\" {edgeType} \"{pair.Item2.Label}\" [label=\"{edge.Label}\"];{Environment.NewLine}");
                }
            }
        }

        sb.Append($"}}{Environment.NewLine}");

        return sb.ToString();
    }

    public static string GenerateDigraphvizContent(DirectedHyperGraph graph, bool linkDomainVertices = false)
    {
        string indent = DomainHelper.GetIndent(4);

        string graphType = "digraph";
        string edgeType = "->";
        string edgeDir = string.Empty;

        StringBuilder sb = new();

        sb.Append($"{graphType}{Environment.NewLine}");
        sb.Append($"{{{Environment.NewLine}");
        sb.Append($"{indent}edge [dir=\"{edgeDir}\"];{Environment.NewLine}");

        foreach(DirectedVertex vertex in graph.Vertices.Values)
        {
            sb.Append($"{indent}\"{vertex.Label}\" node [shape = circle];{Environment.NewLine}");
        }

        foreach(DirectedHyperEdge edge in graph.HyperEdges.Values)
        {
            if(linkDomainVertices)
            {
                // link domain's vertices
                if(edge.Domain.Count == 1)
                {
                    sb.Append($"{indent}\"{edge.Domain[0].Label}\";{Environment.NewLine}");
                }
                else
                {
                    List<(DirectedVertex, DirectedVertex)> pairs = DomainHelper.CollectPairs<DirectedVertex>(edge.Domain.Values.ToList());

                    foreach(var pair in pairs)
                    {
                        sb.Append($"{indent}\"{pair.Item1.Label}\" {edgeType} \"{pair.Item2.Label}\" [label=\"{edge.Label}\"];{Environment.NewLine}");
                    }
                }
            }

            // link to codomain's vertices
            foreach(DirectedVertex domainVertex in edge.Domain.Values)
            {
                foreach(DirectedVertex codomainVertex in edge.Codomain.Values)
                {
                    sb.Append($"{indent}\"{domainVertex.Label}\" {edgeType} \"{codomainVertex.Label}\" [label=\"{edge.Label}\"];{Environment.NewLine}");
                }
            }
        }

        sb.Append($"}}{Environment.NewLine}");

        return sb.ToString();
    }
}

// for %i in (D:\Tmp\art.graphs.viz\*.dot) do D:\Soft\graphviz\12.1.1\bin\dot -Tpng %i -o %i.png
