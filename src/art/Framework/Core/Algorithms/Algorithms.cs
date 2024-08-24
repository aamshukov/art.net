//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.Algorithms;

public static class Algorithms
{
    [SuppressMessage("Blocker Code Smell", "S2368:Public methods should not have multidimensional array parameters", Justification = "<Pending>")]
    public static void CalculateTransitiveClosure(bool[,] a)
    {
        Assert.Ensure((a.GetUpperBound(0) - a.GetLowerBound(0)) > 1, "Number of elements in dimension 0 must be greater than 1.");
        Assert.Ensure((a.GetUpperBound(1) - a.GetLowerBound(1))  > 1, "Number of elements in dimension 1 must be greater than 1.");
        Assert.Ensure((a.GetUpperBound(0) - a.GetLowerBound(0)) == (a.GetUpperBound(1) - a.GetLowerBound(1)), "Number of elements in dimensions must be the same.");

        // calculate transitive closure, Warshall's algorithm,
        // Compiler Construction, Barrett, Bates, Gustafson, Couch, p. 199
        int n = (a.GetUpperBound(0) - a.GetLowerBound(0)) + 1;

        for(var i = 0; i < n; i++)
        {
            for(var j = 0; j < n; j++)
            {
                if(a[j, i])
                {
                    for(var k = 0; k < n; k++)
                    {
                        a[j, k] = a[j, k] || a[i, k];
                    }
                }
            }
        }
    }

    /// <summary>
    /// If set, indicates two elements are equal.
    /// For arrays, for example, avoids computing equality again and again.
    /// </summary>
    public const ulong EQUALITY_ELEMENT_MASK = 0x0010_0000_0000_0000;

    /// <summary>
    /// Calculates longest common subsequence (LCS).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static ulong[,] CalculateLcsTable<T>(T[] lhs, T[] rhs, IEqualityComparer<T> comparer)
    {
        size m = lhs.Length; // rows
        size n = rhs.Length; // columns

        ulong[,] table = new ulong[m + 1, n + 1];

        for(index i = 0; i < m; i++) // rows
        {
            for(index j = 0; j < n; j++) // columns
            {
                T x = lhs[i];
                T y = rhs[j];

                if(comparer.Equals(x, y))
                {
                    table[i + 1, j + 1] = ((table[i, j] + 1) & ~EQUALITY_ELEMENT_MASK) | EQUALITY_ELEMENT_MASK;
                }
                else
                {
                    table[i + 1, j + 1] = Math.Max(table[i + 1, j], table[i, j + 1]) & ~EQUALITY_ELEMENT_MASK;
                }
            }
        }

        return table;
    }

    [SuppressMessage("Blocker Code Smell", "S2368:Public methods should not have multidimensional array parameters", Justification = "<Pending>")]
    public static (index, index)[] BuildLcs(ulong[,] table, size m, size n)
    {
        index i = m; // rows
        index j = n; // columns

        index k = (index)(table[m, n] & ~EQUALITY_ELEMENT_MASK);

        (index, index)[] lcs = new (index, index)[k];

        while(i > 0 && j > 0)
        {
            if((table[i, j] & EQUALITY_ELEMENT_MASK) == EQUALITY_ELEMENT_MASK)
            {
                lcs[k - 1] = (i - 1, j - 1); // -1 as we padded with extra row/col for table

                i--;
                j--;
                k--;
            }
            else if((table[i - 1, j] & ~EQUALITY_ELEMENT_MASK) > (table[i, j - 1] & ~EQUALITY_ELEMENT_MASK))
            {
                i--;
            }
            else
            {
                j--;
            }
        }

        return lcs;
    }
}
