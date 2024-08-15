//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Security.Cryptography;

namespace UILab.Art.Framework.Core.Text;

public static class Text
{
    public static location CalculateNextTabStop(index column, size tabSize = 4)
    {
        return tabSize - column % tabSize;
    }

    public static string GetRandomText(size length, string? text = default)
    {
        string abc = text ?? "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        length = Math.Max(1, length);

        char[] randomChars = new char[length];

        for(int i = 0; i < length; i++)
        {
            randomChars[i] = abc[RandomNumberGenerator.GetInt32(0, abc.Length)];
        }

        return new string(randomChars);
    }

    public static string GetRandomTextWithCrLf(size length)
    {
        string text = GetRandomText(length);

        for(int i = 0; i < length; i++)
        {
            offset offset = RandomNumberGenerator.GetInt32(0, length);

            if((offset % 3) == 0)
                text = text.Insert(offset, "\r");
            else if((offset % 5) == 0)
                text = text.Insert(offset, "\n");
            else if((offset % 7) == 0)
                text = text.Insert(offset, "\r\n");
        }

        return text;
    }

    public static ReadOnlyMemory<codepoint> GetCodepoints(string text)
    {
        return text.EnumerateRunes().Select(r => (codepoint)r.Value).ToArray().AsMemory<codepoint>();
    }
}
