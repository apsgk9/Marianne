using UnityEngine;
using UnityEngine.AddressableAssets;

public class Bootstrapper
{

    public static AssetReferenceGameObject InitializerPrefab;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Initialize()
    {
        if(!GameObject.FindObjectOfType<GameStateMachine>())
        {
            var gameManagerGameObject = new GameObject("GameStateMachine");
            gameManagerGameObject.AddComponent<GameStateMachine>();
            GameObject.DontDestroyOnLoad(gameManagerGameObject.gameObject);
        }
        
    }
}
