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
        Targeting,
        Straight
    }
    public static KeyCode[] KeyBinding = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y };
}
