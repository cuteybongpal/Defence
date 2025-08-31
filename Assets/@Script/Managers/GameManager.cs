using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

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
        Vector3 camPos = ControllingObject.Transform.position;
        camPos.z = -10;
        Camera.main.transform.position = camPos;
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
    public void ChangeControllingObject(int idx)
    {
        
        PlayableObject player = ControllingObject as PlayableObject;
        //hp UI에 보여주는 이벤트 없애기 
        if (player != null)
        {
            player.HpChange -= BindingPlayerHpAction;
            player.MpChange -= BindingPlayerMpAction;
            player.ExpChange -= BindingPlayerExpAction;
        }
        
        ControllingObject = ControllablesObjects[idx];
        player = ControllingObject as PlayableObject;

        player.HpChange += BindingPlayerHpAction;
        player.MpChange += BindingPlayerMpAction;
        player.ExpChange += BindingPlayerExpAction;

        player.MpChange?.Invoke(player.CurrentMp);
        player.HpChange?.Invoke(player.CurrentHp);
        player.ExpChange?.Invoke(player.Lv, player.CurrentExp);
    }
    void BindingPlayerMpAction(float value)
    {
        PlayableObject player = ControllingObject as PlayableObject;
        GameSceneUI ui = ServiceLocator.Get<UIManager>().GetMainUI<GameSceneUI>();
        if (ui == null)
            return;
        ui.ChangeSliderValue(GameSceneUI.Sliders.MpSlider, value / player.PlayerData.Mp);
        ui.ChangeTextValue(GameSceneUI.Texts.MpText, $"{value:F1} / {player.PlayerData.Mp:F1}");
    }
    void BindingPlayerHpAction(float value)
    {
        PlayableObject player = ControllingObject as PlayableObject;
        GameSceneUI ui = ServiceLocator.Get<UIManager>().GetMainUI<GameSceneUI>();
        if (ui == null)
            return;
        ui.ChangeSliderValue(GameSceneUI.Sliders.HpSlider, value / player.PlayerData.Hp);
        ui.ChangeTextValue(GameSceneUI.Texts.HpText, $"{value:F1} / {player.PlayerData.Hp:F1}");
    }
    void BindingPlayerExpAction(int lv, float value)
    {
        PlayableObject player = ControllingObject as PlayableObject;
        GameSceneUI ui = ServiceLocator.Get<UIManager>().GetMainUI<GameSceneUI>();
        if (ui == null)
            return;
        ui.ChangeSliderValue(GameSceneUI.Sliders.ExpSlider, value / player.PlayerData.RequiredExp[lv]);
        float l = 100 * value / player.PlayerData.RequiredExp[lv];
        if (l > 100)
            l = 100;
        ui.ChangeTextValue(GameSceneUI.Texts.ExpText, $"{l:F1} %");
        ui.ChangeTextValue(GameSceneUI.Texts.LvText, $"Lv. {lv}");
    }
}