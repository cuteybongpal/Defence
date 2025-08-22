using UnityEngine;
using System.Collections;

public class Mage : PlayableObject
{
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerData = ServiceLocator.Get<DataManager>().GetPlayerData("����");
        currentHp = playerData.Hp;

        GameManager.Instance.ControllablesObjects.Add(this);

        attackRange = GetComponentInChildren<AttackRange>();
        attackRange.gameObject.transform.localScale = Vector3.one * playerData.Range;
        StartCoroutine(GainHealing());
    }
    protected override void Skill1()
    {
        Debug.Log("��ų 1");
    }
    protected override void Skill2()
    {
        Debug.Log("��ų 2");
    }
    protected override void Skill3()
    {
        Debug.Log("��ų 3");
    }
    IEnumerator GainHealing()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            currentMp += playerData.Mp / 100f;
            currentHp += playerData.Hp / 100f;
        }
    }
}
