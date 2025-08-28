using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class LevelUpService : IService
{
    public void LevelUp(string className, PlayableObject player)
    {
        Time.timeScale = 0;
        List<CardData> cData = ServiceLocator.Get<DataManager>().GetCardDatas(className);
        List<CardData> availiableCard = new List<CardData>();
        player.PlayerData.Hp += player.PlayerData.GrowthHealth;
        player.PlayerData.Attack += player.PlayerData.GrowthAttack;
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
            List<int> indexes = new List<int>();
            for (int i = 0; i < availiableCard.Count; i++)
                indexes.Add(i);

            int[] randomIndexes = new int[3];
            for (int i = 0; i < 3; i++)
            {
                int rnd = Random.Range(0, indexes.Count);
                randomIndexes[i] = indexes[rnd];
                Debug.Log(randomIndexes[i]);
                indexes.RemoveAt(rnd);
            }
            for (int i = 0; i < randomIndexes.Length; i++)
            {
                Debug.Log(availiableCard[randomIndexes[i]].Id);
                finalSelectedCard.Add(availiableCard[randomIndexes[i]]);
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
                    {
                        
                        level = skillData.Level;
                        
                        break;
                    }
                }
            }
            Debug.Log($"{finalSelectedCard[i].Id} : {level}");
            cardLevelDatas.Add(finalSelectedCard[i].Level[level]);
        }
        LvUpPopup lvUp = ServiceLocator.Get<UIManager>().ShowPopupUI<LvUpPopup>();
        lvUp.Init(player, cardLevelDatas);
    }
    public void ApplySkill(SkillData skillData, PlayableObject player)
    {
        DataManager dM = ServiceLocator.Get<DataManager>();

        switch (skillData.Category)
        {
            case "Passive":
                SpecUp(skillData.Name, skillData, player);
                break;
            case "Active":

                break;
        }
        Time.timeScale = 1;
        
        player.Add(skillData, new StraightSkillUse() { SkillData = skillData });
    }
    void SpecUp(string statName, SkillData skillData, PlayableObject player)
    {
        DataManager dM = ServiceLocator.Get<DataManager>();

        switch (statName)
        {
            case "Crit":
                player.PlayerData.Critical = skillData.Op.Operate(dM.GetPlayerData(player.PlayerData.Class).Critical, skillData.Magnitude);
                Debug.Log($"크리티컬 확률 : {player.PlayerData.Critical}");
                break;
            case "AttackSpeed":
                player.PlayerData.AttackSpeed = skillData.Op.Operate(dM.GetPlayerData(player.PlayerData.Class).AttackSpeed, skillData.Magnitude);
                Debug.Log($"공격 속도 : {player.PlayerData.AttackSpeed}");
                break;
            case "Attack":
                player.PlayerData.Attack = skillData.Op.Operate(dM.GetPlayerData(player.PlayerData.Class).Attack + player.PlayerData.GrowthAttack * player.Lv, skillData.Magnitude);
                Debug.Log($"공격 속도 : {player.PlayerData.Attack}");
                break;
            case "Health":
                player.PlayerData.Hp = (int)skillData.Op.Operate(dM.GetPlayerData(player.PlayerData.Class).Hp + player.PlayerData.GrowthHealth * player.Lv, skillData.Magnitude);
                Debug.Log($"공격 속도 : {player.PlayerData.Hp}");
                break;
            case "Speed":
                player.PlayerData.Speed = (int)skillData.Op.Operate(dM.GetPlayerData(player.PlayerData.Class).Speed, skillData.Magnitude);
                Debug.Log($"공격 속도 : {player.PlayerData.Speed}");
                break;
        }
    }
}
