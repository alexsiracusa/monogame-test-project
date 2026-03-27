using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

using MonoGame.Extended.Graphics;

using TestProject.Components;

namespace TestProject.Factories;

public class AnimationFactory
{
    private readonly ContentManager content;
    private readonly Dictionary<string, SpriteSheet> sheetCache = new();

    public AnimationFactory(ContentManager content)
    {
        this.content = content;
    }

    public AnimationComponent CreatePlayer()
    {
        // check if spritesheet is already loaded
        if (sheetCache.TryGetValue("player", out var sheet))
        {
            return new AnimationComponent(new AnimatedSprite(sheet, "idle-left"));
        }
        
        // otherwise load spritesheet and define animations
        var atlas = content.Load<Texture2DAtlas>("spritesheets/player");
        sheet = new SpriteSheet("textures/player", atlas);

        var duration = TimeSpan.FromSeconds(0.25);

        var directions = new[] { "Down", "Left", "Up", "Right" };

        foreach (var direction in directions)
        {
            sheet.DefineAnimation($"Idle{direction}", builder =>
            {
                builder.IsLooping(true)
                    .AddFrame($"Idle{direction}", duration);
            });

            sheet.DefineAnimation($"Walk{direction}", builder =>
            {
                builder.IsLooping(true)
                    .AddFrame($"Walk{direction}/0001", duration)
                    .AddFrame($"Walk{direction}/0002", duration)
                    .AddFrame($"Walk{direction}/0003", duration)
                    .AddFrame($"Walk{direction}/0004", duration);
            });
        }
        
        sheetCache.Add("player", sheet);
        return new AnimationComponent(new AnimatedSprite(sheet, "IdleLeft"));
    }
    
    // Core logic to turn a JSON/Atlas into a SpriteSheet
    // private SpriteSheet LoadBaseSheet(string path)
    // {
    //     
    // }
}