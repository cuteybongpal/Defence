using UnityEngine;

public class Define
{
    public enum SkillCategory
    {
        None,
        Passive,
        Active,
    }
    public enum DeBuff
    {
        Slow,
    }
    public enum SkillType
    {
        Projectile,
    }
    public enum MoveType
    {
        Following,
        Straight
    }
    public enum Target
    {
        Enemy,
        Ally,
        Target
    }
    public enum HitAction
    {
        Destroy,
        DamageDown,
        Stun
    }
    public enum Frenquency
    {
        Once,
        Each
    }
    public static KeyCode[] KeyBinding = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y };
}
