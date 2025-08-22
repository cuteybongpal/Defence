using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class DataManager : IService
{
    public List<int> Ranking = new List<int>();
    Datas PlayerDatas;
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
        if (PlayerDatas == null)
        {
            ResourceManager res = ServiceLocator.Get<ResourceManager>();
            PlayerDatas = JsonUtility.FromJson<Datas>(res.Load<TextAsset>("Data/PlayerData").text);
        }
        foreach (PlayerData playerData in PlayerDatas.PlayerDatas)
        {
            if (playerData.Name == name)
                return playerData;
        }
        Debug.Log("플레이어 데이터 없음");
        return null;
    }
}
[Serializable]
public class Datas
{
    public List<PlayerData> PlayerDatas;
}
[Serializable]
public class PlayerData
{
    public string Name;
    public int Hp;
    public int Mp;
    public int Attack;
    public int Range;
    public int InventorySize;
    public float Ciritical;
    public int Speed;
    public List<Skill> Skills;
}
[Serializable]
public class Skill
{
    public string Path;
    public int Damage;
    public float CoolDown;
    public int MpCost;
}