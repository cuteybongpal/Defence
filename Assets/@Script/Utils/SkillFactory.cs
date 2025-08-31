using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Define;
using static SkillFactory;
using static UnityEngine.Rendering.GPUSort;

public static class SkillFactory
{
    //�� �޼ҵ�� ��Ƽ�� ��ų��  ������
    public static Skill Generateskill(GenerateSkillArgs arg)
    {
        SkillBuilder skillBuilder = new SkillBuilder();
        Skill skill = skillBuilder.SetMove(arg).SetHitResolver(arg).SetHitEffect(arg).Build();
        return skill;
    }
    public struct GenerateSkillArgs
    {
        public SkillData SkillData;
        public Transform Owner;
        public Transform Skill;
        public Vector3 Dir;
        public Transform Target;
        public float Attack;
        public float Crit;
    }
}

public class SkillBuilder
{
    Skill skill;
    public SkillBuilder SetMove(GenerateSkillArgs arg)
    {
        ISkillDeliver skillDeliver = null;
        switch (arg.SkillData.Delivery.MoveType)
        {
            case Define.MoveType.Following:
                skillDeliver = new TargetingSkill() { Skill = arg.Skill, Target = arg.Target, Speed = arg.SkillData.Delivery.Speed };
                break;
            case Define.MoveType.Straight:
                skillDeliver = new StraightSkill() { Skill = arg.Skill, Dir = arg.Dir, Speed = arg.SkillData.Delivery.Speed, Range = arg.SkillData.Delivery.Range };
                break;
        }
        skill.SkillMove = skillDeliver;
        return this;
    }
    //todo : ���߿� ���ǿ� ���� �Ű����� �־��ְ�, �� �־����.
    public SkillBuilder SetHitResolver(GenerateSkillArgs arg)
    {
        ISkillHitResolver skillHitResolver = null;
        switch (arg.SkillData.Targeting.TargetType)
        {
            case Define.Target.Ally:
                skillHitResolver = new SkillAllyHitResolver();
                break;
            case Define.Target.Enemy:
                skillHitResolver = new SkillEnemyHitResolver();
                break;
            case Define.Target.Target:
                skillHitResolver = new SkillTargetHitResolver() { Target = arg.Target };
                break;
        }
        skill.SkillHitFilter = skillHitResolver;
        return this;
    }
    public SkillBuilder SetHitEffect(GenerateSkillArgs arg)
    {
        ISkillEffect[] effects = new ISkillEffect[arg.SkillData.Effects.Count];
        for (int i = 0; i < effects.Length; i++)
        {
            int freq = arg.SkillData.Effects[i].Frequency;
            if (freq == 0)
                freq = -1;
            ISkillEffect skillEffect = null;
            switch (arg.SkillData.Effects[i].Type)
            {
                case "DamageDown":
                    {
                        DamageDownSkillEffect eff = new DamageDownSkillEffect();
                        eff.Skill = arg.Skill;
                        eff.Damage = arg.SkillData.Effects[i].Operate(arg.Attack);
                        eff.DownMagnitude = arg.SkillData.Effects[i].Value;
                        eff.Owner = arg.Owner;
                        eff.Frenquency = freq;
                        eff.Crit = arg.Crit;
                        eff.Probability = arg.SkillData.Effects[i].Probability;
                        skillEffect = eff;
                    }
                    break;
                case "Destroy":
                    {
                        DestroySkillEffect eff = new DestroySkillEffect();
                        eff.Skill = arg.Skill;
                        eff.Damage = arg.SkillData.Effects[0].Operate(arg.Attack);
                        eff.Owner = arg.Owner;
                        eff.Crit = arg.Crit;
                        skillEffect = eff;
                    }
                    break;
                case "Stun":
                    {
                        StunSkillEffect eff = new StunSkillEffect();
                        eff.Skill = arg.Skill;
                        eff.Damage = arg.SkillData.Effects[i].Operate(arg.Attack);
                        eff.Owner = arg.Owner;
                        eff.Crit = arg.Crit;
                        eff.Frenquency = freq;
                        eff.Probability = arg.SkillData.Effects[i].Probability;
                        eff.StunMagnitude = arg.SkillData.Effects[i].Value;
                        skillEffect = eff;
                    }
                    break;
            }
            effects[i] = skillEffect;
            
        }
        skill.SkillEffect = effects;
        return this;
    }
    public Skill Build()
    {
        return skill;
    }
    public SkillBuilder()
    {
        skill = new Skill();
    }
}
public class TargetingSkill : ISkillDeliver
{
    public Transform Skill;
    public Transform Target;
    public float Speed;

