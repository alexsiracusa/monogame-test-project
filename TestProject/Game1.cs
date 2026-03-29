using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

using TestProject.Factories;
using TestProject.Screens;

namespace TestProject;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    
    private readonly ScreenManager screenManager;
    private SceneFactory sceneFactory;

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
        this.sceneFactory = new SceneFactory(Services, spriteBatch);
        
        // Set screens
        this.screenManager.ShowScreen(new GameplayScreen(this, spriteBatch, sceneFactory, "TestWorld"));
        this.screenManager.ShowScreen(new DiagnosticsScreen(this, spriteBatch));
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