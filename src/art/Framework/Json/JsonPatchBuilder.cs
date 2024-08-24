//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
// RFC6902: JavaScript Object Notation (JSON) Patch.
// https://datatracker.ietf.org/doc/html/rfc6902
// See OpenJDK for details. Beautiful ideas based on the LCS algorithm to compare arrays.
// Source: jsonp\impl\src\main\java\org\glassfish\json\JsonPatchImpl.java
//
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UILab.Art.Framework.Core.Algorithms;
using UILab.Art.Framework.Core.Diagnostics;

namespace Art.Framework.Json;

public class JsonPatchBuilder : IJsonPatchBuilder
{
    public const string EmptyDocument = "[]";
    private const string PathSeparator = "/";

    /// <summary>
    /// Gets JsonPatch document with populated operations.
    /// </summary>
    public JsonPatchDocument Document { get; init; }

    public JsonPatchBuilder(IJsonPatchDocument document)
    {
        Assert.NonNullReference(document, nameof(document));
        Document = (JsonPatchDocument)document;
    }

    /// <summary>
    /// Builds Json patch document.
    /// </summary>
    /// <param name="lhs">Source Json document.</param>
    /// <param name="rhs">Target Json document.</param>
    public void Build(string lhs, string rhs)
    {
        Assert.NonNullReference(lhs, nameof(lhs));
        Assert.NonNullReference(rhs, nameof(rhs));

        JToken lhsJson = JToken.Parse(lhs);
        JToken rhsJson = JToken.Parse(rhs);

        Assert.NonNullReference(lhsJson, nameof(lhsJson));
        Assert.NonNullReference(rhsJson, nameof(rhsJson));

        Build(lhsJson, rhsJson);
    }

    /// <summary>
    /// Builds Json patch document.
    /// </summary>
    /// <param name="lhs">Source Json document.</param>
    /// <param name="rhs">Target Json document.</param>
    public void Build(JToken lhs, JToken rhs)
    {
        Assert.NonNullReference(lhs, nameof(lhs));
        Assert.NonNullReference(rhs, nameof(rhs));

        Document.Operations.Clear();
        CalculateDifferences(string.Empty, lhs, rhs);
    }

    /// <summary>
    /// Generates JsonPatch script from document.
    /// </summary>
    /// <returns></returns>
    public string GenerateScript()
    {
        string script = JsonConvert.SerializeObject(Document,
                                                    new JsonSerializerSettings
                                                    {
                                                        MissingMemberHandling = MissingMemberHandling.Error,
                                                        NullValueHandling = NullValueHandling.Include,
                                                        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                                                        Formatting = Formatting.Indented,
                                                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                                        DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                                                        DateParseHandling = DateParseHandling.DateTimeOffset,
                                                        StringEscapeHandling = StringEscapeHandling.EscapeHtml,

                                                    });
        return string.IsNullOrWhiteSpace(script) ? EmptyDocument : script;
    }

    private void CalculateDifferences(string path, JToken lhs, JToken rhs)
    {
        if(ReferenceEquals(lhs, rhs))
            return;

        if(lhs.Type == JTokenType.Object && rhs.Type == JTokenType.Object)
        {
            CalculateObjectsDifferences(path, lhs, rhs);
        }
        else if(lhs.Type == JTokenType.Array && rhs.Type == JTokenType.Array)
        {
            CalculateArraysDifferences(path, (JArray)lhs, (JArray)rhs);
        }
        else
        {
            // Types mismatch - replace VALUE at the path with rhs.
            // The target location MUST exist for the operation to be successful.
            Document.Replace(path, rhs);
        }
    }

