using UnityEngine;

public static class SkillFactory
{
    //이 메소드로 액티브 스킬만 함 ㅇㅋ?
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
                skillDeliver = new StraightSkill() { Skill = arg.Skill, Dir = (arg.Target.position - arg.Skill.position).normalized, Speed = arg.SkillData.Activation.Type.Speed };
                break;
        }
        //todo : 나중에 데이터에 따라 바꿔야함 그런데 아직은 바꿀 필요가 없어보임
        skillHitResolver = new SkillHitSolver();
        //toodo : 이것도 나중에 hitAction에 따라서 바꿔야함.
        skillEffect = new BasicSkillEffect() { Damage = arg.SkillData.Activation.Op.Operate(arg.Attack, arg.SkillData.Activation.Damage) };
        skill.SkillEffect = skillEffect;
        skill.SkillMove = skillDeliver;
        skill.Hit = skillHitResolver;
        return skill;
    }
    public struct GenerateSkillArgs
    {
        public SkillData SkillData;
        public Transform Owner;
        public Transform Skill;
        public Transform Target;
        public float Attack;
    }
}
public class TargetingSkill : ISkillDeliver
{
    public Transform Skill;
    public Transform Target;
    public float Speed;

    Rigidbody2D rb;
    public void Move()
    {
        if (rb == null)
            rb = Skill.GetComponent<Rigidbody2D>();
        Vector3 dir = (Target.position - Skill.position).normalized;
        rb.linearVelocity = dir * Speed;
    }

}
public class StraightSkill : ISkillDeliver
{
    public Transform Skill;
    public Vector3 Dir;
    public float Speed;

    Rigidbody2D rb;
    public void Move()
    {
        if (rb == null)
            rb = Skill.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Dir * Speed;
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
        IHit hit = targets.GetComponent<IHit>();
        if (hit == null)
            return;

        hit.Hit(Damage, Owner);
        GameObject.Destroy(Skill.gameObject);
    }
}