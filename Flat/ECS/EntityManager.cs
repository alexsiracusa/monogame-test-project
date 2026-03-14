using System;
using System.Collections.Generic;
using System.Linq;

namespace Flat.ECS;

public struct EntityRecord
{
    public Archetype Archetype;
    public int Row;
    
    public EntityRecord(Archetype archetype, int row)
    {
        this.Archetype = archetype;
        this.Row = row; 
    }
}

public class EntityBuilder
{
    private readonly EntityManager manager;
    private readonly List<object> components = [];

    public EntityBuilder(EntityManager manager)
    {
        this.manager = manager;
    }

    public EntityBuilder With<T>(T component) where T : struct
    {
        this.components.Add(component);
        return this;
    }

    public int Build()
    {
        var types = this.components.Select(c => c.GetType()).ToArray();
        var archetype = this.manager.GetOrCreateArchetype(types);
        var entityId = manager.NextEntityId();
        
        var row = archetype.AddEntity(entityId, this.components.ToArray());
        manager.RegisterEntityRecord(entityId, archetype, row);
        
        return entityId;
    }
}

public class EntityManager
{
    private readonly List<Archetype> archetypes = [];
    private readonly Dictionary<int, EntityRecord> entityRecords = new();
    private int nextEntityId = 0;
    
    public int NextEntityId()
    {
        nextEntityId++;
        return nextEntityId;
    }

    public EntityBuilder AddEntity()
    {
        return new EntityBuilder(this);
    }
    
    internal void RegisterEntityRecord(int entityId, Archetype archetype, int row)
    {
        entityRecords[entityId] = new EntityRecord(archetype, row);
    }

    public void RemoveEntity(int entityId)
    {
        if (!this.entityRecords.TryGetValue(entityId, out var record)) return;
        var movedEntityId = record.Archetype.RemoveEntity(record.Row);

        if (movedEntityId != -1)
        {
            var movedRecord = this.entityRecords[movedEntityId];
            movedRecord.Row = record.Row;
            this.entityRecords[movedEntityId] = movedRecord;
        }
        
        this.entityRecords.Remove(entityId);
    }

    public Archetype GetOrCreateArchetype(IEnumerable<Type> types)
    {
        var typeSet = new HashSet<Type>(types);
        var existing = this.archetypes.FirstOrDefault(a => a.ComponentTypes.SetEquals(typeSet));

        if (existing != null)
        {
            return existing;
        }

        var newArchetype = new Archetype(types);
        this.archetypes.Add(newArchetype);
        return newArchetype;
    }

    public override string ToString()
    {
        return this.archetypes.Aggregate("", (current, archetype) => current + (archetype + "\n"));
    }
}