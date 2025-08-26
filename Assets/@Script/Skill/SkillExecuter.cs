using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class SkillExecuter : MonoBehaviour
{
    public Skill Skill;

    public void Execute()
    {
        
    }
    IEnumerator StartMove()
    {
        while (true)
        {
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
public class Skill
{
    public Transform Target;
    public ISkillDeliver SkillMove;
    public ISkillEffect SkillEffect;
    public ISkillHitResolver Hit;
}