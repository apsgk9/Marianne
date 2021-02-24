using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static string NAME = "[PLAYERCAMERA]";
    private void Awake()
    {
    }

    private void OnValidate()
    {
        gameObject.name=NAME;        
    }
}
