//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Document;

public sealed class LineMappings
{
    public List<location> LineMap { get; init; }

    public List<bool> TabMap { get; init; }

    public size TabSize { get; init; }

    public count Lf { get; set; }   // LF   \n

    public count Cr { get; set; }   // CR   \r

    public count CrLf { get; set; } // CRLF \r\n

    public count Nel { get; set; }  // <Next Line> (NEL)

    public count Ls { get; set; }   // LINE SEPARATOR

    public count Ps { get; set; }   // PARAGRAPH SEPARATOR

    public LineMappings(size tabSize = 4)
    {
        LineMap = new();
        TabMap = new();
        TabSize = tabSize;
    }
}
