namespace Arduino_Display.Drawing;

public static class WireFrame
{
    public static void DrawLine(byte xA, byte yA, byte xB, byte yB, byte[] color)
    {
        short dx    = (short)Math.Abs(xB - xA);
        short sx    = (short)(xA < xB ? 1 : -1);
        short dy    = (short)-Math.Abs(yB - yA);
        short sy    = (short)(yA < yB ? 1 : -1);
        short error = (short)(dx + dy);

        while (true) {
            if(Utility.WithinBounds(xA, yA)) Utility.DrawPixel(xA, yA, color);
            short e2 = (short)(error * 2);
            if (e2 >= dy) {
                if (xA == xB) break;
                error += dy;
                xA = (byte)(xA + sx);
            }
            if (e2 <= dx) {
                if (yA == yB) break;
                error += dx;
                yA = (byte)(yA + sy);
            }
        }
    }

    public static void DrawCircle(byte x, byte y, byte radius, byte[] color)
    {
        float t1 = radius / 16;
        byte xc = radius; byte yc = 0;
        while (xc > yc) {
            if(Utility.WithinBounds(xc  + x, yc + y)) Utility.DrawPixel((byte)(xc  + x), (byte)(yc + y),  color);
            if(Utility.WithinBounds(yc  + x, xc + y)) Utility.DrawPixel((byte)(yc  + x), (byte)(xc + y),  color);
            if(Utility.WithinBounds(xc  + x, -yc + y)) Utility.DrawPixel((byte)(xc  + x), (byte)(-yc + y), color);
            if(Utility.WithinBounds(yc  + x, -xc + y)) Utility.DrawPixel((byte)(yc  + x), (byte)(-xc + y), color);
            if(Utility.WithinBounds(-xc + x, yc + y)) Utility.DrawPixel((byte)(-xc + x), (byte)(yc + y),  color);
            if(Utility.WithinBounds(-yc + x, xc + y)) Utility.DrawPixel((byte)(-yc + x), (byte)(xc + y),  color);
            if(Utility.WithinBounds(-xc + x, -yc + y)) Utility.DrawPixel((byte)(-xc + x), (byte)(-yc + y), color);
            if(Utility.WithinBounds(-yc + x, -xc + y)) Utility.DrawPixel((byte)(-yc + x), (byte)(-xc + y), color);
            
            yc++;
            t1 += yc;
            float t2 = t1 - xc; 
            if (t2 >= 0) {
                t1 = t2;
                xc--;
            }
        }
    }
}