    Rigidbody2D rb;
    public IEnumerator Move()
    {
        if (rb == null)
            rb = Skill.GetComponent<Rigidbody2D>();
        while (Vector3.Distance(Skill.position, Target.position) > 0.1f)
        {
            Vector3 dir = (Target.position - Skill.position).normalized;
            rb.linearVelocity = dir * Speed;
            GoUtils.LookAt(Skill.gameObject, dir, 0);
            yield return null;
        }
    }

}
public class StraightSkill : ISkillDeliver
{
    public Transform Skill;
    public Vector3 Dir;
    public float Speed;
    public float Range;

    Rigidbody2D rb;

    public IEnumerator Move()
    {
        Vector3 originPos = Skill.position;
        if (rb == null)
            rb = Skill.GetComponent<Rigidbody2D>();
        GoUtils.LookAt(Skill.gameObject, Dir, 0);
        while (Vector3.Distance(originPos, Skill.position) <= Range * 1.5)
        {
            rb.linearVelocity = Dir * Speed;
            yield return null;
        }
        GameObject.Destroy(Skill.gameObject);
    }
}
public class SkillEnemyHitResolver : ISkillHitResolver
{
    public Collider2D ResolveHit(Collider2D collision)
    {
        IHit hit = collision.GetComponent<IHit>();
        if (hit == null)
            return null;
        return collision;
    }
}
public class SkillAllyHitResolver : ISkillHitResolver
{
    public Collider2D ResolveHit(Collider2D collision)
    {
        PlayableObject hit = collision.GetComponent<PlayableObject>();
        if (hit == null)
            return null;
        return collision;
    }
}

public class SkillTargetHitResolver : ISkillHitResolver
{
    public Transform Target;
    public Collider2D ResolveHit(Collider2D collision)
    {
        if (collision.transform == Target)
            return collision;
        return null;
    }
}

public class DestroySkillEffect : ISkillEffect
{
    public float Damage;
    public Transform Owner;
    public Transform Skill;
    public float Crit;

    public void Apply(Collider2D targets)
    {
        if (targets == null)
            return;

        IHit hit = targets.GetComponent<IHit>();
        if (hit == null)
            return;
        if (Random.value < Crit)
            hit.Hit(Damage * 2, Owner);
        else
            hit.Hit(Damage, Owner);
        GameObject.Destroy(Skill.gameObject);
    }
}
public class DamageDownSkillEffect : ISkillEffect
{
    public float Damage;
    public Transform Owner;
    public Transform Skill;
    public float DownMagnitude;
    public int Frenquency;
    public float Crit;
    public float Probability;
    public void Apply(Collider2D targets)
    {
        if (targets == null)
            return;

        IHit hit = targets.GetComponent<IHit>();
        if (hit == null)
            return;

        if (Random.value < Crit)
            hit.Hit(Damage * 2, Owner);
        else
            hit.Hit(Damage, Owner);

        if (Frenquency == 0)
            return;

        if (Probability != 0)
            if (Random.value > Probability)
                return;
        
        Damage -= Damage * DownMagnitude;
        Frenquency--;
    }
}
public class StunSkillEffect : ISkillEffect
{
    public float Damage;
    public Transform Owner;
    public Transform Skill;
    public float StunMagnitude;
    public int Frenquency;
    public float Crit;
    public float Probability;
    public void Apply(Collider2D targets)
    {
        if (targets == null)
            return;

        IHit hit = targets.GetComponent<IHit>();
        if (hit == null)
            return;

        if (Random.value < Crit)
            hit.Hit(Damage * 2, Owner);
        else
            hit.Hit(Damage, Owner);

        if (Frenquency == 0)
            return;

        if (Probability != 0)
            if (Random.value > Probability)
            {
                GameObject.Destroy(Skill.gameObject);
                return;
            }
        IStun stun = targets.GetComponent<IStun>();
        if (stun == null)
            return;
        stun.Stun(StunMagnitude);
        Frenquency--;
        GameObject.Destroy(Skill.gameObject);
    }
}