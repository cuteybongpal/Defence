using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class SkillExecuter : MonoBehaviour
{
    public Skill Skill;

    private void Start()
    {
        StartCoroutine(Skill.SkillMove.Move());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Skill.SkillHitFilter == null)
            Debug.Log("null¿”");
        Collider2D collider = Skill.SkillHitFilter.ResolveHit(other);
        foreach (ISkillEffect effect in Skill.SkillEffect)
        {
            effect.Apply(collider, Skill);
        }
    }
}
public class Skill
{
    public Transform Target;
    public float Damage;
    
    public ISkillDeliver SkillMove;
    public ISkillHitResolver SkillHitFilter;
    public ISkillEffect[] SkillEffect;
}