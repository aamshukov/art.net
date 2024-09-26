//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Tree;

public static class TreeAlgorithms
{
    public static IEnumerable<Tree> Dfs(Tree tree, bool preorder = true)
    {
        Assert.NonNullReference(tree, nameof(tree));

        Stack<Tree> stack = new();

        stack.Push(tree);

        while(stack.Count > 0)
        {
            Tree node = stack.Pop();

            if((node.Flags & Flags.Visited) == Flags.Visited)
                continue;

            node.Flags = HyperGraphAlgorithms.ModifyFlags(node.Flags, add: Flags.Visited);

            if(preorder)
                yield return node;

            foreach(Tree kid in node.Kids)
            {
                if((kid.Flags & Flags.Visited) != Flags.Visited)
                {
                    stack.Push(kid);
                }
            }

            if(!preorder) // postorder
                yield return node;
        }
    }

    public static IEnumerable<Tree> Bfs(Tree tree, bool preorder = true)
    {
        Assert.NonNullReference(tree, nameof(tree));

        Queue<Tree> queue = new();

        queue.Enqueue(tree);

        while(queue.Count > 0)
        {
            Tree node = queue.Dequeue();

            if((node.Flags & Flags.Visited) == Flags.Visited)
                continue;

            node.Flags = HyperGraphAlgorithms.ModifyFlags(node.Flags, add: Flags.Visited);

            if(preorder)
                yield return node;

            foreach(Tree kid in node.Kids)
            {
                if((kid.Flags & Flags.Visited) != Flags.Visited)
                {
                    queue.Enqueue(kid);
                }
            }

            if(!preorder) // postorder
                yield return node;
        }
    }
}
