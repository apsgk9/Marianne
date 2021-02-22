using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameMode
{
    Battle,
    FreeRoam
}
public class GameManager : Singleton<GameManager>
{   
    public GameMode currentGameMode=GameMode.FreeRoam;
    public bool isPaused;
    private UIManager _UIManager;
    [HideInInspector]
    public UserSettings UserSettings;

    private void Awake()
    {
        _SpawnIfNull=true;
        CreateInputSystem();
        CreateUIMANAGER();
    }

    private void CreateInputSystem()
    {
        if (!GameObject.FindObjectOfType<UserInput>())
        {
            var inputGameObject = new GameObject("INPUTSYSTEM");
            inputGameObject.AddComponent<UserInput>();
            GameObject.DontDestroyOnLoad(inputGameObject.gameObject);
        }
    }

    private void CreateUIMANAGER()
    {
        if (!GameObject.FindObjectOfType<UIManager>())
        {
            var inputGameObject = new GameObject("UIMANAGER");
            _UIManager =inputGameObject.AddComponent<UIManager>();
            _UIManager.Setup();
            GameObject.DontDestroyOnLoad(inputGameObject.gameObject);

        }
    }

    void Start()
    {
        isPaused = false;
    }

    public void TogglePauseState()
    {
        isPaused = !isPaused;

        ToggleTimeScale();

        //UpdateActivePlayerInputs();

        SwitchFocusedPlayerControlScheme();
        
        //UpdateUIMenu();

    }

    void SwitchFocusedPlayerControlScheme()
    {
        switch(isPaused)
        {
            case true:
                UserInput.Instance.EnableMenuControls();
                break;

            case false:
                UserInput.Instance.EnableGameplayControls();
                break;
        }
    }

    void UpdateUIMenu()
    {
        UIManager.Instance.UpdateUIMenuState(isPaused);
    }

    public int NumberOfConnectedDevices()
    {
        return InputSystem.devices.Count;
    }
    
    //Pause Utilities ----

    void ToggleTimeScale()
    {
        float newTimeScale = 0f;

        switch(isPaused)
        {
            case true:
                newTimeScale = 0f;
                break;

            case false:
                newTimeScale = 1f;
                break;
        }

        Time.timeScale = newTimeScale;
    }
}
