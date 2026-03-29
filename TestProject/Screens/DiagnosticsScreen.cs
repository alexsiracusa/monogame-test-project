using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

namespace TestProject.Screens;

public class DiagnosticsScreen : GameScreen
{
    private SpriteBatch spriteBatch;
    private SpriteFont font;
    private double fps;
    private double lastFrameMs;
    
    // We use a constructor to pass the Game instance to the base GameScreen
    public DiagnosticsScreen(Game game, SpriteBatch spriteBatch) : base(game)
    {
        this.spriteBatch = spriteBatch;
    }

    public override void LoadContent()
    {
        font = Content.Load<SpriteFont>("fonts/CourierNew");
    }

    public override void Update(GameTime gameTime)
    {
        lastFrameMs = gameTime.ElapsedGameTime.TotalMilliseconds;
        
        if (lastFrameMs > 0)
            fps = 1000.0 / lastFrameMs;
    }

    public override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin();
        
        var fpsText = $"FPS: {fps:0}";
        var timeText = $"Frame: {lastFrameMs:0.00}ms";
        
        spriteBatch.DrawString(font, fpsText, new Vector2(10, 10), Color.Red);
        spriteBatch.DrawString(font, timeText, new Vector2(10, 30), Color.Red);
        
        spriteBatch.End();
    }
}