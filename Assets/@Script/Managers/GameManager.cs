using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<IControllable> ControllablesObjects = new List<IControllable>();
    public IControllable ControllingObject;
    InputManager inputManager;
    static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }
    public void GameStart()
    {
        inputManager = ServiceLocator.Get<InputManager>();

        inputManager.KeyBoardAction -= UserKeyBoardInput;
        inputManager.KeyBoardAction += UserKeyBoardInput;
        inputManager.MouseAction -= UserMouseInput;
        inputManager.MouseAction += UserMouseInput;
    }
    private void Update()
    {
        if (inputManager == null)
            return;
        inputManager.Update();
    }
    void UserKeyBoardInput(KeyCode clickedKey)
    {
        ControllingObject.KeyAction(clickedKey, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    void UserMouseInput(KeyCode clickedMouseButton, Vector2 mousePos, bool isDown)
    {
        if (ControllingObject == null)
            return;

        ControllingObject.Move(mousePos, clickedMouseButton, isDown);
    }
}
