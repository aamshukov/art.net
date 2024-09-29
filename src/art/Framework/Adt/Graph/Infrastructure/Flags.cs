//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Adt.Graph;

[Flags]
public enum Flags : flag
{
    Clear       = 0x0000_0000_0000_0000,
    Dirty       = 0x0000_0000_0000_0001,
    Processed   = 0x0000_0000_0000_0010,
    Completed   = 0x0000_0000_0000_0100,
    Visited     = 0x0000_0000_0000_1000,
    Marked      = 0x0000_0000_0001_0000,
    Deleted     = 0x0000_0000_0010_0000,
    Leaf        = 0x0000_0000_0100_0000,
    Overflow    = 0x0000_0000_1000_0000,
    Underflow   = 0x0000_0001_0000_0000,
    Invalid     = 0x0000_0010_0000_0000,
    Genuine     = 0x0000_0100_0000_0000,
    Synthetic   = 0x0000_1000_0000_0000,  // additional (artificial) tokens which are inserted
                                          // into the tokenizer stream, syntactic sugar - desugaring ...
                                          // also, might be relevant for synthetic names (anonymous functions, closures, etc.)
    Contextual = 0x0001_0000_0000_0000    // contextual, recognized in specific contexts,
                                          // similar to C# get/set, async/await ...
}
