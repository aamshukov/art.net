//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Graphics;

readonly struct Color
{
    public color R { get; init; }

    public color G { get; init; }

    public color B { get; init; }

    public float Alpha { get; init; }

    public static Color White => new(1.0F, 1.0F, 1.0F);

    public static Color Black => new(0.0F, 0.0F, 0.0F);

    public static Color Red => new(1.0F, 0.0F, 0.0F);

    public static Color Green => new(0.0F, 1.0F, 0.0F);

    public static Color Blue => new(0.0F, 0.0F, 1.0F);

    public Color(color red, color green, color blue, float alpha = 1.0F)
    {
        R = red;
        G = green;
        B = blue;
        Alpha = alpha;
    }
}
