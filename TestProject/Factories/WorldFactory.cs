using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;

using TestProject.Systems;
using TestProject.Components;

namespace TestProject.Factories;


class WorldFactory
{
    private readonly ContentManager content;
    private readonly SpriteBatch spriteBatch;
    private readonly OrthographicCamera camera;
    private readonly AnimationFactory animationFactory;
    
    public WorldFactory(ContentManager content, SpriteBatch spriteBatch, OrthographicCamera camera)
    {
        this.content = content;
        this.spriteBatch = spriteBatch;
        this.camera = camera;
        this.animationFactory = new AnimationFactory(content);
    }

    public Entity SpawnPlayer(World world, Vector2 position = default)
    {
        var player = world.CreateEntity();
        player.Attach(new PlayerTag());
        player.Attach(new Circle());
        player.Attach(new Position(position));
        player.Attach(new Velocity(Vector2.Zero));
        player.Attach(new MovementIntent());
        player.Attach(new CastIntent());
        player.Attach(new CameraTarget());
        player.Attach(animationFactory.CreatePlayer());

        return player;
    }

    public World TestWorld()
    {
        var world = new WorldBuilder()
            .AddSystem(new PlayerSystem())
            .AddSystem(new MovementSystem())
            .AddSystem(new VelocitySystem())
            .AddSystem(new CameraFollowSystem(camera))
            .AddSystem(new PlayerMouseSystem(camera))
            .AddSystem(new AnimationStateSystem())
            .AddSystem(new AnimationPlaySystem())
            .AddSystem(new RenderSystem(spriteBatch))
            .AddSystem(new SpriteRenderSystem(spriteBatch))
            .AddSystem(new IntentRenderSystem(spriteBatch))
            .Build();
        
        SpawnPlayer(world);
        
        var e1 = world.CreateEntity();
        e1.Attach(new Position(new Vector2(50, 50)));
        e1.Attach(new Circle());

        return world;
    }
}

