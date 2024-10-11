//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Text;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Tree;

public static class GraphvizSerialization
{
    public async static Task SerializeTree(string filePath, string fileName, Tree tree)
    {
        Assert.NonEmptyString(filePath, nameof(filePath));
        Assert.NonEmptyString(fileName, nameof(fileName));
        Assert.NonNullReference(tree, nameof(tree));

        string content = GenerateGraphvizContent(tree);

        if(!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        using StreamWriter outputFile = new(Path.Combine(filePath, fileName));

        await outputFile.WriteAsync(content).ConfigureAwait(false);
        await outputFile.FlushAsync().ConfigureAwait(false);

        outputFile.Close();
    }

    private static string GenerateGraphvizContent(Tree tree)
    {
        string indent = DomainHelper.GetIndent(4);

        string graphType = "tree";
        //string edgeType = "->";
        string edgeDir = string.Empty;

        StringBuilder sb = new();

        sb.Append($"{graphType}{Environment.NewLine}");
        sb.Append($"{{{Environment.NewLine}");
        sb.Append($"{indent}edge [dir=\"{edgeDir}\"];{Environment.NewLine}");
        sb.Append($"{indent}node [margin=0 fontcolor=black fontsize=11 width=0.3 shape=circle style=filled forcelabels=true];{Environment.NewLine}");

        sb.Append($"}}{Environment.NewLine}");

        return sb.ToString();
    }

    private static string ComposeTreeDescription(Tree tree)
    {
        return $"\"{tree.Label}\" [label=\"{tree.Label}({tree.ReferenceCounter.Count})\"];";
    }
}

// for %i in (D:\Tmp\art.trees.viz\*.dot) do D:\Soft\graphviz\12.1.1\bin\dot -Tpng %i -o %i.png
