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
        this.graphics.PreferredBackBufferWidth = 1000;
        this.graphics.PreferredBackBufferHeight = 600;
        this.graphics.ApplyChanges();
        
        Window.AllowUserResizing = true;
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        this.spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // define aspect ratio and camera
        this.viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080);
        this.camera = new OrthographicCamera(this.viewportAdapter);
        
        // create initial screen and world
        this.worldFactory = new WorldFactory(Content, spriteBatch, camera);
        this.screenManager.ShowScreen(new GameplayScreen(this, worldFactory.TestWorld(), spriteBatch, camera));
        
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