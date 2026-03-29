using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using MonoGame.Extended.ECS;

using TestProject.Components;

namespace TestProject.Factories;

public class EntityFactory
{
    private World world;
    private readonly ContentManager content;
    private readonly AnimationFactory animationFactory;

    public EntityFactory(ContentManager content, AnimationFactory animationFactory)
    {
        this.content = content;
        this.animationFactory = animationFactory;
    }

    public void SetWorld(World world)
    {
        this.world = world;
    }
    
    public Entity SpawnPlayer(Vector2 position = default)
    {
        var player = world.CreateEntity();
        player.Attach(new PlayerTag());
        player.Attach(new Circle());
        player.Attach(new Position(position));
        player.Attach(new Velocity(Vector2.Zero));
        player.Attach(new MovementIntent());
        player.Attach(new CastIntent());
        // player.Attach(new CameraTarget());
        
        var sprite = animationFactory.CreatePlayer();
        player.Attach(sprite);
        player.Attach(new SpriteComponent(sprite.Sprite.TextureRegion));

        return player;
    }
}