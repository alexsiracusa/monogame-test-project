using Flat.ECS;
using Microsoft.Xna.Framework;
using Xunit.Abstractions;

namespace Tests;

public class EntityManagerTests
{
    private readonly ITestOutputHelper testOutputHelper;

    public EntityManagerTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestAddEntity()
    {
        var manager = new EntityManager();
        var e0 = manager.AddEntity()
            .With(new Position(new Vector2(0, 0)))
            .Build();
        
        var e1 = manager.AddEntity()
            .With(new Position(new Vector2(1, 1)))
            .Build();
        
        var e2 = manager.AddEntity()
            .With(new Position(new Vector2(2, 2)))
            .With(new Velocity(new Vector2(3, 3)))
            .Build();
        
        var e3 = manager.AddEntity()
            .With(new Position(new Vector2(4, 4)))
            .With(new TargetFocus())
            .Build();
        
        testOutputHelper.WriteLine(manager.ToString());
        
        manager.RemoveEntity(1);
        
        testOutputHelper.WriteLine("-------\n");
        
        testOutputHelper.WriteLine(manager.ToString());
        
        manager.RemoveEntity(e1);
        manager.RemoveEntity(e3);
        
        testOutputHelper.WriteLine("-------\n");
        
        testOutputHelper.WriteLine(manager.ToString());
    }
}