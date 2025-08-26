using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Data/Monster")]
public class Monster : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public int MaxHp;
    public int Damage;
    public float Speed;
    public int Exp;
}