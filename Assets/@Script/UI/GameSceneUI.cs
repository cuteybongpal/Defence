using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : BaseUI
{
    public enum Buttons
    {
        ChangeHero_Melee,
        ChangeHero_Ranger,
        ChangeHero_Mage
    }
    public enum Sliders
    {
        HpSlider,
        MpSlider,
        TaskSlider,
        ExpSlider
    }
    public enum Texts
    {
        HpText,
        MpText,
        TaskText,
        ExpText,
        LvText
    }
    void Start()
    {
        ServiceLocator.Get<UIManager>().MainUI = this;
        Bind<Button>(typeof(Buttons));
        Bind<Slider>(typeof(Sliders));
        Bind<Text>(typeof(Texts));

        Get<Button>((int)Buttons.ChangeHero_Melee).onClick.AddListener(() => GameManager.Instance.ChangeControllingObject(0));
        Get<Button>((int)Buttons.ChangeHero_Ranger).onClick.AddListener(() => GameManager.Instance.ChangeControllingObject(1));
        Get<Button>((int)Buttons.ChangeHero_Mage).onClick.AddListener(() => GameManager.Instance.ChangeControllingObject(2));

    }
    public void ChangeSliderValue(Sliders slider, float value)
    {
        int idx = (int)slider;
        Get<Slider>(idx).value = value;
    }
    public void ChangeTextValue(Texts text, string s)
    {
        int idx = (int)text;
        Get<Text>(idx).text = s;
    }
}
