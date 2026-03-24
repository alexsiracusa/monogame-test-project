using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;


using TestProject.Screens;

namespace TestProject;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private ScreenManager screenManager;

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
        this.screenManager.ShowScreen(new GameplayScreen(this, spriteBatch));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Update mouse state
        MouseExtended.Update();
        KeyboardExtended.Update();
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}