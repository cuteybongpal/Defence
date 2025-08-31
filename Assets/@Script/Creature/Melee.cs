using System.Collections;
using UnityEngine;

public class Melee : PlayableObject
{
    GameObject basicAttack;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PlayerData = ServiceLocator.Get<DataManager>().GetPlayerData("Melee");
        CurrentHp = PlayerData.Hp;
        CurrentMp = PlayerData.Mp;

        attackRange = GetComponentInChildren<AttackRange>();
        attackRange.gameObject.transform.localScale = Vector3.one * PlayerData.Range;

        basicAttack = ServiceLocator.Get<ResourceManager>().Load<GameObject>(PlayerData.BasicAttackPath);
        StartCoroutine(GainHealing());
        StartCoroutine(StartAttack());
    }
    protected override void BasicAttack()
    {
        GameObject go = Instantiate(basicAttack);
        Projectile bAttack = go.GetComponent<Projectile>();
        IHit target = attackRange.GetTarget();

        bAttack.Target = target.Transform;
        bAttack.Owner = transform;
        bAttack.Damage = PlayerData.Attack;
        bAttack.HitAction += () => Destroy(bAttack.gameObject);
        bAttack.ExpAction = GainExp;

        Vector2 dir = (target.Transform.position - transform.position).normalized;
        bAttack.Shoot(transform.position, dir, PlayerData.Range * 1.5f, 5);
    }
}
