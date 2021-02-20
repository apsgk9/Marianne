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
    public GameObject inScenePlayer;
    public Transform spawnRingCenter;
    public bool isPaused;
    private void Awake()
    {
        CreateUIMANAGER();
        CreateInputSystem();
    }

    private static void CreateInputSystem()
    {
        if (!GameObject.FindObjectOfType<UserInput>())
        {
            var inputGameObject = new GameObject("INPUTSYSTEM");
            inputGameObject.AddComponent<UserInput>();
            GameObject.DontDestroyOnLoad(inputGameObject.gameObject);
        }
    }

    private static void CreateUIMANAGER()
    {
        if (!GameObject.FindObjectOfType<UIManager>())
        {
            var inputGameObject = new GameObject("UIMANAGER");
            inputGameObject.AddComponent<UIManager>();
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

        UpdateUIMenu();

    }

    //void UpdateActivePlayerInputs()
    //{
    //    for(int i = 0; i < activePlayerControllers.Count; i++)
    //    {
    //        if(activePlayerControllers[i] != focusedPlayerController)
    //        {
    //             activePlayerControllers[i].SetInputActiveState(isPaused);
    //        }
//
    //    }
    //}

    void SwitchFocusedPlayerControlScheme()
    {
        switch(isPaused)
        {
            case true:
                UserInput.Instance.EnablePauseMenuControls();
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
