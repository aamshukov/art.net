//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Arcturus.Type;

public record TypeLayout(size Alignment = 16,    // alignment in memory, 0 no aligned, power of 2 - aligned
                         size AbstractSize = 0,  // size in bits, abstract width, like C type hierarchy
                         size PlatformSize = 0); // size in bits, platform specific width
