using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace TestProject.Core;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

public static class Vector2Extensions
{
    public static Vector2 ClampLength(this Vector2 v, float min, float max)
    {
        if (v.Length() > max)
        {
            return Vector2.Normalize(v) * max;
        }

        if (v.Length() < min)
        {
            return Vector2.Normalize(v) * min;
        }
        return v;
    }
}

public static class Util
{
    public static Vector2 GetCubicBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        var u = 1 - t;
        var tt = t * t;
        var uu = u * u;
        var uuu = uu * u;
        var ttt = tt * t;

        var p = uuu * p0; // (1-t)^3 * P0
        p += 3 * uu * t * p1;    // 3(1-t)^2 * t * P1
        p += 3 * u * tt * p2;    // 3(1-t) * t^2 * P2
        p += ttt * p3;           // t^3 * P3

        return p;
    }
    
    public static Vector2 GetCubicBezierVelocity(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        var it = 1.0f - t;
        return 3 * it * it * (p1 - p0) +
               6 * it * t * (p2 - p1) +
               3 * t * t * (p3 - p2);
    }

    public static void DrawBresenhamCubicBezier(
        SpriteBatch spriteBatch, 
        Vector2 p0, Vector2 p1, 
        Vector2 p2, Vector2 p3, 
        Color color,
        int thickness = 1, 
        int segments = 25, 
        float minT = 0, 
        float maxT = 1f
        )
    {
        var previousPoint = p0;

        for (var i = 1; i <= segments; i++)
        {
            var t = (i / (float) segments) * (maxT - minT) + minT;
            var currentPoint = GetCubicBezierPoint(p0, p1, p2, p3, t);

            DrawBresenhamLine(spriteBatch, previousPoint, currentPoint, color, thickness);

            previousPoint = currentPoint;
        }
    }
    
    private static Texture2D pixel;
    
    private static Texture2D GetPixel(SpriteBatch spriteBatch)
    {
        if (pixel == null)
        {
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }
        return pixel;
    }
    
    // Bresenham line function (float -> int internally)
    public static void DrawBresenhamLine(
        SpriteBatch spriteBatch,
        Vector2 start, 
        Vector2 end,
        Color color, 
        int thickness = 1
        )
    {
        int x0 = (int) MathF.Round(start.X);
        int y0 = (int) MathF.Round(start.Y);
        int x1 = (int) MathF.Round(end.X);
        int y1 = (int) MathF.Round(end.Y);
    
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
    
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
    
        int err = dx - dy;
        var pixel = GetPixel(spriteBatch);
    
        while (true)
        {
            DrawThickPixel(spriteBatch, pixel, x0, y0, thickness, color);
    
            if (x0 == x1 && y0 == y1)
            {
                break;
            }
    
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy; x0 += sx;
            }
    
            if (e2 < dx)
            {
                err += dx; y0 += sy;
            }
        }
    }
    
    private static void DrawThickPixel(
        SpriteBatch spriteBatch, 
        Texture2D pixel, 
        int x, int y, 
        int thickness, 
        Color color
        ) 
    {
        int half = thickness / 2;
        spriteBatch.Draw(pixel, new Rectangle(x - half, y - half, thickness, thickness), color);
    }
}
