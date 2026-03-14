using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ViewportAdapters;
using TestProject.Entities;

namespace TestProject;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private Camera<Vector2> camera;
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

        this.viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 600, 600);
        this.camera = new OrthographicCamera(this.viewportAdapter);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        this.spriteBatch = new SpriteBatch(GraphicsDevice);
        this.viewportAdapter.Reset();
        
        this.world = new WorldBuilder()
            .AddSystem(new PlayerSystem())
            .AddSystem(new MovementSystem())
            .AddSystem(new VelocitySystem())
            .AddSystem(new CameraFollowSystem(this.camera))
            .AddSystem(new RenderSystem(GraphicsDevice, this.spriteBatch, this.camera))
            .Build();
        
        playerEntity = this.world.CreateEntity();
        playerEntity.Attach(new PlayerTag());
        playerEntity.Attach(new Circle());
        playerEntity.Attach(new Position(new Vector2(0, 0)));
        playerEntity.Attach(new Velocity(new Vector2(0, 0)));
        playerEntity.Attach(new MovementIntent(new Vector2(0, 0)));
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

        this.world.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Draw background and gutters
        GraphicsDevice.Clear(Color.Black);
        GraphicsDevice.RasterizerState = new RasterizerState { ScissorTestEnable = true };
        GraphicsDevice.Clear(Color.HotPink);
        
        // Draw sprites
        this.world.Draw(gameTime);
        base.Draw(gameTime);
    }
}