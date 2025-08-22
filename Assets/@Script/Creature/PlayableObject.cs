using UnityEngine;
using System.Collections;

public class PlayableObject : MonoBehaviour, IControllable
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Coroutine moveCoroutine;
    protected AttackRange attackRange;
    protected Coroutine[] skillCoroutines = new Coroutine[3];

    protected PlayerData playerData;
    protected float currentHp;
    protected float currentMp;

    public void Move(Vector2 mousePos, KeyCode clickType, bool isDown)
    {
        if (clickType != KeyCode.Mouse0)
            return;
        if (moveCoroutine == null)
            moveCoroutine = StartCoroutine(MoveToDestPos(mousePos));
        else
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveToDestPos(mousePos));
        }

    }
    IEnumerator MoveToDestPos(Vector2 pos)
    {
        anim.Play("Walk");
        while (Vector2.Distance(pos, (Vector2)transform.position) >= 0.1f)
        {
            Vector2 dir = pos - (Vector2)transform.position;
            dir.Normalize();
            gameObject.GetComponent<SpriteRenderer>().flipX = dir.x > 0;
            rb.linearVelocity = dir * playerData.Speed;
            yield return null;
        }
        rb.linearVelocity = Vector3.zero;
        anim.Play("Idle");
    }
    public void KeyAction(KeyCode keyCode, Vector2 mousePos)
    {
        if (keyCode == KeyCode.Q && skillCoroutines[0] == null && currentMp >= playerData.Skills[0].MpCost)
        {
            Skill1();
            skillCoroutines[0] = StartCoroutine(WaitSkillCoolDown(0));
        }
        if (keyCode == KeyCode.W && skillCoroutines[1] == null && currentMp >= playerData.Skills[1].MpCost)
        {
            Skill2();
            skillCoroutines[1] = StartCoroutine(WaitSkillCoolDown(1));
        }
        if (keyCode == KeyCode.E && skillCoroutines[2] == null && currentMp >= playerData.Skills[2].MpCost)
        {
            Skill3();
            skillCoroutines[2] = StartCoroutine(WaitSkillCoolDown(2));
        }
    }
    IEnumerator WaitSkillCoolDown(int skillIndex)
    {
        yield return new WaitForSeconds(playerData.Skills[skillIndex].CoolDown);
        skillCoroutines[skillIndex] = null;
    }
    protected virtual void Skill1()
    {

    }
    protected virtual void Skill2()
    {

    }
    protected virtual void Skill3()
    {

    }
}
