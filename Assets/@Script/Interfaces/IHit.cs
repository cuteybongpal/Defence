using System;
using UnityEngine;

public interface IHit
{
    Transform Transform { get; }
    public void Hit(float damage, Transform Owner);
}
