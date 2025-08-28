using UnityEngine;

public class TargetingSkillUse : ISkillUse
{
    public SkillData SkillData { get; set ; }
    public bool UseSkill(PlayableObject player)
    {
        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        GameObject go = res.Load<GameObject>(SkillData.Activation.Path);
        SkillFactory.GenerateSkillArgs args = new SkillFactory.GenerateSkillArgs();
        

        //카메라로 타겟 찾기
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(pos, player.transform.position) > SkillData.Activation.Range * 1.5)
            return false;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0, LayerMask.GetMask("Enemy"));
        if (hit.collider == null)
            return false;

        GameObject s = GameObject.Instantiate(go);
        args.Attack = player.PlayerData.Attack;
        args.SkillData = SkillData;
        args.Owner = player.transform;
        args.Skill = s.transform;
        args.Target = hit.collider.transform;
        SkillExecuter executer = s.GetComponent<SkillExecuter>();
        s.transform.position = player.transform.position;

        Skill skill = SkillFactory.Generateskill(args);
        executer.Skill = skill;
        return true;
    }
}

public class StraightSkillUse : ISkillUse
{
    public SkillData SkillData { get; set; }
    public bool UseSkill(PlayableObject player)
    {
        if (player.CurrentMp < SkillData.Activation.MpCost)
            return false;

        ResourceManager res = ServiceLocator.Get<ResourceManager>();
        GameObject go = res.Load<GameObject>(SkillData.Activation.Path);
        SkillFactory.GenerateSkillArgs args = new SkillFactory.GenerateSkillArgs();
        
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        dir.Normalize();
        args.Dir = dir;
        GameObject s = GameObject.Instantiate(go);
        SkillExecuter executer = s.GetComponent<SkillExecuter>();

        s.transform.position = player.transform.position;
        args.Attack = player.PlayerData.Attack;
        args.SkillData = SkillData;
        args.Owner = player.transform;
        args.Skill = s.transform;
        
        Skill skill = SkillFactory.Generateskill(args);
        executer.Skill = skill;
        player.CurrentMp -= SkillData.Activation.MpCost;
        return true;
    }
}
