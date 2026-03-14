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
}

public class Circle();
public class CameraTarget();
public class PlayerTag();

