using Microsoft.Xna.Framework;

namespace Flat.Graphics;

public class Camera
{
    public Vector2 Position;
    public float Zoom { get; set; } = 1.0f;
    public float Rotation { get; set; } = 0f;
    public Vector2 Origin { get; private set; }
    
    public Camera(int viewportWidth, int viewportHeight)
    {
        // Center of the screen
        Origin = new Vector2(viewportWidth / 2f, viewportHeight / 2f);
    }

    public Matrix GetTransformation()
    {
        return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(Origin.X, Origin.Y, 0));
    }
}
