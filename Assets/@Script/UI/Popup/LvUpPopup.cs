using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvUpPopup : PopupUI
{
    enum Images
    {
        HeroIcon,
        WeaponIcon
    }
    enum Texts
    {
        LvText
    }
    enum Transforms
    {
        SkillCardLayout
    }

    public void Init(PlayableObject player, List<CardLevelData> list)
    {
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Transform>(typeof(Transforms));

        ShowPopUp();
        Get<Text>((int)Texts.LvText).text = $"Lv.{player.Lv}";

        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        Sprite classSprite = res.Load<Sprite>($"Sprite/WeaponIcon/{player.PlayerData.Class}");
        Sprite weaponSprite = res.Load<Sprite>($"Sprite/ClassIcon/{player.PlayerData.Class}");

        Get<Image>((int)Images.WeaponIcon).sprite = weaponSprite;
        Get<Image>((int)Images.HeroIcon).sprite = classSprite;

        foreach(CardLevelData cardLevelData in list)
        {

        }
    }

}
