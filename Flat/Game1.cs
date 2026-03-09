using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Flat.ECS;

namespace Flat;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        var manager = new EntityManager();
        var e0 = manager.AddEntity()
            .With(new Position(new Vector2(0, 0)))
            .Build();
        
        var e1 = manager.AddEntity()
            .With(new Position(new Vector2(1, 1)))
            .Build();
        
        var e2 = manager.AddEntity()
            .With(new Position(new Vector2(2, 2)))
            .With(new Velocity(new Vector2(3, 3)))
            .Build();
        
        var e3 = manager.AddEntity()
            .With(new Position(new Vector2(4, 4)))
            .With(new TargetFocus())
            .Build();
        
        System.Console.WriteLine(manager);
        
        manager.RemoveEntity(1);
        
        System.Console.WriteLine("\n-------\n");
        
        System.Console.WriteLine(manager);
        
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}