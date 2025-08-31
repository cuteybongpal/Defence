using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    List<IHit> targets = new List<IHit>();
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
    public IHit GetTarget()
    {
        if (targets.Count == 0)
            return null;
        foreach (IHit hit in targets)
        {
            if (!hit.Transform.gameObject.activeSelf)
                continue;

            return hit;
        }

        return null;
    }
    public IHit[] GetTargets(int cnt)
    {
        IHit[] targets = new IHit[cnt];

        int idx = 0;

        for (int i = 0; i < this.targets.Count;)
        {
            if (this.targets[i] == null)
            {
                this.targets.RemoveAt(i);
                continue;
            }
            i++;
        }

        foreach (IHit hit in this.targets)
        {
            if (!hit.Transform.gameObject.activeSelf)
                continue;
            targets[idx] = hit;
            idx++;
        }
        return targets;
    }
}