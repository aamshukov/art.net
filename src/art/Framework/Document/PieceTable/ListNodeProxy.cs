//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace Art.Framework.Document.PieceTable;

public record ListNodeProxy(LinkedListNode<Piece> Node,
                            LinkedListNode<Piece> Prev,
                            LinkedListNode<Piece> Next);
