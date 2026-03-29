using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;

using TestProject.Components;
using TestProject.Core;

namespace TestProject.Systems;

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
            this.spriteBatch.DrawCircle(position.Value, 6, 20, Color.Blue, 6f);
        }
    }
}


public class SpriteRenderSystem : EntityDrawSystem
{
    private readonly SpriteBatch spriteBatch;
    
    private ComponentMapper<Position> positionMapper;
    private ComponentMapper<SpriteComponent> spriteMapper;

    public SpriteRenderSystem(SpriteBatch spriteBatch)
        : base(Aspect.All(typeof(Position), typeof(SpriteComponent)))
    {
        this.spriteBatch = spriteBatch;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.positionMapper = mapperService.GetMapper<Position>();
        this.spriteMapper = mapperService.GetMapper<SpriteComponent>();
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var position = this.positionMapper.Get(entity);
            var sprite = this.spriteMapper.Get(entity).Sprite;
            this.spriteBatch.Draw(sprite, position.Value, 0f);
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

            const int thickness = 1;
            
            Util.DrawBresenhamCubicBezier(
                spriteBatch, castIntent.CastPosition, castIntent.ControlPoint1, 
                castIntent.ControlPoint2, castIntent.TargetPosition, Color.Red, thickness
            );
            
            var launchDirection = castIntent.MousePosition - castIntent.TargetPosition;
            var endPosition = castIntent.TargetPosition + launchDirection.NormalizedCopy() * 1000;
            Util.DrawBresenhamLine(spriteBatch, castIntent.TargetPosition, endPosition, Color.Red, thickness);
            
            
            // spriteBatch.DrawLine(castIntent.TargetPosition, 2500f, endAngle, Color.Red, thickness);
            
            // spriteBatch.DrawCircle(castIntent.CastPosition, 1.5f, 10, Color.Red, 1.5f);
            // spriteBatch.DrawCircle(castIntent.TargetPosition, 1.5f, 10, Color.Red, 1.5f);
            // spriteBatch.DrawCircle(castIntent.MousePosition, 1.5f, 10, Color.Red, 1.5f);
            
            // spriteBatch.DrawCircle(castIntent.ControlPoint1, 5, 10, Color.Green, 5f);
            // spriteBatch.DrawCircle(castIntent.ControlPoint2, 5, 10, Color.Green, 5f);
        }
    }
}