using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using TestProject.Core;

namespace TestProject.Components;

public class BezierPathFollowComponent
{
    public Vector2 P0, P1, P2, P3;
    public float Speed;
    public float T = 0;

    public float MinT = 0;
    public float DistanceTravelled = 0;
    
    public BezierPathFollowComponent(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float speed)
    {
        P0 = p0; P1 = p1; P2 = p2; P3 = p3;
        Speed = speed;
    }
    
    public Vector2 UpdateTarget(float deltaTime)
    {
        // calculate next T value
        float distance = Speed * deltaTime;
        DistanceTravelled += distance;
        T = this.TravelDistance(T, distance);
        
        // calculate minT value to cutoff path after certain length
        if (DistanceTravelled > Constants.BeamTrailLength)
        {
            MinT = this.TravelDistance(MinT, distance);
        }
        
        if (T >= 1.0f)
        {
            return P3 + (P3 - P2).NormalizedCopy() * Speed * (T - 1.0f);
        }

        return Util.GetCubicBezierPoint(P0, P1, P2, P3, Math.Clamp(T, 0, 1));
    }

    private float TravelDistance(float t, float distance)
    {
        float remaining = distance;
        Vector2 currentPos = Util.GetCubicBezierPoint(P0, P1, P2, P3, t);
        const float stepT = 0.001f;
        
        // iteratively step forward by fixed timesteps until given distance is traveled
        while (remaining > 0f && t < 1f)
        {
            float nextT = MathF.Min(t + stepT, 1f);
            Vector2 nextPos = Util.GetCubicBezierPoint(P0, P1, P2, P3, nextT);

            float segmentLength = Vector2.Distance(currentPos, nextPos);

            if (segmentLength >= remaining)
            {
                float alpha = remaining / segmentLength;
                return MathHelper.Lerp(t, nextT, alpha);
            }

            remaining -= segmentLength;
            t = nextT;
            currentPos = nextPos;
        }

        if (t >= 1.0f && remaining >= 0f)
        {
            t += remaining / Speed;
        }

        return t;
    }
}

