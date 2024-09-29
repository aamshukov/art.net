//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;

namespace UILab.Art.Framework.Core.Diagnostics;

public sealed class Diagnostics
{
    public IEnumerable<Status> Diagnostic { get; init; }

    /// <summary>
    /// Gets how many spurious errors are allowed before termination.
    /// </summary>
    public count SpuriousErrors { get; init; }

    [SuppressMessage("Critical Code Smell", "S3265:Non-flag enums should not be used in bitwise operations", Justification = "<Pending>")]
    public IEnumerable<Status> Warnings => Diagnostic.Where(s => (s.CustomCode & StatusCode.WARNING_MASK) == StatusCode.ERROR_MASK);

    [SuppressMessage("Critical Code Smell", "S3265:Non-flag enums should not be used in bitwise operations", Justification = "<Pending>")]
    public IEnumerable<Status> Errors => Diagnostic.Where(s => ((s.CustomCode & StatusCode.ERROR_MASK) == StatusCode.ERROR_MASK) ||
                                                               ((s.CustomCode & StatusCode.FATAL_ERROR_MASK) == StatusCode.FATAL_ERROR_MASK));

    public Status LastStatus => Diagnostic.LastOrDefault() ?? Status.Sentinel;

    public Diagnostics(count spuriousErrors = 1024)
    {
        Diagnostic = new List<Status>();
        SpuriousErrors = spuriousErrors;
    }

    public Status? GetLastStatus()
    {
        return Diagnostic.FirstOrDefault();
    }
}
