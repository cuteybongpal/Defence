using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class LevelUpService : IService
{
    public void LevelUp(string className, PlayableObject player)
    {
        List<CardData> cData = ServiceLocator.Get<DataManager>().GetCardDatas(className);
        List<CardData> availiableCard = new List<CardData>();
        for (int i = 0; i < cData.Count; i++)
        {
            CardData card = cData[i];
            SkillData maxLevelSkill = ServiceLocator.Get<DataManager>().GetSkillData(card.Level[^1].SkillId);

            bool isMaxLevel = false;
            foreach (SkillData skillData in player.Skills)
            {
                if (skillData == maxLevelSkill)
                {
                    isMaxLevel = true;
                    break;
                }
            }
            if (!isMaxLevel)
                availiableCard.Add(card);
        }

        List<CardData> finalSelectedCard = new List<CardData>();
        if (availiableCard.Count <= 3)
            finalSelectedCard = availiableCard;
        else
        {
            HashSet<int> indexes = new HashSet<int>();
            for (int i = 0; i < availiableCard.Count; i++)
                indexes.Add(i);
            int idx = 0;
            foreach(int index in indexes)
            {
                if (idx >= 3)
                    break;
                finalSelectedCard.Add(availiableCard[idx]);
                idx++;
            }
        }
        List<CardLevelData> cardLevelDatas = new List<CardLevelData>();
        for (int i = 0; i < finalSelectedCard.Count; i++)
        {
            int level = 0;
            for (int j = 0; j < finalSelectedCard[i].Level.Count; j++)
            {
                foreach(SkillData skillData in player.Skills)
                {
                    if (skillData.Id == finalSelectedCard[i].Level[j].SkillId)
                        level = skillData.Level;
                        break;
                }
            }
            cardLevelDatas.Add(finalSelectedCard[i].Level[level]);
        }
        LvUpPopup lvUp = ServiceLocator.Get<UIManager>().ShowUI<LvUpPopup>("Prefab/UI/LvUpPopup");
        lvUp.Init(player, cardLevelDatas);
    }
}
