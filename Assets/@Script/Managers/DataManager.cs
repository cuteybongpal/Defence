using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

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
    public string Category;
    public int MaxLevel;
    public int Level;
    public float Magnitude;
    public Activator Activation;
    public Operator Op;
    
}
[Serializable]
public class Operator
{
    public string operation;
    public float Operate(float a, float b)
    {
        switch (operation)
        {
            case "add":
                return a + b;
            case "mul":
                return a * b;
            case "addPercent":
                return a + a * b;
        }
        Debug.Log(operation + " 해당 operation은 등록되어있지 않습니다.");
        return 0;
    }
}
[Serializable]
public class Activator
{
    public string Trigger;
    public string Path;
    public float Range;
    public ActivateType Type;
    public float CoolDown;
    public float MpCost;
    public float Damage;
    public int TargetCount;
    public Operator Op;
}
[Serializable]
public class ActivateType
{
    public string SkillType;
    public string MoveType;
    public float Speed;

    public Define.MoveType MoveMethod { get { return (Define.MoveType)Enum.Parse(typeof(Define.MoveType), MoveType); } }
    public Define.SkillType SkillT { get { return (Define.SkillType)Enum.Parse(typeof(Define.SkillType), SkillType); } }

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