using System;
using System.Collections;
using System.Collections.Generic;
using Service;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    private void Awake()
    {        
        ServiceLocator.Initialize();
        CreateUserSettings();
        CreateInputSystem();
        CreateUIMANAGER();
        QualitySettings.vSyncCount = 1;
    }

    private void CreateUserSettings()
    {

        if (!GameObject.FindObjectOfType<SettingsManager>())
        {
            var obj = new GameObject("SETTINGSMANAGER");
            var sManager=obj.AddComponent<SettingsManager>();
            GameObject.DontDestroyOnLoad(obj.gameObject);
            ServiceLocator.Current.Register<SettingsManager>(sManager);
        }
        else
        {
            var user =GameObject.FindObjectOfType<SettingsManager>();
            GameObject.DontDestroyOnLoad(user.gameObject);
            ServiceLocator.Current.Register<SettingsManager>(user);
        }

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
        //UIManager.Instance.UpdateUIMenuState(isPaused);
        //UIManager.Instance.UpdateUIMenuState(isPaused);
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
