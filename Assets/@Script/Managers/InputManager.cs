using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : IService
{
    public event Action<KeyCode> KeyBoardAction;
    public event Action<KeyCode, Vector2, bool> MouseAction;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            MouseAction?.Invoke(KeyCode.Mouse0, Camera.main.ScreenToWorldPoint(Input.mousePosition), false);

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            MouseAction?.Invoke(KeyCode.Mouse0, Camera.main.ScreenToWorldPoint(Input.mousePosition), true);

        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(code))
                KeyBoardAction?.Invoke(code);
        }
    }
}
