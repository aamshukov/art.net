//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Adt.Graph;

public enum Direction
{
    Undirectional = 0, // U  --  V
    Directional = 1,   // U  --> V
                       // U  <-- V
    Bidirectional = 2, // U <--> V
}
