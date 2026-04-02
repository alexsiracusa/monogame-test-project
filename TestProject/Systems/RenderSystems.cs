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
            if (castIntent.State != CastState.Aiming) continue;

            var color = Color.LightGray;
            const int thickness = 1;

            if (castIntent.IsStraight())
            {
                Util.DrawStabilizedBresenhamLine(
                    spriteBatch, castIntent.CastPosition, castIntent.TargetPosition, 
                    color, thickness
                );
            }
            else
            {
                Util.DrawBresenhamCubicBezier(
                    spriteBatch, castIntent.CastPosition, castIntent.ControlPoint1, 
                    castIntent.ControlPoint2, castIntent.TargetPosition, 
                    color, thickness, segments: 30
                );
            
                var launchDirection = castIntent.MousePosition - castIntent.TargetPosition;
                var endPosition = castIntent.TargetPosition + launchDirection.NormalizedCopy() * Constants.BeamMaxTravelDistance;
                Util.DrawStabilizedBresenhamLine(spriteBatch, castIntent.TargetPosition, endPosition, color, thickness);
            }
            
            
            // spriteBatch.DrawLine(castIntent.TargetPosition, 2500f, endAngle, Color.Red, thickness);
            
            // spriteBatch.DrawCircle(castIntent.CastPosition, 1.5f, 10, Color.Red, 1.5f);
            // spriteBatch.DrawCircle(castIntent.TargetPosition, 1.5f, 10, Color.Red, 1.5f);
            // spriteBatch.DrawCircle(castIntent.MousePosition, 1.5f, 10, Color.Red, 1.5f);
            
            // spriteBatch.DrawCircle(castIntent.ControlPoint1, 5, 10, Color.Green, 5f);
            // spriteBatch.DrawCircle(castIntent.ControlPoint2, 5, 10, Color.Green, 5f);
        }
    }
}


internal class PathRenderSystem : EntityDrawSystem
{
    private readonly SpriteBatch spriteBatch;
    
    private ComponentMapper<BezierPathFollowComponent> pathMapper;
    private ComponentMapper<Position> positionMapper;

    public PathRenderSystem(SpriteBatch spriteBatch)
        : base(Aspect.All(typeof(BezierPathFollowComponent), typeof(Position)))
    {
        this.spriteBatch = spriteBatch;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.pathMapper = mapperService.GetMapper<BezierPathFollowComponent>();
        this.positionMapper = mapperService.GetMapper<Position>();
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var path = this.pathMapper.Get(entity);
            var position = this.positionMapper.Get(entity);
            
            var color = Color.White;
            const int thickness = 1;

            Util.DrawStabilizedBresenhamCubicBezier(
                spriteBatch, path.P0, path.P1, path.P2, path.P3, color, 
                thickness, segments: 25, minT: path.MinT, maxT: Math.Clamp(path.T, 0, 1)
            );

            if (path.T > 1.0f)
            {
                var dir = (path.P3 - path.P2).NormalizedCopy();
                var start = path.P3 + dir * path.Speed * Math.Max(0, path.MinT - 1);
                var end = position.Value;
                Util.DrawStabilizedBresenhamLine(spriteBatch, start, end, color, thickness);
            }
            
            // Draw target and position for debugging
            // spriteBatch.DrawCircle(position.Value, 2f, 10, Color.Red, 2f);
            // spriteBatch.DrawCircle(path.P3, 2f, 10, Color.Red, 2f);
        }
    }

}