//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using Newtonsoft.Json.Linq;

namespace UILab.Art.Framework.Json;

public interface IJsonPatchBuilder
{
    /// <summary>
    /// Builds Json patch document.
    /// </summary>
    /// <param name="lhs">Source Json document.</param>
    /// <param name="rhs">Target Json document.</param>
    void Build(string lhs, string rhs);

    /// <summary>
    /// Builds Json patch document.
    /// </summary>
    /// <param name="lhs">Source Json document.</param>
    /// <param name="rhs">Target Json document.</param>
    void Build(JToken lhs, JToken rhs);

    /// <summary>
    /// Generates JsonPatch script from document.
    /// </summary>
    /// <returns></returns>
    string GenerateScript();
}
