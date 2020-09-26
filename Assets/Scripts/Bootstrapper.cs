using UnityEngine;

public class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Initialize()
    {
        var inputGameObject = new GameObject("INPUT SYSTEM");
        inputGameObject.AddComponent<PlayerCharacterInput>();
        GameObject.DontDestroyOnLoad(inputGameObject.gameObject);
        
    }
}
