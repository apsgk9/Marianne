using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public static string NAME = "[PLAYERCAMERA]";

    private void OnValidate()
    {
        gameObject.name=NAME;        
    }
}
