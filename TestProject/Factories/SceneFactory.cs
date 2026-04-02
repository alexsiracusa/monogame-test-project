using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.ECS;

using TestProject.Systems;
using TestProject.Components;

namespace TestProject.Factories;


public class GameScene : IDisposable
{
    public readonly ContentManager Content;
    public readonly World World;
    
    public GameScene(ContentManager content, World world)
    {
        Content = content;
        World = world;
    }

    public void Dispose()
    {
        World.Dispose();
        Content.Dispose();
    }
}

public class SceneFactory
{
    private readonly IServiceProvider services;
    private readonly SpriteBatch spriteBatch;
    
    public SceneFactory(IServiceProvider services, SpriteBatch spriteBatch)
    {
        this.services = services;
        this.spriteBatch = spriteBatch;
    }

    public GameScene CreateScene(string mapName, OrthographicCamera camera)
    {
        return TestWorld(camera);
    }

    private GameScene TestWorld(OrthographicCamera camera)
    {
        var sceneContent = new ContentManager(this.services, "Content");
        var animationFactory = new AnimationFactory(sceneContent);
        var entityFactory = new EntityFactory(sceneContent, animationFactory);

        var world = new WorldBuilder()
            // Mechanics
            .AddSystem(new PlayerKeyboardSystem())
            
            // Movement 
            .AddSystem(new MovementSystem())
            .AddSystem(new PathFollowSystem())
            .AddSystem(new VelocitySystem())
            .AddSystem(new CameraFollowSystem(camera))
            
            // Attack
            .AddSystem(new PlayerMouseSystem(camera))
            .AddSystem(new CastingSystem(entityFactory))
            
            // Animations
            .AddSystem(new AnimationStateSystem())
            .AddSystem(new AnimationPlaySystem())
            
            // Rendering
            .AddSystem(new RenderSystem(spriteBatch))
            .AddSystem(new SpriteRenderSystem(spriteBatch))
            .AddSystem(new IntentRenderSystem(spriteBatch))
            .AddSystem(new PathRenderSystem(spriteBatch))
            .Build();
        
        var e1 = world.CreateEntity();
        e1.Attach(new Position(new Vector2(50, 50)));
        e1.Attach(new Circle());
        
        entityFactory.SetWorld(world);
        entityFactory.SpawnPlayer();

        return new GameScene(
            content: sceneContent, 
            world: world
        );
    }
}

