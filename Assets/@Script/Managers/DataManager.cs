using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using JetBrains.Annotations;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class DataManager : IService
{
    public List<int> Ranking = new List<int>();
    Dictionary<string, PlayerData> playerDataDict;
    Dictionary<string, List<CardData>> cardLDataDict;
    Dictionary<int, SkillData> skillDataDict;
    public DataManager()
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<int>));

        if (!Directory.Exists(Application.persistentDataPath + "/ranking.json"))
        {
            using (FileStream stream = new FileStream(Application.persistentDataPath + "/ranking.json", FileMode.Create))
            {
                xmlSerializer.Serialize(stream, Ranking);
            }
        }

        using (FileStream stream = new FileStream(Application.persistentDataPath+"/ranking.json", FileMode.Open))
        {
            Ranking = (List<int>)xmlSerializer.Deserialize(stream);
        }
    }
    public PlayerData GetPlayerData(string name)
    {
        if (playerDataDict == null)
        {
            ResourceManager res = ServiceLocator.Get<ResourceManager>();
            playerDataDict = JsonUtility.FromJson<PlayerDataLoader>(res.Load<TextAsset>("Data/PlayerData").text).MakeDict();
        }
        if (playerDataDict.TryGetValue(name, out PlayerData data))
            return data;
        Debug.Log("플레이어 데이터 없음");
        return new PlayerData();
    }
    public List<CardData> GetCardDatas(string name)
    {
        if (cardLDataDict == null)
        {
            ResourceManager res = ServiceLocator.Get<ResourceManager>();
            cardLDataDict = JsonUtility.FromJson<CardDataLoader>(res.Load<TextAsset>("Data/CardData").text).MakeDict();
        }
        if (cardLDataDict.TryGetValue(name, out List<CardData> data))
            return data;
        Debug.Log("카드 데이터 없음");
        return null;
    }
    public SkillData GetSkillData(int skillId)
    {
        if (skillDataDict == null)
        {
            ResourceManager res = ServiceLocator.Get<ResourceManager>();
            skillDataDict = JsonUtility.FromJson<SkillDataLoader>(res.Load<TextAsset>("Data/SkillData").text).MakeDict();
        }
        if (skillDataDict.TryGetValue(skillId, out SkillData data))
            return data;
        Debug.Log("스킬 데이터 없음");
        return null;
    }
    public T GetScriptableObject<T>(string name) where T : ScriptableObject
    {
        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        T data = res.Load<T>($"Data/{name}");
        if (data == null)
            Debug.Log("해당 데이터가 없음");
        return data;
    }
}
[Serializable]
public class PlayerDataLoader : ILoader<string, PlayerData>
{
    public List<PlayerData> PlayerDatas;

    public Dictionary<string, PlayerData> MakeDict()
    {
        Dictionary<string, PlayerData> dict = new Dictionary<string, PlayerData>();
        foreach(PlayerData playerData in PlayerDatas)
        {
            dict.Add(playerData.Class, playerData);
        }
        return dict;
    }
}
[Serializable]
public struct PlayerData
{
    public string Class;
    public int Hp;
    public int Mp;
    public float Attack;
    public float AttackSpeed;
    public int Range;
    public int InventorySize;
    public float Critical;
    public int Speed;
    public string BasicAttackPath;
    public int GrowthHealth;
    public int GrowthAttack;
    public List<int> RequiredExp;
}
[Serializable]
public class SkillDataLoader : ILoader<int, SkillData>
{
    public List<SkillData> SkillDatas;

    public Dictionary<int, SkillData> MakeDict()
    {
        Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
        foreach (SkillData skillData in SkillDatas)
        {
            dict.Add(skillData.Id, skillData);
        }
        return dict;
    }
}
[Serializable]
public class SkillData
{
    public int Id;
    public string Name;
    public int Level;
    public int MaxLevel;
    public string Category;
    public string Trigger;
    public string Path;
    public SkillTargeting Targeting;
    public SkillDelivery Delivery;
    public List<SkillEffect> Effects;
    public SkillPolicy Policy;
}
[Serializable]
public class SkillTargeting
{
    public string Type;
    public int MaxTargetCount;
    public float Range;

    public Define.Target TargetType { get { return Enum.Parse<Define.Target>(Type); } }
}
[Serializable]
public class SkillDelivery
{
    public string Type;
    public float Speed;
    public int Count;
    public float Range;
    public string ShootStyle;
    public bool FollowingTarget;

    public Define.MoveType MoveType { get { return (Define.MoveType)Enum.Parse<Define.MoveType>(Type); } }
}
[Serializable]
public class SkillEffect
{
    public string Type;
    public string Operator;
    public float Value;
    public float Probability;
    public int Frequency;

    public float Operate(float a)
    {
        switch (Operator)
        {
            case "add":
                return a + Value;
            case "mul":
                return a * Value;
            case "addPercent":
                return a + a * Value;
            default:
                return Value;
        }
    }
}
[Serializable]
public class SkillPolicy
{
    public string OutOfRange;
    public string OnHit;
    public float CoolDown;
    public float MpCost;
}
[Serializable]
public class CardDataLoader : ILoader<string, List<CardData>>
{
    public List<CardData> CardDatas;
    public Dictionary<string, List<CardData>> MakeDict()
    {
        Dictionary<string, List<CardData>> dict = new Dictionary<string, List<CardData>>();
        foreach (CardData cardData in CardDatas)
        {
            foreach(string key in cardData.OwnerClass)
            {
                if (!dict.ContainsKey(key))
                    dict.Add(key, new List<CardData>());
                dict[key].Add(cardData);
            }
        }
        return dict;
    }
}
[Serializable]
public class CardData
{
    public int Id;
    public List<string> OwnerClass;
    public List<CardLevelData> Level;
}
[Serializable]
public class CardLevelData
{
    public string Name;
    public int SkillId;
    public string Description;
    public string SpritePath;
}