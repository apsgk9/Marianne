using UnityEngine;

public class PlayerFreeLookCamera : Singleton<PlayerFreeLookCamera>
{
    public static string NAME = "[PLAYERFREELOOKCAMERA]";
    private void OnValidate()
    {
        gameObject.name=NAME;        
    }
}
