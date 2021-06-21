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
    private State _StartState;
    [SerializeField]
    private State _NullState;


    public void Activate()
    {
        DontDestroyOnLoad(gameObject);
        _StateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);
        _StateMachine.SetState(_StartState);
    }
    public void LoadStateMachine(StateMachine stateMachine,State startState,State nullState)
    {
        _StateMachine=stateMachine;
        _StartState=startState;
        _NullState=nullState;
        
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
    public bool isGameplay()
    {
        return _StateMachine.CurrentState is GameplayState;
    }
    private void OnApplicationQuit()
    {
        _StateMachine.SetState(_NullState);
    }
}

