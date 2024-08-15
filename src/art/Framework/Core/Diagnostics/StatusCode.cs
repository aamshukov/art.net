//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.Diagnostics;

public enum StatusCode : ulong
{
#pragma warning disable CA1069
    SUCCESS_MASK     = 0x0000_0000_0000_0000,
    INFORMATION_MASK = 0x0001_0000_0000_0000,
    WARNING_MASK     = 0x0010_0000_0000_0000,
    ERROR_MASK       = 0x0100_0000_0000_0000,
    FATAL_ERROR_MASK = 0x1000_0000_0000_0000,

    Unknown = 0,                        // unknown state/value
    Ok = 1          | SUCCESS_MASK,
    Success = 1     | SUCCESS_MASK,
    Information = 2 | INFORMATION_MASK,
    Warning = 3     | WARNING_MASK,
    Attention = 4   | WARNING_MASK,
    Error = 0       | ERROR_MASK,       // failure, recoverable error
    Aborted = 1     | ERROR_MASK,       // request aborted
    FatalError = 0  | FATAL_ERROR_MASK, // failure, non-recoverable error

    UnexpextedEos = 139 | ERROR_MASK
#pragma warning restore CA1069
}
