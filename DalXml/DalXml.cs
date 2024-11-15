﻿using DalApi;
using System.Diagnostics;

namespace Dal;

sealed  internal class DalXml : IDal
{
    /// <summary>
    /// creates a lazy singleton to avoid wasting system resources when the object is not used.
    /// the true parameter causes the initialization to be Thread Safe.
    /// </summary>
    //public static IDal Instance { get; } = new Lazy<DalXml>(true).Value;
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();
}


