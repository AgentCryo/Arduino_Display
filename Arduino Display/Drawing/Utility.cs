using Raylib_cs;

namespace Arduino_Display.Drawing;

public static class Utility
{
    public static void DrawPixel(byte x, byte y, byte[] color)
    {
        Raylib.DrawRectangle(x*Program.PixelScale, y*Program.PixelScale, 
            Program.PixelScale, Program.PixelScale,
            new Color(color[0], color[1], color[2]));
    }

    public static byte ModuloAdd(int a, int b)
    {
        return (a + b) >= byte.MaxValue ? (byte)0 : (byte)(a + b);
    }

    public static bool WithinBounds(int xA, int xB)
    {
        return (xA >= 0 && xA < Program.ScreenSize && xB >= 0 && xB < Program.ScreenSize);
    }
}