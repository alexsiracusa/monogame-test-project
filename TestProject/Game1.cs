using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Screens.Transitions;

using TestProject.Factories;
using TestProject.Screens;

namespace TestProject;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private ScreenManager screenManager;
    
    private OrthographicCamera camera;
    private BoxingViewportAdapter viewportAdapter;
    
    private WorldFactory worldFactory;

    public Game1()
    {
        this.graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        this.screenManager = new ScreenManager();
        Components.Add(screenManager);
    }

    protected override void Initialize()
    {
        // var dm = this.GraphicsDevice.DisplayMode;
        graphics.PreferredBackBufferWidth = 1440;
        graphics.PreferredBackBufferHeight = 810;
        
        // fixed fps
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
        
        // uncap fps
        // IsFixedTimeStep = false;
        // graphics.SynchronizeWithVerticalRetrace = false;
        
        Window.AllowUserResizing = true;
        graphics.ApplyChanges();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        this.spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // define aspect ratio and camera
        this.viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 480, 270);
        this.camera = new OrthographicCamera(this.viewportAdapter);
        this.camera.Zoom = 1.0f;
        this.camera.LookAt(Vector2.Zero);
        
        // create initial screen and world
        this.worldFactory = new WorldFactory(Content, spriteBatch, camera);
        this.screenManager.ShowScreen(new GameplayScreen(this, worldFactory.TestWorld(), spriteBatch, camera));
        this.screenManager.ShowScreen(new DiagnosticsScreen(this, spriteBatch));
        
        this.viewportAdapter.Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Update MonoGame Extended input handlers
        MouseExtended.Update();
        KeyboardExtended.Update();
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}