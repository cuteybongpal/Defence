using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    Dictionary<(int, Type), Component> uiDict = new Dictionary<(int, Type), Component>();

    protected void Bind<T>(Type type) where T : Component
    {
        string[] names = Enum.GetNames(type);
        Array values = Enum.GetValues(type);
        
        for (int i = 0; i < names.Length; i++)
        {
            T compoenent = GetChildComponent<T>(names[i]);
            uiDict.Add(((int)values.GetValue(i), typeof(T)), compoenent as Component);
        }
    }
    T GetChildComponent<T>(string name) where T : Component
    {
        T[] components = GetComponentsInChildren<T>();
        foreach (T component in components)
        {
            if (component.gameObject.name == name)
                return component;
        }
        return null;
    }
    protected T Get<T>(int idx) where T : Component
    {
        return uiDict[(idx, typeof(T))] as T;
    }
}
