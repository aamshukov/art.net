//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using Art.Framework.Core.Domain;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Framework.Core.Diagnostics;

public sealed class Status
{
    public string Origin { get; init; }

    public string OriginId { get; init; }

    public string Message { get; init; }

    public IReadOnlyDictionary<string, string> Details { get; init; }

    public StatusCode CustomCode { get; init; }

    public StatusCode SystemCode { get; init; }

    public StatusCode LibraryCode { get; init; }

    public bool Succeded() => CustomCode == StatusCode.Success &&
                              SystemCode == StatusCode.Success &&
                              LibraryCode == StatusCode.Success;
    public Exception? Exception { get; init; }

    public string ExceptionMessage { get; init; }

    public string Timestamp { get; init; }

    public Status(string? origin = default,
                  string? originId = default,
                  string? message = default,
                  IDictionary<string, string>? details = default,
                  StatusCode customCode = StatusCode.Success,
                  StatusCode systemCode = StatusCode.Success,
                  StatusCode libraryCode = StatusCode.Success,
                  Exception? exception = default)
    {
        Origin = origin?.Trim() ?? string.Empty;
        OriginId = originId?.Trim() ?? string.Empty;
        Message = message?.Trim() ?? string.Empty;
        Details = details?.AsReadOnly() ?? new Dictionary<string, string>().AsReadOnly();
        CustomCode = customCode;
        SystemCode = systemCode;
        LibraryCode = libraryCode;
        Exception = exception;
        ExceptionMessage = Exception != default ? DomainHelper.BuilExceptionText(Exception, nameof(Status)) : nameof(Status);
        Timestamp = DomainHelper.Timestamp();
    }
}
