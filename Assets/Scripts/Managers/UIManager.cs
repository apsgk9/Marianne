using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
public class UIManager : Singleton<UIManager>
{
    public UIPauseMenu PauseMenu;

    [SerializeField][HideInInspector]
    PlayerInputActions _inputActionToUse;
    public InputSystemUIInputModule CurrentInputSystemUIInputModule;

    private void Awake()
    {
        SetupInputModule();
    }

    private void SetupInputModule()
    {
        var modulesFound = GameObject.FindObjectsOfType<InputSystemUIInputModule>();
        if (modulesFound.Length==0)
        {
            var inputGameObject = new GameObject("EventSystem");
            inputGameObject.AddComponent<InputSystemUIInputModule>();
            GameObject.DontDestroyOnLoad(inputGameObject.gameObject);
        }        
        else if(modulesFound.Length>1)
        {
            for(int i=1;i<modulesFound.Length;i++)
            {
                Destroy(modulesFound[i].gameObject); 
            }
        }
        else
        {
            CurrentInputSystemUIInputModule = modulesFound[0].GetComponent<InputSystemUIInputModule>();
        }
    }

    internal void SetupManager()
    {
        throw new NotImplementedException();
    }

    internal void UpdateUIMenuState(bool isPaused)
    {

    }
}
