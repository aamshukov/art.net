//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Text;
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Tree;

public static class TreeSerialization
{
    public async static Task SerializeTree(string filePath, string fileName, Tree tree, bool digraph = true)
    {
        Assert.NonEmptyString(filePath, nameof(filePath));
        Assert.NonEmptyString(fileName, nameof(fileName));
        Assert.NonNullReference(tree, nameof(tree));

        string content = GenerateGraphvizContent(tree, digraph);

        if(!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        using StreamWriter outputFile = new(Path.Combine(filePath, fileName));

        await outputFile.WriteAsync(content).ConfigureAwait(false);
        await outputFile.FlushAsync().ConfigureAwait(false);

        outputFile.Close();
    }

    private static string GenerateGraphvizContent(Tree tree, bool digraph = true)
    {
        string indent = DomainHelper.GetIndent(4);

        string graphType = digraph ? "digraph" : "graph";
        string edgeType = digraph ? "->" : "--";
        string edgeDir = string.Empty;

        StringBuilder sb = new();

        sb.Append($"{graphType}{Environment.NewLine}");
        sb.Append($"{{{Environment.NewLine}");
        sb.Append($"{indent}edge [dir=\"{edgeDir}\"];{Environment.NewLine}");
        sb.Append($"{indent}node [margin=0 fontcolor=black fontsize=11 width=0.3 shape=circle style=filled forcelabels=true];{Environment.NewLine}");

        foreach(Tree root in TreeAlgorithms.Dfs(tree))
        {
            foreach(Tree? kid in root.Kids.Where(t => t is not null))
            {
                if(kid is null)
                    continue;
                sb.Append($"{indent}\"{root.Label}\" {edgeType} \"{kid.Label}\" [label=\"{root.Label}:{kid.Label}\"];{Environment.NewLine}");
            }
        }

        sb.Append($"}}{Environment.NewLine}");

        return sb.ToString();
    }

    private static string ComposeTreeDescription(Vertex vertex)
    {
        return $"\"{vertex.Label}\" [label=\"{vertex.Label}({vertex.ReferenceCounter.Count})\"]";
    }
}

// for %i in (D:\Tmp\art.trees.viz\*.dot) do D:\Soft\graphviz\12.1.1\bin\dot -Tpng %i -o %i.png
