//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Framework.Core.Platform;

public static class Platform
{
    public static bool IsLittleEndian()
    {
        return BitConverter.IsLittleEndian;
    }

    public static bool IsBigEndian()
    {
        return !BitConverter.IsLittleEndian;
    }

    public static EndiannessType GetEndianness()
    {
        return BitConverter.IsLittleEndian ? EndiannessType.LittleEndian : EndiannessType.BigEndian;
    }
}
