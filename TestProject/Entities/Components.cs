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

public class CastIntent(Vector2 castPosition, Vector2 targetPosition, Vector2 mousePosition)
{
    public CastState State = CastState.InActive;
    public Vector2 CastPosition = castPosition;
    public Vector2 TargetPosition = targetPosition;
    public Vector2 MousePosition = mousePosition;
    
    public Vector2 ControlPoint1
    {
        get
        {
            // 1. Get the vector from the End Point to its Control Point
            var v2 = ControlPoint2 - TargetPosition;

            // 2. Define the axis of the curve (the line between start and end)
            var curveAxis = TargetPosition - CastPosition;
    
            if (curveAxis.LengthSquared() > 0)
                curveAxis.Normalize();

            // 3. To mirror across the perpendicular bisector:
            // We invert the component of the vector that is PARALLEL to the curve axis.
            // Reflection across the bisector: V_mirrored = V - 2 * (V dot Axis) * Axis
            var dot = Vector2.Dot(v2, curveAxis);
            var mirroredVector = v2 - 2 * dot * curveAxis;

            // 4. P1 is the Start Point + this mirrored vector
            // return CastPosition + 0.1f * mirroredVector;

            return CastPosition;
        }
    }

    public Vector2 ControlPoint2 => TargetPosition - 4 * (MousePosition - TargetPosition);

    public CastIntent() : this(Vector2.Zero, Vector2.Zero, Vector2.Zero) { }
}

public class Circle();
public class CameraTarget();
public class PlayerTag();

