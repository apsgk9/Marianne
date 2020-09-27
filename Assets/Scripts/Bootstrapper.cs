using UnityEngine;

public class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Initialize()
    {
        if(!GameObject.FindObjectOfType<PlayerCharacterInput>())
        {
            var inputGameObject = new GameObject("INPUT SYSTEM");
            inputGameObject.AddComponent<PlayerCharacterInput>();
            GameObject.DontDestroyOnLoad(inputGameObject.gameObject);
        }
        
    }
}
