using Microsoft.Xna.Framework;
using Flat.Graphics;

namespace Flat.ECS;

public interface IUpdateSystem 
{
    void Update(GameTime gameTime);
}

public interface IDrawSystem 
{
    void Draw(GameTime gameTime); // Or pass SpriteBatch here
}

public class CameraFollowSystem : IUpdateSystem
{
    private Camera camera;
    private World world;
    
    public void Update(GameTime gameTime)
    {
        // Find the player (or focus target)
        // var query = new QueryDescription().WithAll<Position, TargetFocus>();
        // world.Query(in query, (ref Position pos) => 
        // {
        //     // Smoothly move camera to player position
        //     camera.Position = Vector2.Lerp(camera.Position, pos.Value, 0.1f);
        // });
    }
}