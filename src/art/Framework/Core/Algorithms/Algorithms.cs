//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
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
    /// Calculates longest common subsequence (LCS) for two strings.
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

    /// <summary>
    /// Calculates longest common subsequence (LCS) for three strings.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    [SuppressMessage("Major Bug", "S2583:Conditionally executed code should be reachable", Justification = "<Pending>")]
    public static ulong[,,] CalculateLcsTable3<T>(T[] lhs, T[] mhs, T[] rhs, IEqualityComparer<T> comparer)
    {
        size m = lhs.Length; // rows
        size n = mhs.Length; // columns
        size s = rhs.Length; // columns

        ulong[,,] table = new ulong[m + 1, n + 1, s + 1];

        for(index i = 0; i < m; i++)
        {
            for(index j = 0; j < n; j++)
            {
                for(index k = 0; k < s; k++)
                {
                    if(i == 0 || j == 0 || k == 0)
                    {
                        table[i, j, k] = 0;
                    }
                    else
                    {
                        T x = lhs[i];
                        T y = mhs[j];
                        T z = rhs[k];

                        if(comparer.Equals(x, y) && comparer.Equals(x, z))
                        {
                            table[i + 1, j + 1, k + 1] = ((table[i, j, k] + 1) & ~EQUALITY_ELEMENT_MASK) | EQUALITY_ELEMENT_MASK;
                        }
                        else
                        {
                            table[i + 1, j + 1, k + 1] = Math.Max(Math.Max(table[i + 1, j, k], table[i, j + 1, k]), table[i, j, k + 1]) & ~EQUALITY_ELEMENT_MASK;
                        }
                    }
                }
            }
        }

        return table;
    }

    [SuppressMessage("Blocker Code Smell", "S2368:Public methods should not have multidimensional array parameters", Justification = "<Pending>")]
    public static (index, index, index)[] BuildLcs3(ulong[,,] table, size m, size n, size s)
    {
        index i = m; // x
        index j = n; // y
        index k = s; // z

        index c = (index)(table[m, n, s] & ~EQUALITY_ELEMENT_MASK);

        (index, index, index)[] lcs = new (index, index, index)[c];

        while(i > 0 && j > 0 && k > 0)
        {
            if((table[i, j, k] & EQUALITY_ELEMENT_MASK) == EQUALITY_ELEMENT_MASK)
            {
                lcs[c - 1] = (i - 1, j - 1, k - 1); // -1 as we padded with extra row/col for table

                i--;
                j--;
                k--;

                c--;
            }
            else if((table[i - 1, j, k] & ~EQUALITY_ELEMENT_MASK) > (table[i, j - 1, k] & ~EQUALITY_ELEMENT_MASK) &&
                    (table[i - 1, j, k] & ~EQUALITY_ELEMENT_MASK) > (table[i, j, k - 1] & ~EQUALITY_ELEMENT_MASK))
            {
                i--;
            }
            else if((table[i, j - 1, k] & ~EQUALITY_ELEMENT_MASK) > (table[i - 1, j, k] & ~EQUALITY_ELEMENT_MASK) &&
                    (table[i, j - 1, k] & ~EQUALITY_ELEMENT_MASK) > (table[i, j, k - 1] & ~EQUALITY_ELEMENT_MASK))
            {
                j--;
            }
            else
            {
                k--;
            }
        }

        return lcs;
    }

    /// <summary>
    /// Calculates MLCS.
    /// Non optimized version: calculates LCS for first two strings and then result applied to the rest of strings.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sequences"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<index> CalculateMlcsTable<T>(IEnumerable<T> sequences, IEqualityComparer<T> comparer)
    {
        Assert.NotImplemented();
        return Enumerable.Empty<index>();
    }

    /// <summary>
    /// Calculates Longest Common Substring.
    /// The longest common substring is the max value of LCP[] array.
    /// Suffix array construction by induced sorting (SA-IS),
    /// see C++  and Python projects (might be ported over at some point).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strings"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static void CalculateLcs<T>(IEnumerable<T> strings,
                                       IEqualityComparer<T> comparer,
                                       out index index,
                                       out size length)
    {
        Assert.NotImplemented();
        index = 0;
        length = 0;
    }

    public static string CalculateSha256(byte[] data)
    {
        Assert.NonNullReference(data);
        using SHA256 sha256 = SHA256.Create();
        return CalculateSha256(data, sha256);
    }

    public static string CalculateSha256(byte[] data, SHA256 sha256)
    {
        Assert.NonNullReference(data);
        Assert.NonNullReference(sha256);

        byte[] hashBytes = sha256.ComputeHash(data);

        StringBuilder sb = new();

        for(index k = 0, n = hashBytes.Length; k < n; k++)
        {
            sb.Append($"{hashBytes[k]:X2}");
        }

        return sb.ToString();
    }
}
