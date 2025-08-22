using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : IService
{
    Dictionary<string, UnityEngine.Object> loadedObjects = new Dictionary<string, UnityEngine.Object>();

    public T Load<T>(string path) where T : UnityEngine.Object
    {
        if (loadedObjects.ContainsKey(path))
            return loadedObjects[path] as T;

        T element = Resources.Load<T>(path);
        loadedObjects.Add(path, element);

        return element as T;
    }
}
