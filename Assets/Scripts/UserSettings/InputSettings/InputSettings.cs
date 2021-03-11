using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "ScriptableObjects/Settings/InputSettings", order = 1)]
public class InputSettings : ScriptableObject, Service.IGameService
{
    public float ControllerYAxisSensitivity= 1f;
    public float ControllerXAxisSensitivity= 1f;

}
