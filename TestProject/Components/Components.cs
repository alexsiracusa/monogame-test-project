using Microsoft.Xna.Framework;

namespace TestProject.Components;

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

public class CastIntent(Vector2 castPosition, Vector2 targetPosition, Vector2 mousePosition)
{
    public CastState State = CastState.InActive;
    public Vector2 CastPosition = castPosition;
    public Vector2 TargetPosition = targetPosition;
    public Vector2 MousePosition = mousePosition;
    
    public Vector2 ControlPoint1 => CastPosition;

    public Vector2 ControlPoint2 => TargetPosition - 4 * (MousePosition - TargetPosition);

    public CastIntent() : this(Vector2.Zero, Vector2.Zero, Vector2.Zero) { }
}

public class Circle();
public class CameraTarget();
public class PlayerTag();

