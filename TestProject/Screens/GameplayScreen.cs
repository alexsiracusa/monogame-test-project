using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Screens;


namespace TestProject.Screens;

public class GameplayScreen : GameScreen
{
    private SpriteBatch spriteBatch;
    private OrthographicCamera camera;
    private World world;

    public GameplayScreen(Game game, World world, SpriteBatch spriteBatch, OrthographicCamera camera) : base(game)
    {
        this.world = world;
        this.spriteBatch = spriteBatch;
        this.camera = camera;
        
        DrawWhenInactive = true; 
        UpdateWhenInactive = true;
    }

    public override void LoadContent()
    {
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        this.world.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        // Draw background and gutters
        GraphicsDevice.Clear(Color.Black);
        GraphicsDevice.RasterizerState = new RasterizerState { ScissorTestEnable = true };
        GraphicsDevice.Clear(Color.DarkGray);
        
        // Draw sprites
        this.spriteBatch.Begin
        (
            transformMatrix: this.camera.GetViewMatrix(),
            samplerState: SamplerState.PointClamp
        );
        this.world.Draw(gameTime);
        this.spriteBatch.End();
    }
}