    private void CalculateObjectsDifferences(string path, JToken lhs, JToken rhs)
    {
        JObject lhsObject = (JObject)lhs;
        JObject rhsObject = (JObject)rhs;

        foreach(JProperty lhsProperty in lhsObject.Properties())
        {
            string lhsName = lhsProperty.Name;
            JToken lhsValue = lhsProperty.Value;

            string newPath = $"{path}{PathSeparator}{lhsName}";

            if(rhsObject.TryGetValue(lhsName, StringComparison.Ordinal, out JToken? rhsValue))
            {
                CalculateDifferences(newPath, lhsValue, rhsValue);
            }
            else
            {
                // The target location MUST exist for the operation to be successful.
                Document.Remove(newPath);
            }
        }

        foreach(JProperty rhsProperty in rhsObject.Properties())
        {
            string rhsName = rhsProperty.Name;
            JToken rhsValue = rhsProperty.Value;

            string newPath = $"{path}{PathSeparator}{rhsName}";

            if(lhsObject.Property(rhsName) is null)
            {
                // The "add" operation performs ...
                Document.Add(newPath, rhsValue);
            }
        }
    }

    private void CalculateArraysDifferences(string path, JArray lhs, JArray rhs)
    {
        // Phase I: Calculate LCS table
        ulong[,] lcsTable = Algorithms.CalculateLcsTable<JToken>(lhs.ToArray(), rhs.ToArray(), comparer: JToken.EqualityComparer);

        // Phase II: Traverse and generate JsonPatch operations.
        CalculateArraysDifferences(path, lhs, rhs, lcsTable, lhs.Count, rhs.Count);
    }

    private void CalculateArraysDifferences(string path, JArray lhs, JArray rhs, ulong[,] lcsTable, index i, index j)
    {
        if(i == 0)
        {
            // end of source detected, let's check target
            if(j > 0)
            {
                CalculateArraysDifferences(path, lhs, rhs, lcsTable, i, j - 1); // considering left, in reversed order to keep indices accurate
                Document.Add($"{path}{PathSeparator}{j - 1}", rhs[j - 1]);
            }
        }
        else if(j == 0)
        {
            // end of target detected, let's check target
            if(i > 0)
            {
                Document.Remove($"{path}{PathSeparator}{i - 1}");
                CalculateArraysDifferences(path, lhs, rhs, lcsTable, i - 1, j); // considering up, in reversed order to keep indices accurate
            }
        }
        else if((lcsTable[i, j] & Algorithms.EQUALITY_ELEMENT_MASK) == Algorithms.EQUALITY_ELEMENT_MASK)
        {
            // array elements are equal, continue with up-diagonal
            CalculateArraysDifferences(path, lhs, rhs, lcsTable, i - 1, j - 1);
        }
        else
        {
            // primary case consulting either left, up or up-diagonal
            ulong lcsValueLeft = lcsTable[i - 1, j] & ~Algorithms.EQUALITY_ELEMENT_MASK;
            ulong lcsValueUp = lcsTable[i, j - 1] & ~Algorithms.EQUALITY_ELEMENT_MASK;

            if(lcsValueLeft > lcsValueUp)
            {
                //         j-1
                //        ---------
                //    i-1 | 0 | 0 | i-1, j
                //        ---------
                // i-1, j | 1 | * |
                //        ---------
                // left is bigger than up, move left
                CalculateArraysDifferences(path, lhs, rhs, lcsTable, i, j - 1);
                Document.Add($"{path}{PathSeparator}{j - 1}", rhs[j - 1]);
            }
            else if(lcsValueLeft < lcsValueUp)
            {
                //         j-1
                //        ---------
                //    i-1 | 0 | 1 | i-1, j
                //        ---------
                // i-1, j | 0 | * |
                //        ---------
                // left is less than up, move up
                Document.Remove($"{path}{PathSeparator}{i - 1}");
                CalculateArraysDifferences(path, lhs, rhs, lcsTable, i - 1, j);
            }
            else
            {
                //         j-1
                //        ---------
                //    i-1 | 0 | 1 | i-1, j
                //        ---------
                // i-1, j | 1 | * |
                //        ---------
                // left and up are equal, follow u-diagonal
                CalculateDifferences($"{path}{PathSeparator}{i - 1}", lhs[i - 1], rhs[j - 1]);
                CalculateArraysDifferences(path, lhs, rhs, lcsTable, i - 1, j - 1);
            }
        }
    }
}
