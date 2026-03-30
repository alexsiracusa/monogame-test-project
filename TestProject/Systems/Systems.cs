using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

using TestProject.Components;

namespace TestProject.Systems;

internal class VelocitySystem : EntityUpdateSystem
{
    private ComponentMapper<Position> positionMapper;
    private ComponentMapper<Velocity> velocityMapper;
    
    public VelocitySystem() : base(Aspect.All(typeof(Position), typeof(Velocity))) {}

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.positionMapper = mapperService.GetMapper<Position>();
        this.velocityMapper = mapperService.GetMapper<Velocity>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var position = this.positionMapper.Get(entity);
            var velocity = this.velocityMapper.Get(entity);

            position.Value += velocity.Value * (float) gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}

internal class MovementSystem : EntityUpdateSystem
{
    private ComponentMapper<MovementIntent> movementIntentMapper;
    private ComponentMapper<Velocity> velocityMapper;
    
    public MovementSystem() : base(Aspect.All(typeof(MovementIntent), typeof(Velocity))) {}
    
    public override void Initialize(IComponentMapperService mapperService)
    {
        this.movementIntentMapper = mapperService.GetMapper<MovementIntent>();
        this.velocityMapper = mapperService.GetMapper<Velocity>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var moveDirection = this.movementIntentMapper.Get(entity).Direction;
            var velocity = this.velocityMapper.Get(entity);

            velocity.Value = moveDirection;
        }
    }
}


internal class CameraFollowSystem : EntityProcessingSystem
{
    private readonly Camera<Vector2> camera;
    private ComponentMapper<Position> positionMapper;

    public CameraFollowSystem(Camera<Vector2> camera)
        : base(Aspect.All(typeof(Position), typeof(CameraTarget)))
    {
        this.camera = camera;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.positionMapper = mapperService.GetMapper<Position>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var position = this.positionMapper.Get(entityId);
        this.camera.LookAt(position.Value);
    }
}
