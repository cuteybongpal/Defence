using UnityEngine;
using System.Collections;
public class Ranger : PlayableObject
{
    GameObject basicAttack;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PlayerData = ServiceLocator.Get<DataManager>().GetPlayerData("¿ø°Å¸®");

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
        Projectile skill = go.GetComponent<Projectile>();

        IHit target = attackRange.GetTarget();
        Vector2 dir = (target.Transform.position - transform.position).normalized;

        skill.Owner = transform;
        skill.Target = target.Transform;
        skill.Damage = PlayerData.Attack;
        skill.HitAction += () => Destroy(go);
        skill.ExpAction = GainExp;

        skill.Shoot(transform.position, dir, 1.5f * PlayerData.Range, 10, 180);
    }
}