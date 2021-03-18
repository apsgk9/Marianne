using System;
using System.Collections;
using System.Collections.Generic;
using StateMachinePattern;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStateMachine : Singleton<GameStateMachine>
{
    [SerializeField]
    private StateMachine _StateMachine;

    public Type CurrentStateType=> _StateMachine.CurrentState.GetType();

    public static event Action<State> OnGameStateChanged;
    [SerializeField]
    public State _StartState;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        _StateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);
        _StateMachine.SetState(_StartState);
    }
    private void Update()
    {
        _StateMachine.Tick();
    }

    public bool isPaused()
    {
        return _StateMachine.CurrentState is PauseMenuState;
    }
    private void OnApplicationQuit()
    {
    }
}

