using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public List<IHit> targets = new List<IHit>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHit target = collision.gameObject.GetComponent<IHit>();
        if (target == null)
            return;
        targets.Add(target);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IHit target = collision.gameObject.GetComponent<IHit>();
        if (target == null)
            return;
        targets.Remove(target);
    }
}
