//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace Art.Framework.Document.PieceTable;

public enum ContentType
{
    Sentinel = 0,  // unknow content, for sentinels
    Original = 1, // readonly original content
    Added = 2     // working (add) content
}
