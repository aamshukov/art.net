//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
// Based on Dan Gusfield's book "Algorithms on Strings, Trees, and Sequences".
//
// 𝑖 𝑗 𝑘 𝑙 𝑚 𝑛
//
using System.Diagnostics.CodeAnalysis;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;
using UILab.Art.Framework.Core.Text.Search.Abstractions;

namespace UILab.Art.Framework.Core.Text.Search;

public sealed class BoyerMoore : ISearch
{
    public enum ZAlgorithm
    {
        Naive,
        Gusfield
    }

    public TextEncoding Encoding { get; init; }

    public BoyerMoore(TextEncoding encoding = TextEncoding.Ascii)
    {
        Encoding = encoding;
    }

    /// <summary>
    /// Implements Boyer-Moore algorithm.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pattern"></param>
    /// <param name="start"></param>
    /// <param name="length"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public IEnumerable<index> Search(ReadOnlyMemory<codepoint> text,
                                     ReadOnlyMemory<codepoint> pattern,
                                     index start = 0,
                                     size length = size.MaxValue,
                                     count count = count.MaxValue,
                                     object? meta = default)
    {
        Assert.NonNullReference(text, nameof(text));
        Assert.NonNullReference(pattern, nameof(pattern));
        Assert.Ensure(start >= 0, nameof(start));
        Assert.Ensure(length >= 0, nameof(length));
        Assert.Ensure(count >= 0, nameof(count));

        List<index> matches = new();

        DomainHelper.NormalizeRange(ref start, ref length, text.Length);

        if(length == 0 || count == 0 || text.Length == 0 || pattern.Length == 0)
            return matches;

        ReadOnlySpan<codepoint> txt = text.Slice(start, length).Span;
        ReadOnlySpan<codepoint> ptr = pattern.Span;

        // preprocessing
        CalculateBadCharValues(ptr, Encoding, out index[] badChars);
        CalculateGoodSuffixValues(ptr, out index[] upperCaseLprime, out size[] lowerCaseLprime, meta);

        // search
        size m = length;
        size n = ptr.Length;

        index k = n - 1; // global index, init with end of pattern
        index i;         // pattern's index
        index h;         // text's index
        index s = -1;    // sentinel - last found mathed index + 1, when search back DO NOT pass it

        while(k < m)
        {
            i = n - 1; // end of pattern
            h = k;

            while(i >= 0 && ptr[i] == txt[h]) // match right to left
            {
                i = i - 1;
                h = h - 1;

                if(h < s) // check sentinel
                    break;
            }

            if(i < 0) // match
            {
                s = k + 1; // set sentinel

                matches.Add(k - n + 1); // ... ending at position k

                if(matches.Count == count)
                    break;

                size lprime2 = n > 1 ? lowerCaseLprime[2 - 1] : 0; // l'(2)
                k = k + n - lprime2;
            }
            else // mismatch
            {
                // shift P (increase k) by the maximum amount
                // deterrnined by the bad character rule and the good suffìx rule
                size shift = 1;

                if(i == n - 1)
                {
                    // one special case remains, when the first
                    // comparison (in pattern, P[n]) is mismatch then
                    // shift one place to the right.
                }
                else
                {
                    // bad char shift operates with mismatched char
                    size badCharShift = i - badChars[txt[h]];
                    shift = Math.Max(shift, badCharShift);

                    // good suffix shift operates with the last matched character
                    size goodSuffixShift = upperCaseLprime[i + 1] > 0
                                                ? (n - upperCaseLprime[i + 1] - 1)  // -1 beacuse L' is index
                                                : (n - lowerCaseLprime[i] - 1);     // l' is size, still need to substract
                    shift = Math.Max(shift, goodSuffixShift);
                }

                k += shift;
            }
        }

        return matches;
    }

    /// <summary>
    /// Gusfield: preprocessing phase I, calculate R(x).
    /// Bad character rule of the Boyer-Moore algorithm.
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="badChars"></param>
    public static void CalculateBadCharValues(ReadOnlySpan<codepoint> pattern, TextEncoding encoding, out index[] badChars)
    {
        badChars = new index[encoding == TextEncoding.Ascii ? Codepoint.AsciiCharactersCount :
                                                              Codepoint.UnicodeCharactersCount];
        size n = pattern.Length;

        for(index k = 0; k < n; k++)
        {
            badChars[pattern[k]] = k;
        }
    }

    /// <summary>
    /// Gusfield: preprocessing phase II.
    /// Good suffix rule of the Boyer-Moore algorithm.
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="upperCaseLprime"></param>
    /// <param name="lowerCaseLprime"></param>
    [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>")]
    public static void CalculateGoodSuffixValues(ReadOnlySpan<codepoint> pattern,
                                                 out index[] upperCaseLprime,
                                                 out size[] lowerCaseLprime,
                                                 object? meta = default)
    {
        size n = pattern.Length;

        // reverse the pattern
        Span<codepoint> reversedPattern = new(GC.AllocateUninitializedArray<codepoint>(n)); // skip zero-init for large arrays

        pattern.CopyTo(reversedPattern);
        reversedPattern.Reverse();

        ReadOnlySpan<codepoint> p = reversedPattern;

        // calculate Z-array
        size[] z = new size[n];

        ZAlgorithm algorithm = meta != default ? (ZAlgorithm)meta : ZAlgorithm.Naive;

        if(algorithm == ZAlgorithm.Naive)
        {
            for(index k = 1; k < n; k++)
            {
                size c = 0; // count

                while(c + k < n && p[c] == p[c + k])
                {
                    c += 1;
                }

                z[k] = c;
            }
        }
        else // Gusfield, based on examples from the book
        {
            // calculate
            index l = 0; // Z-box start, inclusive
            index r = 0; // Z-box end, inclusive

            for(index k = 1; k < n; k++) // start from 1 as S is 0-based and considering the second char first
            {
                if(k > r)
                {
                    // case 1
                    index j = 0;

                    while(k + j < n && p[j] == p[k + j])
                    {
                        j++;
                    }

                    z[k] = j;

                    if(j > 0)
                    {
                        l = k;
                        r = k + z[k] - 1;
                    }
                }
                else
                {
                    // case 2
                    index kprime = k - l;   // k'
                    index beta = r - k + 1; // 𝛃

                    if(z[kprime] < beta)
                    {
                        // case 2a
                        z[k] = z[kprime];
                    }
                    else
                    {
                        // case 2b
                        index j = 1;

                        while(r + j < n && p[beta + j] == p[r + j])
                        {
                            j++;
                        }

                        z[k] = r + j - k;

                        l = k;
                        r = r + j - 1;
                    }
                }
            }
        }

        // calculate Nj(P)
        size[] njp = new size[n];

        for(index j = 0; j < n; j++)
        {
            // N𝑗(P) = Z𝑛-𝑗+1(P')
            njp[j] = z[(n - 1) - j];
        }

        // calculate L'
        // 0 1 2 3 4 5 6 7 8
        // c a b d a b d a b
        //
        // L(7) = 5 because:
        //  at position 7 there is P[7..8] = 'a b', and there might be a suffix 'a b' before position 7
        //  aha, at position 4 there is 'a b' and L(𝑖) gives the right end-position of
        //  the right-most copy of P[𝑖..𝑛] that is not a suffix of P, so L(𝑖) = L(7) = 5.
        //
        // L'(7) = 2 because:
        //  ... with the stronger, added condition that its preceding character is unequal to P(𝑖-1).
        // In this case, P[4..5] = 'a b', but preceding character P(𝑖-1) = P[7-1] = P[6] = 'd' and that controverses with L' requirement,
        // looking further back, there is P[1..2] = 'a b', and P[0] = 'c' and because 'c' != (P(𝑖-1) = P[7-1] = P[6] = 'd') the right end-position is 2, so L'(7) = 2.
        index[] Lprime = new index[n];

        for(index j = 0; j < n - 1; j++) // n - 1 ... For each 𝑖, L(𝑖) is the largest position less than 𝑛 ...
        {
            index i = n - njp[j];

            if(i < n) // L(𝑖) is the largest index 𝑗 less than 𝑛 ...
                Lprime[i] = j;
        }

        upperCaseLprime = Lprime;

        // calculate l', case when L'(𝑖) = 0 or when an occurrence of P is found ... aka matched prefixes
        size[] lprime = new size[n];

        if(n > 1) // this small optimization avoids checking if(i + 1 < n) in every iteration
            lprime[n - 1] = p[0] == p[n - 1] ? 1 : 0;

        for(index i = n - 1 - 1, j = 1; i >= 1; i--, j++)
        {
            if(njp[j] == j)
                lprime[i] = j;
            else
                lprime[i] = lprime[i + 1];
        }

        lowerCaseLprime = lprime;
    }

    [SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<Pending>")]
    private static void CalculateZValuesNaive(ReadOnlySpan<codepoint> pattern, out size[] z)
    {
        size length = pattern.Length;

        z = new size[length];

        for(index k = 0; k < length; k++)
        {
            size n = 0; // count

            while(n + k < length && pattern[n] == pattern[n + k])
            {
                n += 1;
            }

            z[k] = n;
        }
    }
}
