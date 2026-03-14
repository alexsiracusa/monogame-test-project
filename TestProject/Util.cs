using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace TestProject;

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

    public static void DrawCubicBezier(SpriteBatch spriteBatch, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Color color,
        int segments = 20, float minT = 0, float maxT = 1f)
    {
        var previousPoint = p0;

        for (var i = 1; i <= segments; i++)
        {
            var t = (i / (float) segments) * (maxT - minT) + minT;
            var currentPoint = GetCubicBezierPoint(p0, p1, p2, p3, t);

            // Assuming you have a DrawLine helper method
            spriteBatch.DrawLine(previousPoint, currentPoint, color, 2f);

            previousPoint = currentPoint;
        }
    }
}