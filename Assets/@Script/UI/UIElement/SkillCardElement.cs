using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillCardElement : BaseUI
{
    enum Texts
    {
        SkillName,
        Lv,
        Desc
    }
    enum Images
    {
        OutLine,
        SkillIcon
    }
    public Action SelectAction;
    PlayableObject player;
    CardLevelData cardLevelData;
    public void Init(PlayableObject player, CardLevelData cardLevelData)
    {
        this.player = player;
        this.cardLevelData = cardLevelData;
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        DataManager dM = ServiceLocator.Get<DataManager>();
        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        SkillData skill = dM.GetSkillData(cardLevelData.SkillId);

        Get<Text>((int)Texts.Lv).text = $"Lv.{skill.Level}";
        Get<Text>((int)Texts.SkillName).text = $"{cardLevelData.Name}";
        Get<Text>((int)Texts.Desc).text = $"{cardLevelData.Description}";

        if (skill.Category == "Active")
            Get<Image>((int)Images.OutLine).color = Color.red;
        else if (skill.Category == "Passive")
            Get<Image>((int)Images.OutLine).color = Color.white;

        Sprite sprite = res.Load<Sprite>(cardLevelData.SpritePath);
        Get<Image>((int)Images.SkillIcon).sprite = sprite;
        GetComponent<Button>().onClick.AddListener(GetOrUpGradeSkill);
    }
    
    void GetOrUpGradeSkill()
    {
        DataManager dM = ServiceLocator.Get<DataManager>();
        ServiceLocator.Get<LevelUpService>().ApplySkill(dM.GetSkillData(cardLevelData.SkillId), player);
        SelectAction?.Invoke();
    }
}