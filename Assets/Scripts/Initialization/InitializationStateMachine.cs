using System;
using StateMachinePattern;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
public class InitializationStateMachine
{
    

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeOnLoad()
    {
        if(!GameObject.FindObjectOfType<ConsoleToGUI>())
        {
            var consoleGameObject = new GameObject("ConsoleToGUI");
            var consoleref=consoleGameObject.AddComponent<ConsoleToGUI>();
        }
        if(!GameObject.FindObjectOfType<GameStateMachine>())
        {
            Debug.Log("Initializing");
            
            GameObject gameManagerGameObject = GameObject.Instantiate(Resources.Load("Initialization/GameStateMachine", typeof(GameObject))) as GameObject;
            gameManagerGameObject.name="GameStateMachine";
            var gameSM=gameManagerGameObject.GetComponent<GameStateMachine>();
            

            GameObject.DontDestroyOnLoad(gameManagerGameObject.gameObject);
        }
    }
}