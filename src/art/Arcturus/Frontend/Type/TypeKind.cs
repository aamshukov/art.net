//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Arcturus.Typing;

[Flags]
public enum TypeKind : flag
{
    BuiltinMask   = 0x1000_0000_0000_0000,
    ScalarMask    = 0x0000_0001_0000_0000,
    ArrayMask     = 0x0000_0010_0000_0000,
    CompositeMask = 0x0000_0100_0000_0000,   // struct, record, tuple, enum, etc.
    SubtypeMask   = 0x0000_1000_0000_0000,   // sub-type, slice, span, range, etc.
    CallableMask  = 0x0001_0000_0000_0000,   // fn, proc, lambda, slosure,
                                             // anonymous function/procedure, etc.
    GenericMask   = 0x0010_0000_0000_0000,

    Unknown             =  0,                // initial value for type inference algorithms

    Integer             = 1 | BuiltinMask,   //  int, integer
    Real                = 2 | BuiltinMask,   //  real, float, double
    String              = 3 | BuiltinMask,   //  ' "" ', " '' "
    Boolean             = 4 | BuiltinMask,   //  true, false
    Void                = 5 | BuiltinMask,   //  void
    None                = 6 | BuiltinMask,   //  void

    Class               = 1 | CompositeMask, //  record
    Struct              = 2 | CompositeMask, //  structure
    Record              = 3 | CompositeMask, //  record
    Tuple               = 4 | CompositeMask, //  record
    Enum                = 5 | CompositeMask, //  enumeration

    Sub                 = 1 | SubtypeMask,   //  subtype: byte = 0-255, ascii7 = 0-127
    Slice               = 2 | SubtypeMask,   //  slice: [1:5:1]
    Span                = 3 | SubtypeMask,   //  span: 1-5
    Range               = 4 | SubtypeMask,   //  range: [1-5], (1-5], [1-5), (1-5), closed, half-open, open

    Function            = 1 | CallableMask,  //  fn
    AnonymousFunction   = 2 | CallableMask,  //  anonymous function
    Procedure           = 3 | CallableMask,  //  proc
    AnonymousProcedure  = 4 | CallableMask,  //  anonymous procedure
    Lambda              = 5 | CallableMask,  //  lambda
    Closure             = 6 | CallableMask,  //  closure

    TypeParameter       = 7 | GenericMask,
    TypeArgument        = 8 | GenericMask
}
