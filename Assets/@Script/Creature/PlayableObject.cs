using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayableObject : MonoBehaviour, IControllable
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Coroutine moveCoroutine;
    public AttackRange attackRange;
    protected Coroutine[] skillCoroutines = new Coroutine[3];
    public List<SkillData> Skills = new List<SkillData>();
    List<ISkillUse> usableSkill = new List<ISkillUse>();
    Dictionary<int, Coroutine> skillCoolDown = new Dictionary<int, Coroutine>();

    public PlayerData PlayerData;
    float currentHp;
    float currentMp;
    public int Lv;
    float currentExp;

    public Action<float> HpChange;
    public Action<float> MpChange;
    public Action<int, float> ExpChange;

    public float CurrentHp
    {
        get { return currentHp; }
        protected set 
        {
            currentHp = value;

            if (currentHp > PlayerData.Hp)
                currentHp = PlayerData.Hp;

            HpChange?.Invoke(currentHp);
        }
    }
    public float CurrentMp
    {
        get { return currentMp; }
        set
        {
            currentMp = value;

            if (currentMp > PlayerData.Mp)
                currentMp = PlayerData.Mp;

            MpChange?.Invoke(currentMp);
        }
    }
    public float CurrentExp { get { return currentExp; } }

    Transform IControllable.Transform { get => transform; }

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
            rb.linearVelocity = dir * PlayerData.Speed;
            yield return null;
        }
        rb.linearVelocity = Vector3.zero;
        anim.Play("Idle");
    }
    protected virtual void BasicAttack()
    {

    }
    protected IEnumerator StartAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/PlayerData.AttackSpeed);
            while (attackRange.GetTarget() == null)
                yield return null;
            BasicAttack();
        }
    }
    protected IEnumerator GainHealing()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            CurrentMp += PlayerData.Mp / 100f;
            CurrentHp += PlayerData.Hp / 100f;
        }
    }
    public void GainExp(float gainExp)
    {
        currentExp += gainExp;

        StartCoroutine(IncreaseLevel());
        ExpChange?.Invoke(Lv, currentExp);
    }
    IEnumerator IncreaseLevel()
    {
        if (Lv > PlayerData.RequiredExp.Count - 2)
            yield break;
        while (currentExp >= PlayerData.RequiredExp[Lv])
        {
            if (Lv > PlayerData.RequiredExp.Count - 2)
                break;
            currentExp -= PlayerData.RequiredExp[Lv];
            Lv++;
            //todo : 레벨업 서비스에 랩업한 거 알리고 스킬 찍게 해야함
            LevelUpService lv = ServiceLocator.Get<LevelUpService>();
            lv.LevelUp(PlayerData.Class, this);
            ExpChange?.Invoke(Lv, currentExp);
            
            yield return new WaitForSeconds(0.1f);
            
        }
    }
     public void KeyAction(KeyCode keyCode, Vector2 mousePos)
    {
        for (int i = 0; i < usableSkill.Count; i++)
        {
            if (usableSkill[i].SkillData.Category == "Passive")
                continue;
            if (Input.GetKeyDown(Define.KeyBinding[i]) && IsAvailiableSkill(i))
            {
                bool isSuccess = usableSkill[i].UseSkill(this);
                
                if (isSuccess)
                    skillCoolDown[i] = StartCoroutine(SkillCoolDown(i));
            }
        }
    }
    public void Add(SkillData skill, ISkillUse skillUse = null)
    {
        bool isDuplicate = false;
        for (int i = 0; i < Skills.Count; i++)
        {
            if (skill.Name == Skills[i].Name)
            {
                Skills[i] = skill;
                isDuplicate = true;
                break;
            }
        }
        if (!isDuplicate)
            Skills.Add(skill);

        if (skillUse == null)
            return;
        isDuplicate = false;
        for (int i = 0; i < usableSkill.Count; i++)
        {
            if (usableSkill[i].SkillData.Name == skill.Name)
            {
                usableSkill[i].SkillData = skill;
                isDuplicate = true;
                break;
            }
        }
        if (!isDuplicate)
            usableSkill.Add(skillUse);
    }
    bool IsAvailiableSkill(int idx)
    {
        if (!skillCoolDown.ContainsKey(idx))
        {
            skillCoolDown[idx] = null;
            return true;
        }
        return skillCoolDown[idx] == null;
    }
    IEnumerator SkillCoolDown(int idx)
    {
        yield return new WaitForSeconds(Skills[idx].Policy.CoolDown);
        skillCoolDown[idx] = null;
    }
}