using System;
using StateMachinePattern;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

[assembly: AlwaysLinkAssembly]

[CreateAssetMenu]
public class InitializationStateMachine : ScriptableObject
{
    
    [SerializeField]
    private StateMachine _StateMachine;
    [SerializeField]
    private State _StartState;
    [SerializeField]
    private State _NullState;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void InitializeOnLoad()
    {
        if(!GameObject.FindObjectOfType<GameStateMachine>())
        {
            Debug.Log("Initializing");
            var gameManagerGameObject = new GameObject("GameStateMachine");
            var gstate=gameManagerGameObject.AddComponent<GameStateMachine>();
            gstate.Activate();
            //gstate.LoadStateMachine(_StateMachine,_StartState,_NullState);

            GameObject.DontDestroyOnLoad(gameManagerGameObject.gameObject);
        }
        //Debug.Assert(instance != null, "Failed to load MyCoolManager");
        // Do one time initialization here, like creating game objects or loading other resources
    }
}