using Microsoft.Xna.Framework;

namespace Flat.ECS;

public partial record struct Position(Vector2 Value);
public partial record struct Velocity(Vector2 Value);
public partial record struct TargetFocus();


public partial record struct Position { public override string ToString() => $"{Value}"; }
public partial record struct Velocity { public override string ToString() => $"{Value}"; }
public partial record struct TargetFocus { public override string ToString() => "Target"; }
