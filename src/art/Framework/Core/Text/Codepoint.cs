//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
#pragma warning disable CS8981
global using codepoint = uint;
#pragma warning restore CS8981

namespace UILab.Art.Framework.Core.Text;

public readonly struct Codepoint
{
    public static readonly codepoint MinCodepoint = 0x00000000;

    public static readonly codepoint MaxCodepoint = 0x0010FFFF;

    public static readonly codepoint MaxBmpCodepoint = 0x0000FFFF;

    public static readonly codepoint EpsilonCodepoint = 0x000003B5;

    public static readonly codepoint BadCodepoint = 0x0F000002;

    public static readonly count UnicodePlanesNumber = 17;

    public static readonly size UnicodePlaneSize = 0x0000FFFF;

    public static readonly size UnicodeCharactersCount = 0x0010FFFF;

    public static readonly size AsciiCharactersCount = 0x000000FF;

    public static bool Valid(codepoint value)
    {
        return value >= MinCodepoint && value <= MaxCodepoint;
    }
}
