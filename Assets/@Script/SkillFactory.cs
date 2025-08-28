using System.Collections;
using UnityEngine;

public static class SkillFactory
{
    //이 메소드로 액티브 스킬만  생성함
    public static Skill Generateskill(GenerateSkillArgs arg)
    {
        Skill skill = new Skill();
        ISkillDeliver skillDeliver = null;
        ISkillHitResolver skillHitResolver = null;
        ISkillEffect skillEffect = null;

        switch (arg.SkillData.Activation.Type.MoveMethod)
        {
            case Define.MoveType.Targeting:
                skillDeliver = new TargetingSkill() { Skill = arg.Skill, Target = arg.Target, Speed = arg.SkillData.Activation.Type.Speed };
                break;
            case Define.MoveType.Straight:
                skillDeliver = new StraightSkill() { Skill = arg.Skill, Dir = arg.Dir, Speed = arg.SkillData.Activation.Type.Speed, Range = arg.SkillData.Activation.Range };
                break;
        }
        //todo : 나중에 데이터에 따라 바꿔야함 그런데 아직은 바꿀 필요가 없어보임
        skillHitResolver = new SkillHitSolver();
        //toodo : 이것도 나중에 hitAction에 따라서 바꿔야함.
        skillEffect = new BasicSkillEffect() { Damage = arg.SkillData.Activation.Op.Operate(arg.Attack, arg.SkillData.Activation.Damage), Owner = arg.Owner, Skill = arg.Skill};
        skill.SkillEffect = skillEffect;
        skill.SkillMove = skillDeliver;
        skill.SkillHitFilter = skillHitResolver;
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

        while (Vector3.Distance(originPos, Skill.position) <= Range * 1.5)
        {
            rb.linearVelocity = Dir * Speed;
            yield return null;
        }
        GameObject.Destroy(Skill.gameObject);
    }
}
public class SkillHitSolver : ISkillHitResolver
{
    public Collider2D ResolveHit(Collider2D collision)
    {
        IHit hit = collision.GetComponent<IHit>();
        if (hit == null)
            return null;
        return collision;
    }
}

public class BasicSkillEffect : ISkillEffect
{
    public float Damage;
    public Transform Owner;
    public Transform Skill;
    public void Apply(Collider2D targets)
    {
        if (targets == null)
            return;

        IHit hit = targets.GetComponent<IHit>();
        if (hit == null)
            return;

        hit.Hit(Damage, Owner);
        GameObject.Destroy(Skill.gameObject);
    }
}