using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace TestProject.Entities;

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

            position.Value += 0.001f * velocity.Value * gameTime.ElapsedGameTime.Milliseconds;
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

public class RenderSystem : EntityDrawSystem
{
    private readonly GraphicsDevice graphics;
    private readonly SpriteBatch spriteBatch;
    private readonly Camera<Vector2> camera;
    
    private ComponentMapper<Position> positionMapper;

    public RenderSystem(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Camera<Vector2> camera)
        : base(Aspect.All(typeof(Position), typeof(Circle)))
    {
        this.graphics = graphicsDevice;
        this.spriteBatch = spriteBatch;
        this.camera = camera;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.positionMapper = mapperService.GetMapper<Position>();
    }

    public override void Draw(GameTime gameTime)
    {
        // graphics.Clear(Color.DimGray);
        // graphics.Viewport = this.viewportAdapter.Viewport;
        // graphics.SetRenderTarget(this.camera.BoundingRectangle);
        // graphics.Viewport = camera.BoundingRectangle;
        graphics.Clear(Color.HotPink);
        
        this.spriteBatch.Begin
        (
            transformMatrix: this.camera.GetViewMatrix(),
            samplerState: SamplerState.PointClamp
        );
        
        foreach (var entity in ActiveEntities)
        {
            var position = this.positionMapper.Get(entity);
            this.spriteBatch.FillRectangle(position.Value, new SizeF(50, 50), Color.Blue);
        }
        
        this.spriteBatch.End();
    }
}

internal class PlayerSystem : EntityProcessingSystem
{
    private ComponentMapper<MovementIntent> movementIntentMapper;
    
    public PlayerSystem() : base(Aspect.All(typeof(PlayerTag), typeof(MovementIntent))) {}

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.movementIntentMapper = mapperService.GetMapper<MovementIntent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var movementIntent = movementIntentMapper.Get(entityId);
        var dir = Vector2.Zero;

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            dir.X -= 1;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            dir.X += 1;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            dir.Y -= 1;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            dir.Y += 1;
        }

        if (dir != Vector2.Zero)
        {
            dir.Normalize();
        }

        movementIntent.Direction = 250 * dir;
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
