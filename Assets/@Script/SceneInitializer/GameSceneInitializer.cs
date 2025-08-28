using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    void Awake()
    {
        ServiceLocator.Set(new DataManager());
        ServiceLocator.Set(new InputManager());
        ServiceLocator.Set(new SoundManager());
        ServiceLocator.Set(new ResourceManager());
        ServiceLocator.Set(new LevelUpService());
        ServiceLocator.Set(new UIManager());
        
        GameObject go1 = ServiceLocator.Get<ResourceManager>().Load<GameObject>("Prefab/Melee");
        GameObject go2 = ServiceLocator.Get<ResourceManager>().Load<GameObject>("Prefab/Ranger");
        GameObject go3 = ServiceLocator.Get<ResourceManager>().Load<GameObject>("Prefab/Mage");

        IControllable player1 = Instantiate(go1).GetComponent<PlayableObject>();
        IControllable player2 = Instantiate(go2).GetComponent<PlayableObject>();
        IControllable player3 = Instantiate(go3).GetComponent<PlayableObject>();

        GameManager.Instance.ControllablesObjects.Add(player1);
        GameManager.Instance.ControllablesObjects.Add(player2);
        GameManager.Instance.ControllablesObjects.Add(player3);

        GameManager.Instance.ChangeControllingObject(0);
        GameManager.Instance.GameStart();
    }
}
