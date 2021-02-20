using UnityEngine;

public class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Initialize()
    {
        QualitySettings.vSyncCount = 2;
        if(!GameObject.FindObjectOfType<UserInput>())
        {
            var inputGameObject = new GameObject("INPUT SYSTEM");
            inputGameObject.AddComponent<UserInput>();
            GameObject.DontDestroyOnLoad(inputGameObject.gameObject);
        }
        if(!GameObject.FindObjectOfType<GameManager>())
        {
            var gameManagerGameObject = new GameObject("GAME MANAGER");
            gameManagerGameObject.AddComponent<GameManager>();
            GameObject.DontDestroyOnLoad(gameManagerGameObject.gameObject);
        }
        
    }
}
