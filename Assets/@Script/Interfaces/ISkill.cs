using System.Collections;
using UnityEngine;

public interface ISkillDeliver
{
    public IEnumerator Move();
}
public interface ISkillHitResolver
{
    public Collider2D ResolveHit(Collider2D collision);
}
public interface ISkillEffect
{
    public void Apply(Collider2D targets);
}
public interface ISkillUse
{
    public SkillData SkillData { get; set; }
    public bool UseSkill(PlayableObject player);
}