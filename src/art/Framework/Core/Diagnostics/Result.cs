//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Core.Diagnostics;

public class Result : Domain.ValueType
{
    public Status Status { get; init; }

    public Result(Status status, string? version = default) : base(version)
    {
        Assert.NonNullReference(status, nameof(status));
        Status = status;
    }

    public bool Succeded() => Status.Succeded();

    public static Result Success(string? version = default)
    {
        return new Result(new Status(version: version), version: version);
    }

    public static Result Failure(Exception? exception = default, string? message = default, string? version = default)
    {
        return new Result(new Status(customCode: StatusCode.Error,
                                     message: message,
                                     exception: exception,
                                     version: version));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;

        yield return Status;
    }

    public override string ToString()
    {
        return DomainHelper.Stringify(this);
    }
}

public class Result<T> : Result
{
    public T Payload { get; init; }

    public Result(T payload,
                  Status status,
                  string? version = default) : base(status, version)
    {
        Payload = payload;
    }

    public static Result<T> Success(T payload,
                                    string? origin = default,
                                    string? originId = default,
                                    string? message = default,
                                    IDictionary<string, string>? details = default,
                                    StatusCode customCode = StatusCode.Success,
                                    StatusCode systemCode = StatusCode.Success,
                                    StatusCode libraryCode = StatusCode.Success,
                                    string? version = default)
    {
        return new Result<T>(payload,
                             new Status(origin: origin,
                                        originId: originId,
                                        message: message,
                                        details: details,
                                        customCode: customCode,
                                        systemCode: systemCode,
                                        libraryCode: libraryCode,
                                        version : version));
    }

    public static Result<T> Failure(T payload,
                                    string? origin = default,
                                    string? originId = default,
                                    string? message = default,
                                    IDictionary<string, string>? details = default,
                                    StatusCode customCode = StatusCode.Error,
                                    StatusCode systemCode = StatusCode.Error,
                                    StatusCode libraryCode = StatusCode.Error,
                                    Exception? exception = default,
                                    string? version = default)
    {
        return new Result<T>(payload,
                             new Status(origin: origin,
                                        originId: originId,
                                        message: message,
                                        details: details,
                                        customCode: customCode,
                                        systemCode: systemCode,
                                        libraryCode: libraryCode,
                                        exception: exception,
                                        version : version));
    }
}
