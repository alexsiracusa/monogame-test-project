using System;
using System.Collections.Generic;

namespace Flat.Graphics;

public class World
{
    private Dictionary<Type, Array> componentPools = new Dictionary<Type, Array>();
    private int entityCount = 0;
    private const int MaxEntities = 10000;

    private T[] GetPool<T>()
    {
        var type = typeof(T);
        if (!this.componentPools.TryGetValue(type, out var pool))
        {
            pool = new T[MaxEntities];
            componentPools[type] = pool;
        }
        return pool as T[];
    }

    public int CreateEntity()
    {
        entityCount++;
        return entityCount;
    }

    public void AddComponent<T>(int entityId, T component)
    {
        T[] pool = this.GetPool<T>();
        pool[entityId] = component;
    }
}