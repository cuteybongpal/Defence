using UnityEngine;

public interface ISkillDeliver
{
    public void Move();
}
public interface ISkillHitResolver
{
    public Collider2D ResolveHit(Collider2D collision);
}
public interface ISkillEffect
{
    public void Apply(Collider2D targets);
}
