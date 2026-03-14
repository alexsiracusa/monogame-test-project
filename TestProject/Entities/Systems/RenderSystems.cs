using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace TestProject.Entities.Systems;

public class RenderSystem : EntityDrawSystem
{
    private readonly SpriteBatch spriteBatch;
    
    private ComponentMapper<Position> positionMapper;

    public RenderSystem(SpriteBatch spriteBatch)
        : base(Aspect.All(typeof(Position), typeof(Circle)))
    {
        this.spriteBatch = spriteBatch;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.positionMapper = mapperService.GetMapper<Position>();
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var position = this.positionMapper.Get(entity);
            this.spriteBatch.DrawCircle(position.Value, 25, 20, Color.Blue, 25f);
        }
    }
}

public class IntentRenderSystem : EntityDrawSystem
{
    private readonly SpriteBatch spriteBatch;
    
    private ComponentMapper<CastIntent> castIntentMapper;

    public IntentRenderSystem(SpriteBatch spriteBatch)
        : base(Aspect.All(typeof(CastIntent)))
    {
        this.spriteBatch = spriteBatch;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.castIntentMapper = mapperService.GetMapper<CastIntent>();
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var castIntent = this.castIntentMapper.Get(entity);
            if (castIntent.State != CastState.Active) continue;
            
            Util.DrawCubicBezier(spriteBatch, castIntent.CastPosition, castIntent.ControlPoint1, castIntent.ControlPoint2, castIntent.TargetPosition, Color.Red);
            
            spriteBatch.DrawCircle(castIntent.CastPosition, 5, 10, Color.Red, 5f);
            spriteBatch.DrawCircle(castIntent.TargetPosition, 5, 10, Color.Red, 5f);
            spriteBatch.DrawCircle(castIntent.MousePosition, 5, 10, Color.Red, 5f);
            
            spriteBatch.DrawCircle(castIntent.ControlPoint1, 5, 10, Color.Green, 5f);
            spriteBatch.DrawCircle(castIntent.ControlPoint2, 5, 10, Color.Green, 5f);
        }
    }
}