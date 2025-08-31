using System;
using UnityEngine;

public class Dummy : MonoBehaviour, IHit, IStun
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
            Owner.GetComponent<PlayableObject>().GainExp(Exp * currentHp / MaxHp);
        else
            Owner.GetComponent<PlayableObject>().GainExp(Exp * damage / MaxHp);

        currentHp -= damage;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Stun(float duration)
    {
        Debug.Log($"기절함 {duration}초");
    }
}
