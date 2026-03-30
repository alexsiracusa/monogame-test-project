using Microsoft.Xna.Framework;

using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

using TestProject.Components;
using TestProject.Core;
using TestProject.Factories;

namespace TestProject.Systems;

internal class CastingSystem : EntityProcessingSystem
{
    private readonly EntityFactory entityFactory;
    private ComponentMapper<CastIntent> castIntentMapper;

    public CastingSystem(EntityFactory entityFactory) : base(Aspect.All(typeof(CastIntent)))
    {
        this.entityFactory = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.castIntentMapper = mapperService.GetMapper<CastIntent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var castIntent = this.castIntentMapper.Get(entityId);
        if (castIntent.State != CastState.Fired) return;
        
        castIntent.State = CastState.Inactive;
        entityFactory.SpawnBeam(castIntent);
    }

}


internal class PathFollowSystem : EntityProcessingSystem
{
    private ComponentMapper<BezierPathFollowComponent> pathMapper;
    private ComponentMapper<Position> positionMapper;
    private ComponentMapper<Velocity> velocityMapper;
    
    public PathFollowSystem() 
        : base(Aspect.All(typeof(BezierPathFollowComponent), typeof(Position), typeof(Velocity))) { }

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.pathMapper = mapperService.GetMapper<BezierPathFollowComponent>();
        this.positionMapper = mapperService.GetMapper<Position>();
        this.velocityMapper = mapperService.GetMapper<Velocity>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var path = this.pathMapper.Get(entityId);
        var position = this.positionMapper.Get(entityId);
        var velocity = this.velocityMapper.Get(entityId);
        
        var deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        var target = path.UpdateTarget(deltaTime);
        velocity.Value = (target -  position.Value) / deltaTime;
        
        if (path.DistanceTravelled > Constants.BeamMaxTravelDistance)
        {
            DestroyEntity(entityId);
        }
    }

}


internal class DespawnProjectileSystem : EntityProcessingSystem
{
    private ComponentMapper<BezierPathFollowComponent> pathMapper;
    private ComponentMapper<Position> positionMapper;
    private ComponentMapper<Velocity> velocityMapper;
    
    public DespawnProjectileSystem() 
        : base(Aspect.All(typeof(BezierPathFollowComponent), typeof(Position), typeof(Velocity))) { }

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.pathMapper = mapperService.GetMapper<BezierPathFollowComponent>();
        this.positionMapper = mapperService.GetMapper<Position>();
        this.velocityMapper = mapperService.GetMapper<Velocity>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var path = this.pathMapper.Get(entityId);
        var position = this.positionMapper.Get(entityId);
        var velocity = this.velocityMapper.Get(entityId);
        
        var deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        var target = path.UpdateTarget(deltaTime);

        velocity.Value = (target -  position.Value) / deltaTime;
    }

}