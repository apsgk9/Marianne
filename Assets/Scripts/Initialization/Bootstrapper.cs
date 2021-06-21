using UnityEngine;
using UnityEngine.AddressableAssets;


public class Bootstrapper
{
    
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
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

/*
https://uninomicon.com/runtimeinitializeonload

RuntimeInitializeOnLoadMethod
The documentation is not correct about this function running after awake. It does by default, but not if you specify a load type:

The order of callbacks is: 1)

SubsystemRegistration
AfterAssembliesLoad
BeforeSplashScreen2)
BeforeSceneLoad
Unity MonoBehaviour.Awake() runs here
AfterSceneLoad, Default
Providing no load type defaults to RuntimeInitializeLoadType.AfterSceneLoad, running the function after Awake.

Other versions that have been verified to also use this exact order:

2021.1.12f1 (editor only)
Pages that link to RuntimeInitializeOnLoadMethod:The Uninomicon
*/