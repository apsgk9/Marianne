using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserSettings", menuName = "ScriptableObjects/UserSettings", order = 1)]
public class UserSettings : ScriptableObject
{
    public float ControllerYAxisSensitivity= 1f;
    public float ControllerXAxisSensitivity= 1f;
}
