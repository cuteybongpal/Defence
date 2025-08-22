using System.Collections.Generic;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

public class ServiceLocator
{
    public static Dictionary<Type, IService> serviceDict = new Dictionary<Type, IService>();

    public static void Set<T>(T service) where T : class, IService
    {
        serviceDict.Add(typeof(T), service);
    }
    public static T Get<T>() where T : class, IService
    {
        return serviceDict[typeof(T)] as T;
    }
}
