using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;
// using MonoGame.Extended.Graphics;
// using MonoGame.Extended.Content;
// using MonoGame.Extended.Serialization.Json;

using TestProject.Entities;
using TestProject.Entities.Systems;

namespace TestProject;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private OrthographicCamera camera;
    private BoxingViewportAdapter viewportAdapter;
    
    private World world;
    private Entity playerEntity;

    public Game1()
    {
        this.graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // var dm = this.GraphicsDevice.DisplayMode;
        this.graphics.PreferredBackBufferWidth = 1000;
        this.graphics.PreferredBackBufferHeight = 600;
        this.graphics.ApplyChanges();
        
        Window.AllowUserResizing = true;

        this.viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080);
        this.camera = new OrthographicCamera(this.viewportAdapter);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        this.spriteBatch = new SpriteBatch(GraphicsDevice);
        this.viewportAdapter.Reset();
        
        // load player spritesheet
        // var atlas = Content.Load<Texture2DAtlas>("textures/player");
        // var playerSpriteSheet = new SpriteSheet("SpriteSheet/adventurer", atlas);
        // var playerSpriteSheet = Content.Load<SpriteSheet>("spritesheets/player", new JsonContentLoader());
        
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
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Update mouse state
        MouseExtended.Update();
        KeyboardExtended.Update();

        this.world.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
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
        
        base.Draw(gameTime);
    }
}