using System.Numerics;
using Arduino_Display.Drawing;

namespace Arduino_Display;

using System;
using System.IO.Ports;
using Raylib_cs;

class Program
{
    public const byte ScreenSize = 255;
    public const byte PixelScale = 4;
    
    static readonly byte[] White   = [255, 255, 255];
    static readonly byte[] Cyan    = [0,   255, 255];
    static readonly byte[] Magenta = [255, 0,   255];
    static readonly byte[] Yellow  = [255, 255, 0];
    static readonly byte[] Red     = [255, 0,   0];
    static readonly byte[] Blue    = [0,   0,   255];
    static readonly byte[] Green   = [0,   255, 0];

    static readonly (float, float, float)[] OriginalCube = [(0, 0, 0), (0, 64, 0), (64, 0, 0), (64, 64, 0), (0, 0, 64), (0, 64, 64), (64, 0, 64), (64, 64, 64)];
    static (float, float, float)[] Cube = [(0, 0, 0), (0, 64, 0), (64, 0, 0), (64, 64, 0), (0, 0, 64), (0, 64, 64), (64, 0, 64), (64, 64, 64)];
    static readonly (byte, byte)[] CubeIndices = [(0, 1), (1, 3), (3, 2), (2, 0), (4, 5), (5, 7), (7, 6), (6, 4), (0, 4), (1, 5), (2, 6), (3, 7)];

    static readonly float FocalLength = 70;
    
    static void Main(string[] args)
    {
        SerialPort serialPort = new SerialPort("COM4", 9600);
        
        try {
            serialPort.Open();
        } catch {Console.WriteLine("Unable to open port");}
        
        Raylib.InitWindow(ScreenSize*PixelScale, ScreenSize*PixelScale, "ArduinoDisplay");
        Raylib.ClearBackground(Color.Black);
        Raylib.SetTargetFPS(60);
        long frame = 0;
        float time = 0;
        
        byte[] triangle = [10, 30, 20, 10, 50, 50];

        int dir = 1;
        Vector3 position = new Vector3(-10, -10, 32);
        
        while (!Raylib.WindowShouldClose())
        {
            Raylib.ClearBackground(Color.Black);
            
            Raylib.BeginDrawing();  
            
            Utility.DrawPixel(0,0, White);
            Utility.DrawPixel(1,1, Cyan);
            Utility.DrawPixel(1,2, Magenta);
            Utility.DrawPixel(2,3, Yellow);
            Utility.DrawPixel(2,4, Red);
            Utility.DrawPixel(3,5, Blue);
            Utility.DrawPixel(3,6, Green);
            
            WireFrame.DrawLine(20, 30, 10, 10, Red);

            triangle[0]++;
            triangle[3]--;
            
            WireFrame.DrawLine(triangle[0], triangle[1], triangle[2], triangle[3], Red);
            WireFrame.DrawLine(triangle[2], triangle[3], triangle[4], triangle[5], Red);
            WireFrame.DrawLine(triangle[4], triangle[5], triangle[0], triangle[1], Red);
            
            //WireFrame.DrawLine(11 * 5, 16 * 5, 17 * 5, 16 * 5, White);
            //WireFrame.DrawLine(11 * 5, 16 * 5, 8  * 5, 14 * 5, White);
            //WireFrame.DrawLine(11 * 5, 16 * 5, 12 * 5, 10 * 5, White);
            
            //WireFrame.DrawLine(17 * 5, 16 * 5, 14 * 5, 14 * 5, White);
            //WireFrame.DrawLine(9  * 5, 8  * 5, 8  * 5, 14 * 5, White);
            //WireFrame.DrawLine(9  * 5, 8  * 5, 12 * 5, 10 * 5, White);
            //WireFrame.DrawLine(12 * 5, 10 * 5, 18 * 5, 10 * 5, White);
            //WireFrame.DrawLine(15 * 5, 8  * 5, 18 * 5, 10 * 5, White);
            //WireFrame.DrawLine(15 * 5, 8  * 5, 14 * 5, 14 * 5, White);
            //WireFrame.DrawLine(17 * 5, 16 * 5, 18 * 5, 10 * 5, White);
            //WireFrame.DrawLine(8  * 5, 14 * 5, 14 * 5, 14 * 5, White);
            //WireFrame.DrawLine(9  * 5, 8  * 5, 15 * 5, 8  * 5, White);

            time += Raylib.GetFrameTime();
            position.X = float.Sin(time)*40 - 32;
            position.Y = float.Sin(time*2)*40 - 32;
            for (int v = 0; v < Cube.Length; v++) {
                Cube[v].Item1 = OriginalCube[v].Item1 + position.X;
                Cube[v].Item2 = OriginalCube[v].Item2 + position.Y;
                Cube[v].Item3 = OriginalCube[v].Item3 + position.Z;
            }
            for (int i = 0; i < CubeIndices.Length; i++) {
                WireFrame.DrawLine(
                    (byte)(FocalLength * Cube[CubeIndices[i].Item1].Item1/(FocalLength + Cube[CubeIndices[i].Item1].Item3) + ScreenSize/2), (byte)(FocalLength * Cube[CubeIndices[i].Item1].Item2/(FocalLength + Cube[CubeIndices[i].Item1].Item3) + ScreenSize/2),
                    (byte)(FocalLength * Cube[CubeIndices[i].Item2].Item1/(FocalLength + Cube[CubeIndices[i].Item2].Item3) + ScreenSize/2), (byte)(FocalLength * Cube[CubeIndices[i].Item2].Item2/(FocalLength + Cube[CubeIndices[i].Item2].Item3) + ScreenSize/2),
                    White
                    );
            }
            
            WireFrame.DrawCircle(64, 64, 16, Cyan);
            
            WireFrame.DrawCircle(ScreenSize, 64, 16, Cyan);
            WireFrame.DrawCircle(64, ScreenSize, 16, Cyan);
            WireFrame.DrawCircle(ScreenSize-ScreenSize/4, 0, 16, Cyan);
            WireFrame.DrawCircle(0, ScreenSize-ScreenSize/4, 16, Cyan);
            
            WireFrame.DrawCircle(triangle[0], triangle[1], 8, Magenta);
            WireFrame.DrawCircle(triangle[2], triangle[3], 8, Magenta);
            WireFrame.DrawCircle(triangle[4], triangle[5], 8, Magenta);
            
            //byte[] color = new byte[3];
            //for (uint i = 0; i <= (ScreenSize * ScreenSize); i++) {
            //    Raylib.DrawRectangle((int)i/ScreenSize*PixelScale, (int)i%ScreenSize*PixelScale,
            //        PixelScale, PixelScale,
            //        new Color(color[0]+=1, color[1]+=color[0], color[2]+=color[1]));
            //}
            Raylib.EndDrawing();

            frame++;
            //long managedBytes = GC.GetTotalMemory(forceFullCollection: false);
            //Console.WriteLine($"Managed memory: {managedBytes} bytes");
            if (frame % 600 == 0)
            {
                GC.Collect();
                Console.WriteLine("After full GC: " + GC.GetTotalMemory(true));
            }

            //serialPort.WriteLine("Reading");
            //Console.WriteLine(serialPort.ReadLine());
        }
    }
}