using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.GPUSort;
using System.Collections;

public class TargetingSkillUse : ISkillUse
{
    public SkillData SkillData { get; set ; }
    public bool UseSkill(PlayableObject player)
    {
        if (player.CurrentMp < SkillData.Policy.MpCost)
            return false;

        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        GameObject go = res.Load<GameObject>(SkillData.Path);
        SkillFactory.GenerateSkillArgs args = new SkillFactory.GenerateSkillArgs();
        
        //카메라로 타겟 찾기
        Vector3 pos = InputUtils.GetMousePos();
        if (Vector2.Distance(pos, player.transform.position) > SkillData.Delivery.Range * 1.5)
            return false;
        Transform collider = InputUtils.GetHoveringObject("Monster");
        if (collider == null)
            return false;

        GameObject s = GameObject.Instantiate(go);
        args.Attack = player.PlayerData.Attack;
        args.SkillData = SkillData;
        args.Owner = player.transform;
        args.Skill = s.transform;
        args.Target = collider;
        SkillExecuter executer = s.GetComponent<SkillExecuter>();
        s.transform.position = player.transform.position;

        Skill skill = SkillFactory.Generateskill(args);
        executer.Skill = skill;
        player.CurrentMp -= SkillData.Policy.MpCost;

        return true;
    }
}

public class StraightSkillUse : ISkillUse
{
    public SkillData SkillData { get; set; }
    public bool UseSkill(PlayableObject player)
    {
        if (player.CurrentMp < SkillData.Policy.MpCost)
            return false;

        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        GameObject go = res.Load<GameObject>(SkillData.Path);
        SkillFactory.GenerateSkillArgs args = new SkillFactory.GenerateSkillArgs();
        
        GameObject s = GameObject.Instantiate(go);
        SkillExecuter executer = s.GetComponent<SkillExecuter>();

        s.transform.position = player.transform.position;
        args.Attack = player.PlayerData.Attack;
        args.SkillData = SkillData;
        args.Owner = player.transform;
        args.Skill = s.transform;
        args.Dir = (InputUtils.GetMousePos() - (Vector2)player.transform.position).normalized;

        Skill skill = SkillFactory.Generateskill(args);
        executer.Skill = skill;
        player.CurrentMp -= SkillData.Policy.MpCost;

        return true;
    }
}
public class MultipleSkillUse : ISkillUse
{
    public SkillData SkillData { get; set; }
    public bool UseSkill(PlayableObject player)
    {
        if (player.CurrentMp < SkillData.Policy.MpCost)
            return false;

        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        GameObject go = res.Load<GameObject>(SkillData.Path);
        SkillFactory.GenerateSkillArgs args = new SkillFactory.GenerateSkillArgs();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, SkillData.Delivery.Range, LayerMask.GetMask("Monster"));
        List<IHit> targets = new List<IHit>();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (i >= SkillData.Targeting.MaxTargetCount)
                break;
            if (colliders[i] == null)
                continue;
            IHit hit = colliders[i].GetComponent<IHit>();
            if (hit == null)
                continue;
            targets.Add(hit);
        }
        if (targets.Count == 0)
            return false;
        int index = 0;
        int targetIndex = 0;
        while (index < SkillData.Delivery.Count)
        {
            if (targets[targetIndex] == null)
                continue;

            GameObject s = GameObject.Instantiate(go);
            SkillExecuter executer = s.GetComponent<SkillExecuter>();

            s.transform.position = player.transform.position;
            args.Attack = player.PlayerData.Attack;
            args.SkillData = SkillData;
            args.Owner = player.transform;
            args.Skill = s.transform;
            args.Target = targets[targetIndex].Transform;
            args.Dir = (InputUtils.GetMousePos() - (Vector2)player.transform.position).normalized;

            Skill skill = SkillFactory.Generateskill(args);
            executer.Skill = skill;
            targetIndex = (targetIndex + 1) % targets.Count;
            index++;
        }

        player.CurrentMp -= SkillData.Policy.MpCost;
        return true;
    }
}
public class RadSkillUse : ISkillUse
{
    public SkillData SkillData { get; set; }

    public bool UseSkill(PlayableObject player)
    {
        if (player.CurrentMp < SkillData.Policy.MpCost)
            return false;

        SkillFactory.GenerateSkillArgs args = new SkillFactory.GenerateSkillArgs();
        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        GameObject go = res.Load<GameObject>(SkillData.Path);
        Vector2 mouseDir = (InputUtils.GetMousePos() - (Vector2)player.transform.position).normalized;
        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        Vector2 startVector = new Vector3(Mathf.Cos((angle - 22.5f)* Mathf.Deg2Rad), Mathf.Sin((angle - 22.5f) * Mathf.Deg2Rad));
        Vector2 endVector = new Vector3(Mathf.Cos((angle + 22.5f) * Mathf.Deg2Rad), Mathf.Sin((angle + 22.5f) * Mathf.Deg2Rad));

        for (int i = 0; i < SkillData.Delivery.Count; i++)
        {

            GameObject s = GameObject.Instantiate(go);
            SkillExecuter executer = s.GetComponent<SkillExecuter>();

            s.transform.position = player.transform.position;
            args.Attack = player.PlayerData.Attack;
            args.SkillData = SkillData;
            args.Owner = player.transform;
            args.Skill = s.transform;
            args.Dir = Vector3.Slerp(startVector, endVector, (float)i / SkillData.Delivery.Count);

            Skill skill = SkillFactory.Generateskill(args);
            executer.Skill = skill;
        }
        return true;
    }
}
public class SeqeunceSkillUse : ISkillUse
{
    public SkillData SkillData { get; set; }

    public bool UseSkill(PlayableObject player)
    {
        if (player.CurrentMp < SkillData.Policy.MpCost)
            return false;

        CoroutineRunner.Instance.RunCoroutine(Fire(player));
        return true;
    }
    IEnumerator Fire(PlayableObject player)
    {
        SkillFactory.GenerateSkillArgs args = new SkillFactory.GenerateSkillArgs();
        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        GameObject go = res.Load<GameObject>(SkillData.Path);
        Vector2 mouseDir = InputUtils.GetMousePos() - (Vector2)player.transform.position;
        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < SkillData.Delivery.Count; i++)
        {
            GameObject s = GameObject.Instantiate(go);
            SkillExecuter executer = s.GetComponent<SkillExecuter>();

            s.transform.position = player.transform.position;
            args.Attack = player.PlayerData.Attack;
            args.SkillData = SkillData;
            args.Owner = player.transform;
            args.Skill = s.transform;
            args.Dir = mouseDir.normalized;

            Skill skill = SkillFactory.Generateskill(args);
            executer.Skill = skill;
            yield return new WaitForSeconds(0.1f);
        }
    }
}