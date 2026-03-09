using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Flat.ECS;

public interface IComponentColumn
{
    void SetData(int row, object data);
    void MoveData(int sourceRow, int targetRow, IComponentColumn targetColumn);
    void Swap(int rowToRemove, int lastRow);
    void Resize(int newCapacity);
    void Free();
}

public class ComponentColumn<T> : IComponentColumn
{
    public T[] Data;

    public void SetData(int row, object data)
    {
        this.Data[row] = (T) data;
    }

    public void MoveData(int sourceRow, int targetRow, IComponentColumn targetColumn)
    {
        var target = (ComponentColumn<T>) targetColumn;
        target.Data[targetRow] = Data[sourceRow];
    }

    public void Swap(int rowToRemove, int lastRow)
    {
        this.Data[rowToRemove] = Data[lastRow];
        this.Data[lastRow] = default;
    }

    public void Resize(int newCapacity)
    {
        T[] newData = ArrayPool<T>.Shared.Rent(newCapacity);
        if (Data != null)
        {
            Array.Copy(Data, newData, newCapacity);
            ArrayPool<T>.Shared.Return(Data);
        }
        Data = newData;
    }

    public void Free()
    {
        if (Data == null) return;
        ArrayPool<T>.Shared.Return(Data);
        Data = null;
    }
}

public class Archetype
{
    public HashSet<Type> componentTypes { get; }
    private Dictionary<Type, IComponentColumn> columns = new();
    private List<int> entityIds = [];
    
    private int count = 0;
    private int capacity = 1024;

    public Archetype(IEnumerable<Type> types)
    {
        this.componentTypes = new HashSet<Type>(types);
        
        foreach (var type in types)
        {
            var columnType = typeof(ComponentColumn<>).MakeGenericType(type);
            var columnInstance = (IComponentColumn) Activator.CreateInstance(columnType);
            
            columnInstance.Resize(this.capacity);
            this.columns[type] = columnInstance;
        }
    }

    public T[] GetRawArray<T>()
    {
        if (this.columns.TryGetValue(typeof(T), out var column))
        {
            return ((ComponentColumn<T>) column).Data;
        }
        throw new KeyNotFoundException($"Component {typeof(T).Name} not found in this archetype.");
    }

    public int AddEntity(int entityId, object[] componentData)
    {
        if (this.count >= this.capacity)
        {
            this.capacity *= 2;
            foreach (var column in this.columns.Values) { column.Resize(this.capacity); }
        }
        
        this.entityIds.Add(entityId);
        int row = this.count;
        
        foreach (var data in componentData)
        {
            var type = data.GetType();
            if (this.columns.TryGetValue(type, out var column))
            {
                column.SetData(row, data);
            }
            else
            {
                // Error handling: The entity creation requested a component 
                // that this archetype doesn't support.
                throw new ArgumentException($"Archetype does not support component type: {type.Name}");
            }
        }

        this.count += 1;
        return row;
    }

    public int RemoveEntity(int rowToRemove)
    {
        int lastRow = this.count - 1;
        int movedEntityId = -1;

        if (rowToRemove < lastRow)
        {
            foreach (var column in this.columns.Values)
            {
                column.Swap(rowToRemove, lastRow);
            }
            
            movedEntityId = this.entityIds[lastRow];
            this.entityIds[rowToRemove] = movedEntityId;
        }

        this.entityIds.RemoveAt(lastRow);
        this.count -= 1;
        
        return movedEntityId;
    }

    public override string ToString()
    {
        const int maxDisplay = 20;

        var str = this.columns.Keys.Aggregate($"{"Id",-6}", (current, column) =>
        {
            return current + $"{column.Name,-15}";
        }) + "\n";

        for (var i = 0; i < maxDisplay && i < count; i++)
        {
            str += this.columns.Values.Aggregate($"{i,-6}", (current, column) =>
            {
                var dataArray = column.GetType().GetField("Data")?.GetValue(column) as Array;
                return current + $"{dataArray?.GetValue(i),-15}";
            }) + "\n";
        }

        if (count > maxDisplay)
        {
            str += $"...{count - maxDisplay} wows not shown\n";
        }

        return str;
    }
}