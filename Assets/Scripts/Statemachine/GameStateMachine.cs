using System;
using System.Collections;
using System.Collections.Generic;
using StateMachinePattern;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStateMachine : Singleton<GameStateMachine>
{
    private static bool _initialized;
    private StateMachine _StateMachine;

    public Type CurrentStateType=> _StateMachine.CurrenState.GetType();

    public static event Action<State> OnGameStateChanged;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _StateMachine= new StateMachine();
        _StateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);
    }
    private void Update()
    {
        _StateMachine.Tick();
    }
}

