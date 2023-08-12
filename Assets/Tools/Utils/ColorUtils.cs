using UnityEngine;
public class ColorUtils
{
    // format 255|255|255|1
    public static Color ParseColorByRGBA(string rgbaStr, string separator)
    {
        Color c = Color.white;
        string[] str = rgbaStr.Split(separator);
        if (str.Length != 4)
        {
            return c;
        }
        c.r = int.Parse(str[0]) / 255;
        c.g = int.Parse(str[1]) / 255;
        c.b = int.Parse(str[2]) / 255;
        c.a = int.Parse(str[3]);
        return c;
    }
}