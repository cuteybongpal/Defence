using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Val>
{
    public Dictionary<Key, Val> MakeDict();
}
