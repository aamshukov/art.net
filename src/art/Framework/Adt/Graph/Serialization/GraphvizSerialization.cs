//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Text;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Graph;

public static class GraphvizSerialization
{
    public async static Task SerializeUndirectedHyperGraph(string filePath,
                                                           string fileName,
                                                           UndirectedHyperGraph graph,
                                                           bool showSelfLoops = true)
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

    public async static Task SerializeDirectedHyperGraph(string filePath,
                                                         string fileName,
                                                         DirectedHyperGraph graph,
                                                         bool showSelfLoops = true)
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

    private static string GenerateGraphvizContent(UndirectedHyperGraph graph, bool showSelfLoops = true)
    {
        string indent = DomainHelper.GetIndent(4);

        string graphType = "graph";
        string edgeType = "--";
        string edgeDir = string.Empty;

        StringBuilder sb = new();

        sb.Append($"{graphType}{Environment.NewLine}");
        sb.Append($"{{{Environment.NewLine}");
        sb.Append($"{indent}edge [dir=\"{edgeDir}\"];{Environment.NewLine}");
        sb.Append($"{indent}node [margin=0 fontcolor=black fontsize=11 width=0.3 shape=circle style=filled forcelabels=true];{Environment.NewLine}");

        foreach(UndirectedVertex vertex in graph.Vertices.Values)
        {
            string description = ComposeVertexDescription<UndirectedVertex>(vertex);
            sb.Append($"{indent}{description}{Environment.NewLine}");
        }

        foreach(UndirectedVertex vertex in graph.Vertices.Values)
        {
            if(vertex.IsEmpty())
            {
                sb.Append($"{indent}\"{vertex.Label}\";{Environment.NewLine}");
            }
        }

        foreach(UndirectedHyperEdge edge in graph.HyperEdges.Values)
        {
            if(edge.Vertices.Count == 1)
            {
                var label = edge.Vertices.Values.First().Label;

                if(showSelfLoops)
                {
                    sb.Append($"{indent}\"{label}\" {edgeType} \"{label}\" [label=\"{edge.Label}\"];{Environment.NewLine}");
                }
                else
                {
                    sb.Append($"{indent}\"{label}\";{Environment.NewLine}");
                }
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

    public static string GenerateDigraphvizContent(DirectedHyperGraph graph, bool linkDomainVertices = false, bool showSelfLoops = true)
    {
        string indent = DomainHelper.GetIndent(4);

        string graphType = "digraph";
        string edgeType = "->";
        string edgeDir = string.Empty;

        StringBuilder sb = new();

        sb.Append($"{graphType}{Environment.NewLine}");
        sb.Append($"{{{Environment.NewLine}");
        sb.Append($"{indent}edge [dir=\"{edgeDir}\"];{Environment.NewLine}");
        sb.Append($"{indent}node [margin=0 fontcolor=black fontsize=11 width=0.3 shape=circle style=filled forcelabels=true];{Environment.NewLine}");

        foreach(DirectedVertex vertex in graph.Vertices.Values)
        {
            string description = ComposeVertexDescription<DirectedVertex>(vertex);
            sb.Append($"{indent}{description}{Environment.NewLine}");
        }

        foreach(DirectedVertex vertex in graph.Vertices.Values)
        {
            if(vertex.IsEmpty())
            {
                sb.Append($"{indent}\"{vertex.Label}\";{Environment.NewLine}");
            }
        }

        foreach(DirectedHyperEdge edge in graph.HyperEdges.Values)
        {
            if(linkDomainVertices)
            {
                // link domain's vertices
                if(edge.Domain.Count == 1)
                {
                    var label = edge.Domain[0].Label;

                    if(showSelfLoops)
                    {
                        sb.Append($"{indent}\"{label}\" {edgeType} \"{label}\" [label=\"{edge.Label}\"];{Environment.NewLine}");
                    }
                    else
                    {
                        sb.Append($"{indent}\"{label}\";{Environment.NewLine}");
                    }
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

    private static string ComposeVertexDescription<TVertex>(TVertex vertex) where TVertex : Vertex
    {
        return $"\"{vertex.Label}\" [label=\"{vertex.Label}({vertex.ReferenceCounter.Count})\"];";
    }
}

// for %i in (D:\Tmp\art.graphs.viz\*.dot) do D:\Soft\graphviz\12.1.1\bin\dot -Tpng %i -o %i.png
