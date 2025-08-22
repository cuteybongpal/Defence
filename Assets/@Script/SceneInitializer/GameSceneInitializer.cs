using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    void Awake()
    {
        ServiceLocator.Set<DataManager>(new DataManager());
        ServiceLocator.Set<InputManager>(new InputManager());
        ServiceLocator.Set<SoundManager>(new SoundManager());
        ServiceLocator.Set<ResourceManager>(new ResourceManager());
        
        GameManager.Instance.GameStart();

        GameObject go1 = ServiceLocator.Get<ResourceManager>().Load<GameObject>("Prefab/Melee");
        IControllable player1 = Instantiate(go1).GetComponent<Melee>();
    }
}
