using System.Collections.Generic;
using UnityEngine;

public class UIManager : IService
{
    private BaseUI mainUI;
    private List<PopupUI> popupUIs = new List<PopupUI>();
    //properties

    public BaseUI MainUI 
    {
        set { mainUI = value; }
    }
    public T GetMainUI<T>() where T : BaseUI
    {
        return mainUI as T;
    }
    public void Add(PopupUI popup)
    {
        popupUIs.Add(popup);
    }
    public void Remove(PopupUI popup)
    {
        popupUIs.Remove(popup);
    }
    public T ShowUI<T>() where T : BaseUI
    {
        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        GameObject go = GameObject.Instantiate(res.Load<GameObject>($"Prefab/UI/{typeof(T).Name}"));
        T ui = go.GetComponent<T>();
        return ui;
    }
    public T ShowPopupUI<T>() where T : PopupUI
    {
        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        GameObject go = GameObject.Instantiate(res.Load<GameObject>($"Prefab/UI/Popup/{typeof(T).Name}"));
        T ui = go.GetComponent<T>();
        return ui;
    }
}
