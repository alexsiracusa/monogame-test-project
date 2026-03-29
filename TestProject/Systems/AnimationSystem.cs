using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

using TestProject.Core;
using TestProject.Components;

namespace TestProject.Systems;

internal class AnimationStateSystem : EntityProcessingSystem
{
    private ComponentMapper<AnimationComponent> animationMapper;
    private ComponentMapper<Velocity> velocityMapper;

    public AnimationStateSystem() : base(Aspect.All(typeof(AnimationComponent))) {}

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.animationMapper = mapperService.GetMapper<AnimationComponent>();
        this.velocityMapper = mapperService.GetMapper<Velocity>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var state = animationMapper.Get(entityId);
        var velocity = this.velocityMapper.Get(entityId)?.Value;
        
        this.UpdateDirection(state, velocity);
    }

    private void UpdateDirection(AnimationComponent state, Vector2? velocity)
    {
        if (velocity is null) return;
        var v = velocity.Value;

        if (v.LengthSquared() < 0.001f)
        {
            state.Action = ActionState.Idle;
            return;
        }

        Direction dir;

        if (Math.Abs(v.X) >= Math.Abs(v.Y))
        {
            dir = v.X > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            dir = v.Y > 0 ? Direction.Down : Direction.Up;
        }
        
        state.Direction = dir;
        state.Action = ActionState.Walk;
    }
}


internal enum AnimationID
{
    IdleUp,
    IdleDown,
    IdleLeft,
    IdleRight,
    WalkUp,
    WalkDown,
    WalkLeft,
    WalkRight,
    AttackUp,
    AttackDown,
    AttackLeft,
    AttackRight,
}

internal static class AnimationMapper
{
    private static readonly Dictionary<(ActionState, Direction), AnimationID> Map = new()
    {
        {(ActionState.Idle, Direction.Up), AnimationID.IdleUp}, 
        {(ActionState.Idle, Direction.Down), AnimationID.IdleDown},
        {(ActionState.Idle, Direction.Left), AnimationID.IdleLeft},
        {(ActionState.Idle, Direction.Right), AnimationID.IdleRight},
        {(ActionState.Walk, Direction.Up), AnimationID.WalkUp},
        {(ActionState.Walk, Direction.Down), AnimationID.WalkDown},
        {(ActionState.Walk, Direction.Left), AnimationID.WalkLeft},
        {(ActionState.Walk, Direction.Right), AnimationID.WalkRight},
        {(ActionState.Attack, Direction.Up), AnimationID.AttackUp},
        {(ActionState.Attack, Direction.Down), AnimationID.AttackDown},
        {(ActionState.Attack, Direction.Left), AnimationID.AttackLeft},
        {(ActionState.Attack, Direction.Right), AnimationID.AttackRight},
    };

    public static AnimationID GetAnimation(ActionState action, Direction dir)
    {
        return Map[(action, dir)];
    }
}


internal class AnimationPlaySystem : EntityProcessingSystem
{
    private ComponentMapper<AnimationComponent> animationMapper;
    private ComponentMapper<SpriteComponent> spriteMapper;

    public AnimationPlaySystem() : base(Aspect.All(typeof(AnimationComponent), typeof(SpriteComponent))) {}

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.animationMapper = mapperService.GetMapper<AnimationComponent>();
        this.spriteMapper = mapperService.GetMapper<SpriteComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var animation = animationMapper.Get(entityId);
        var sprite = spriteMapper.Get(entityId);
        
        animation.Sprite.Update(gameTime);
        
        var desiredAnimation = AnimationMapper.GetAnimation(animation.Action, animation.Direction);
        if (animation.Sprite.CurrentAnimation != desiredAnimation.ToString())
        {
            animation.Sprite.SetAnimation(desiredAnimation.ToString());
        }
        
        // update base Sprite component to current frame
        sprite.Sprite.TextureRegion = animation.Sprite.TextureRegion;
        sprite.Sprite.Origin = animation.Sprite.Origin;
    }

}
