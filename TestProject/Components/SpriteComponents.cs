using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using TestProject.Core;

namespace TestProject.Components;

public class SpriteComponent(Texture2DRegion textureRegion)
{
    public Sprite Sprite = new Sprite(textureRegion);
    
    public Vector2 GetCenterOffset()
    {
        var center = new Vector2(Sprite.TextureRegion.Width / 2f, Sprite.TextureRegion.Height / 2f);
        return center - Sprite.Origin;
    }
}

public enum ActionState
{
    Idle,
    Walk,
    Attack,
}

public class AnimationComponent(AnimatedSprite sprite)
{
    public AnimatedSprite Sprite = sprite;
    public ActionState Action = ActionState.Idle;
    public Direction Direction = Direction.Down;
}
