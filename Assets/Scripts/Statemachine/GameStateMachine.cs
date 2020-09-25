using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateMachine : MonoBehaviour
{
    private static bool _initialized;
    private static GameStateMachine _instance;
    private StateMachine _StateMachine;

    public Type CurrentStateType=> _StateMachine.CurrenState.GetType();

    public static event Action<IState> OnGameStateChanged;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance= this;
        _initialized= true;
        
        DontDestroyOnLoad(gameObject);

        _StateMachine= new StateMachine();
        _StateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);

        //var menu = new Menu();
        //var loading = new LoadLevel();
        //var play = new Play();
        //var pause = new Pause();

        //_StateMachine.SetState(menu);

        //_StateMachine.AddTransition(menu,loading,() => PlayButton.LevelToLoad!=null);
        //_StateMachine.AddTransition(loading,play,loading.Finished);
        //_StateMachine.AddTransition(play,pause,() => PlayerInput.Instance.PausePressed);
        //_StateMachine.AddTransition(pause,play,() =>PlayerInput.Instance.PausePressed);
        //_StateMachine.AddTransition(pause,loading,() => RestartButton.Pressed);
    }
    private void Update()
    {
        _StateMachine.Tick();
    }
}

