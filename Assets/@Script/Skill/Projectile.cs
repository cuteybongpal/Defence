using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    public Action HitAction;
    public Transform Owner;
    public Transform Target;
    public float Damage;
    public Action<float> ExpAction;

    Rigidbody2D rb;
    public void Shoot(Vector3 origin, Vector3 dir, float maxDistance, float speed, float angleOffset)
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = origin;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
        rb.linearVelocity = dir * speed;
        StartCoroutine(CheckOutOfRange(origin, maxDistance));
    }
    IEnumerator CheckOutOfRange(Vector3 originPos, float distance)
    {
        while (Vector3.Distance(transform.position, originPos) <= distance)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == Owner)
            return;
        if (Target == null)
        {
            IHit coll = collision.GetComponent<IHit>();
            if (coll == null)
                return;

            coll.Hit(Damage, Owner);
            HitAction?.Invoke();
            return;
        }
        if (collision.transform != Target)
            return;

        collision.GetComponent<IHit>().Hit(Damage, Owner);
        HitAction?.Invoke();
    }
}