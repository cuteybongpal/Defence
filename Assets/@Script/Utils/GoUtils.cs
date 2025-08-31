using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class GoUtils
{
    public static void LookAt(GameObject go, Vector2 dir, float angleOffset)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        go.transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
    }
    public static void LookAt(GameObject go, GameObject target, float angleOffset)
    {
        Vector2 dir = (target.transform.position - go.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        go.transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
    }
}
