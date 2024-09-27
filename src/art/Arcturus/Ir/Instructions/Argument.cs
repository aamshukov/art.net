//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Arcturus.Ir.Instructions;

public record Argument<TSymbol>(TSymbol Symbol, // symbol
                                id Id);         // ssa version, default = 0
