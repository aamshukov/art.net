//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Framework.Core.Platform;

namespace UILab.Art.Arcturus.Typing;

public record TypeLayout(size Alignment = 16,    // alignment in memory, 0 not aligned, power of 2 - aligned
                         size AbstractSize = 0,  // size in bits, abstract width, like C type hierarchy
                         size PlatformSize = 0,  // size in bits, platform specific width
                         EndiannessType Endianness = EndiannessType.LittleEndian); 
