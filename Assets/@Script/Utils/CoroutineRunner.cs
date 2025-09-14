using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    static CoroutineRunner instance;
    public static CoroutineRunner Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        Coroutine co = StartCoroutine(coroutine);
        return co;
    }
}
