using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Input;

namespace TestProject.Entities.Systems;

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
        this.HandleMovement(entityId);
    }

    private void HandleMovement(int entityId)
    {
        var movementIntent = movementIntentMapper.Get(entityId);
        var dir = Vector2.Zero;

        if (KeyboardExtended.GetState().IsKeyDown(Keys.A))
        {
            dir.X -= 1;
        }
        if (KeyboardExtended.GetState().IsKeyDown(Keys.D))
        {
            dir.X += 1;
        }
        if (KeyboardExtended.GetState().IsKeyDown(Keys.W))
        {
            dir.Y -= 1;
        }
        if (KeyboardExtended.GetState().IsKeyDown(Keys.S))
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


internal class PlayerMouseSystem : EntityProcessingSystem
{
    private ComponentMapper<Position> positionMapper;
    private ComponentMapper<CastIntent> castIntentMapper;
    
    private readonly OrthographicCamera camera;

    public PlayerMouseSystem(OrthographicCamera camera)
        : base(Aspect.All(typeof(PlayerTag), typeof(CastIntent)))
    {
        this.camera = camera;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        this.positionMapper = mapperService.GetMapper<Position>();
        this.castIntentMapper = mapperService.GetMapper<CastIntent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        this.HandleCast(entityId);
    }
    
    private void HandleCast(int entityId)
    {
        var castIntent = this.castIntentMapper.Get(entityId);
        var position = this.positionMapper.Get(entityId);
        
        if (MouseExtended.GetState().WasButtonPressed(MouseButton.Left))
        {
            var mouse = camera.ScreenToWorld(MouseExtended.GetState().Position.ToVector2());
            
            castIntent.State = CastState.Active;
            castIntent.CastPosition = position.Value;
            castIntent.TargetPosition = mouse;
            castIntent.MousePosition = mouse;
        }
        else if (MouseExtended.GetState().IsButtonDown(MouseButton.Left))
        {
            var mouse = camera.ScreenToWorld(MouseExtended.GetState().Position.ToVector2());
            castIntent.CastPosition = position.Value;
            castIntent.MousePosition = mouse;
        }
        else if (MouseExtended.GetState().WasButtonReleased(MouseButton.Left))
        {
            castIntent.State = CastState.InActive;
        }
    }
}