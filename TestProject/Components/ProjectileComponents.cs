using System;

using Microsoft.Xna.Framework;
using TestProject.Core;

namespace TestProject.Components;

class BezierPathFollower
{
    public Vector2 P0, P1, P2, P3;
    public float Speed;
    public float T = 0;
    
    public BezierPathFollower(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float speed)
    {
        P0 = p0; P1 = p1; P2 = p2; P3 = p3;
        Speed = speed;
    }
    
    public Vector2 UpdateTarget(float deltaTime)
    {
        if (T >= 1.0f) return Util.GetCubicBezierPoint(P0, P1, P2, P3, 1.0f);

        // 1. Calculate the velocity (derivative) at the current T
        // Formula: B'(t) = 3(1-t)^2(P1-P0) + 6(1-t)t(P2-P1) + 3t^2(P3-P2)
        Vector2 velocity = Util.GetCubicBezierVelocity(P0, P1, P2, P3, T);
        float magnitude = velocity.Length();

        // 2. Prevent division by zero if points are overlapping
        if (magnitude > 0.001f)
        {
            // The distance we want to travel is (Speed * deltaTime)
            // Delta T = Distance / Speed_at_this_point
            T += (Speed * deltaTime) / magnitude;
        }
        else
        {
            // Fallback for singular points
            T += deltaTime * 0.1f; 
        }

        T = Math.Clamp(T, 0, 1);
        return Util.GetCubicBezierPoint(P0, P1, P2, P3, T);
    }
}

