using System;
using UnityEngine;

public class Dummy : MonoBehaviour, IHit
{
    public int MaxHp;
    public int Exp;
    public float currentHp;
    public Transform Transform { get { return transform; } }

    private void Start()
    {
        currentHp = MaxHp;
    }

    public void Hit(float damage, Transform Owner)
    {
        if (damage > currentHp)
            Owner.GetComponent<PlayableObject>().GainExp((float)Exp * currentHp / (float)MaxHp);
        else
            Owner.GetComponent<PlayableObject>().GainExp((float)Exp * damage / (float)MaxHp);

        currentHp -= damage;
        Debug.Log($"데미지 입음! {damage}");
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
