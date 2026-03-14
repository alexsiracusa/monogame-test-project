using Microsoft.Xna.Framework;

namespace TestProject.Entities;

public class Position(Vector2 value)
{
    public Vector2 Value = value;
}

public class Velocity(Vector2 value)
{
    public Vector2 Value = value;
}

public class MovementIntent(Vector2 direction)
{
    public Vector2 Direction = direction;
    
    public MovementIntent() : this(Vector2.Zero) {}
}

public enum CastState
{
    InActive,
    Active,
    Cast,
}

public class CastIntent(Vector2 castPosition, Vector2 targetPosition, Vector2 controlPoint1, Vector2 controlPoint2)
{
    public CastState State = CastState.InActive;
    public Vector2 CastPosition = castPosition;
    public Vector2 TargetPosition = targetPosition;
    public Vector2 ControlPoint1 = controlPoint1;
    public Vector2 ControlPoint2 = controlPoint2;
    
    public CastIntent() : this(Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero) { }
}

public class Circle();
public class CameraTarget();
public class PlayerTag();

