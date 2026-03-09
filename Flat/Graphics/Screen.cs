using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flat.Graphics;

public abstract class Screen
{
    protected GraphicsDevice Graphics;
    protected RenderTarget2D Target;

    protected Screen(GraphicsDevice graphics, int width, int height)
    {
        this.Graphics = graphics;
        this.Target = new RenderTarget2D(this.Graphics, width, height);
    }
    
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);
    
    public Texture2D GetTexture() => Target;
}


public class GameplayScreen : Screen
{
    private World world;
    private Camera camera;

    public GameplayScreen(GraphicsDevice graphics, int width, int height) : base(graphics, width, height)
    {
        world = new World();
        camera = new Camera(width, height);
    }

    public override void Update(GameTime gameTime)
    {
        
    }

    public override void Draw(GameTime gameTime)
    {
        this.Graphics.SetRenderTarget(this.Target);
        this.Graphics.Clear(Color.LightPink);
    }
}