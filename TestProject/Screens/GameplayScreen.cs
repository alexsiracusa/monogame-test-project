using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;

using TestProject.Factories;

namespace TestProject.Screens;

public class GameplayScreen : GameScreen
{
    private readonly SceneFactory sceneFactory;
    private readonly string mapName;
    
    private SpriteBatch spriteBatch;
    private BoxingViewportAdapter viewportAdapter;
    private OrthographicCamera camera;
    private GameScene scene;

    public GameplayScreen(Game game, SpriteBatch spriteBatch, SceneFactory sceneFactory, string mapName) : base(game)
    {
        this.sceneFactory = sceneFactory;
        this.mapName = mapName;
        this.spriteBatch = spriteBatch;
        
        DrawWhenInactive = true; 
        UpdateWhenInactive = true;
    }

    public override void LoadContent()
    {
        // create camera
        this.viewportAdapter = new BoxingViewportAdapter(Game.Window, GraphicsDevice, 480, 270);
        this.camera = new OrthographicCamera(viewportAdapter);
        this.camera.Zoom = 1.0f;
        this.camera.LookAt(Vector2.Zero);
        
        // create scene
        this.scene = sceneFactory.CreateScene(mapName, camera);
        
        viewportAdapter.Reset();
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        this.scene.World.Update(gameTime);
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
        this.scene.World.Draw(gameTime);
        this.spriteBatch.End();
    }

    public override void UnloadContent()
    {
        scene?.Dispose();
        base.UnloadContent();
    }
}