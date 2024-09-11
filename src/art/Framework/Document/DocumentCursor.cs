//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Document;

public sealed class DocumentCursor
{
    public location MasterLocation { get; init; }

    public List<location> Locations { get; init; }

    public DocumentCursor()
    {
        MasterLocation = 0;
        Locations = new();
    }
}
