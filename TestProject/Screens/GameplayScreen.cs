using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Graphics;

using TestProject.Systems;
using TestProject.Components;

namespace TestProject.Screens;

public class GameplayScreen : GameScreen
{
    private SpriteBatch spriteBatch;

    private OrthographicCamera camera;
    private BoxingViewportAdapter viewportAdapter;
    
    private World world;
    private Entity playerEntity;

    public GameplayScreen(Game game, SpriteBatch spriteBatch) : base(game)
    {
        this.spriteBatch = spriteBatch;
        this.viewportAdapter = new BoxingViewportAdapter(Game.Window, GraphicsDevice, 1920, 1080);
        this.camera = new OrthographicCamera(this.viewportAdapter);
    }

    public override void LoadContent()
    {
        this.viewportAdapter.Reset();
        
        // load player spritesheet
        var atlas = Content.Load<Texture2DAtlas>("spritesheets/player");
        var playerSpriteSheet = new SpriteSheet("textures/player", atlas);
        
        this.world = new WorldBuilder()
            .AddSystem(new PlayerSystem())
            .AddSystem(new MovementSystem())
            .AddSystem(new VelocitySystem())
            .AddSystem(new CameraFollowSystem(camera))
            .AddSystem(new PlayerMouseSystem(camera))
            .AddSystem(new RenderSystem(spriteBatch))
            .AddSystem(new IntentRenderSystem(spriteBatch))
            .Build();
        
        playerEntity = this.world.CreateEntity();
        playerEntity.Attach(new PlayerTag());
        playerEntity.Attach(new Circle());
        playerEntity.Attach(new Position(new Vector2(0, 0)));
        playerEntity.Attach(new Velocity(new Vector2(0, 0)));
        playerEntity.Attach(new MovementIntent());
        playerEntity.Attach(new CastIntent());
        playerEntity.Attach(new CameraTarget());
        
        var e1 = this.world.CreateEntity();
        e1.Attach(new Position(new Vector2(200, 200)));
        e1.Attach(new Circle());
        
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        this.world.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        // Draw background and gutters
        GraphicsDevice.Clear(Color.Black);
        GraphicsDevice.RasterizerState = new RasterizerState { ScissorTestEnable = true };
        GraphicsDevice.Clear(Color.WhiteSmoke);
        
        // Draw sprites
        this.spriteBatch.Begin
        (
            transformMatrix: this.camera.GetViewMatrix(),
            samplerState: SamplerState.PointClamp
        );
        this.world.Draw(gameTime);
        this.spriteBatch.End();
    }
}