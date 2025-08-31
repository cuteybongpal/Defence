using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;

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
public class MultipleTargetingSkillUse : ISkillUse
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

            if (colliders[i] == null)
                continue;
            IHit hit = colliders[i].GetComponent<IHit>();
            if (hit == null)
                continue;
            targets.Add(hit);
        }
        if (targets.Count == 0)
            return false;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
                continue;
            if (i >= SkillData.Targeting.MaxTargetCount)
                break;
            GameObject s = GameObject.Instantiate(go);
            SkillExecuter executer = s.GetComponent<SkillExecuter>();

            s.transform.position = player.transform.position;
            args.Attack = player.PlayerData.Attack;
            args.SkillData = SkillData;
            args.Owner = player.transform;
            args.Skill = s.transform;
            args.Target = targets[i].Transform;

            Skill skill = SkillFactory.Generateskill(args);
            executer.Skill = skill;
        }
        player.CurrentMp -= SkillData.Policy.MpCost;
        return true;
    }
}