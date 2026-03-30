using Microsoft.Xna.Framework;
using TestProject.Core;

namespace TestProject.Components;

public class MovementIntent(Vector2 direction)
{
    public Vector2 Direction = direction;
    
    public MovementIntent() : this(Vector2.Zero) {}
}

public enum CastState
{
    Inactive,
    Aiming,
    Fired,
}

public class CastIntent(Vector2 castPosition, Vector2 targetPosition, Vector2 mousePosition)
{
    public CastState State = CastState.Inactive;
    public Vector2 CastPosition = castPosition;
    public Vector2 TargetPosition = targetPosition;
    public Vector2 MousePosition = mousePosition;
    
    public Vector2 ControlPoint1 => CastPosition;

    public Vector2 ControlPoint2 => TargetPosition - 3.5f * (MousePosition - TargetPosition).ClampLength(0, 96);

    public CastIntent() : this(Vector2.Zero, Vector2.Zero, Vector2.Zero) { }

    public bool IsStraight() => (TargetPosition - MousePosition).Length() < 12;

    public BezierPathFollowComponent PathFollow()
    {
        return new BezierPathFollowComponent(
            CastPosition, ControlPoint1, 
            ControlPoint2, TargetPosition, 
            Constants.BeamProjectileSpeed
        );
    }
}
