using System.Collections.Generic;
using UnityEngine;

public class UIManager : IService
{
    private BaseUI mainUI;
    private List<PopupUI> popupUIs;
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
    public T ShowUI<T>(string path) where T : BaseUI
    {
        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        GameObject go = GameObject.Instantiate(res.Load<GameObject>(path));
        T ui = go.GetComponent<T>();
        return ui;

    }
}
