using System;
using System.Collections.Generic;

namespace Flat.ECS;

public class QueryDescription
{
    // Store the Types we want to find
    private List<Type> requiredComponents = new List<Type>();

    public QueryDescription WithAll<T1, T2>()
    {
        requiredComponents.Add(typeof(T1));
        requiredComponents.Add(typeof(T2));
        return this; // Return 'this' to allow chaining
    }

    public List<Type> GetRequiredTypes() => requiredComponents;
}