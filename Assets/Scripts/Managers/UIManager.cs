using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Service;

public class UIManager : Singleton<UIManager> //IGameService
{
    private const string UISceneName = "UI";

    public UIObjects UIObjects;
    [HideInInspector]
    public UIPauseMenu PauseMenuReference {get; private set;}
    public UIQuickMenu QuickMenuReference { get; private set; }

    //PlayerInputActions _inputActionToUse;
    public InputSystemUIInputModule CurrentInputSystemUIInputModule {get; private set;}
    public bool UIHasBeenBuilt { get; private set; }

    public bool isInMenu=false;

    //For some reason, it doesn't find UIObject reference when I play in Awake().
    private void Start()
    {
        Setup();
    }
    private void UpdateActionMap()
    {
        if (isInMenu)
        {
            UserInput.Instance.EnableMenuControls();
        }
        else
        {
            
            UserInput.Instance.EnableGameplayControls();
        }
    }

    public void Setup()
    {
        if(!UIHasBeenBuilt)
        {
            SetupInputModule();
            SetupUIScene();
            UIHasBeenBuilt=true;
            UpdateActionMap();
        }
        
    }

    private void SetupUIScene()
    {
        var UIScene=SceneManager.GetSceneByName(UISceneName);
        if(!UIScene.IsValid())
        {
            SceneManager.CreateScene(UISceneName);
        }
        
        if (!GameObject.FindObjectOfType<UIPauseMenu>())
        {            
            Addressables.InstantiateAsync(UIObjects.PauseMenuGameObject).Completed+=PauseMenuLoaded;
        }

        
        if (!GameObject.FindObjectOfType<UIQuickMenu>())
        {            
            Addressables.InstantiateAsync(UIObjects.QuickMenuGameObject).Completed+=QuickMenuLoaded;
        }
    }

    private void QuickMenuLoaded(AsyncOperationHandle<GameObject> obj)
    {
        QuickMenuReference = obj.Result.GetComponent<UIQuickMenu>();
        if (QuickMenuReference == null)
        {
            Debug.LogError("Quick Menu Reference is null.");
        }
        RenameAndMovetoUIScene(QuickMenuReference.gameObject);
        var notify = QuickMenuReference.gameObject.AddComponent<NotifyOnDestroy>();
        notify.Destroyed += Remove;
        notify.AssetReference = UIObjects.PauseMenuGameObject;
    }

    private void PauseMenuLoaded(AsyncOperationHandle<GameObject> obj)
    {
        PauseMenuReference = obj.Result.GetComponent<UIPauseMenu>();
        if (PauseMenuReference == null)
        {
            Debug.LogError("Pause Menu Reference is null.");
        }
        RenameAndMovetoUIScene(PauseMenuReference.gameObject);
        var notify = PauseMenuReference.gameObject.AddComponent<NotifyOnDestroy>();
        notify.Destroyed += Remove;
        notify.AssetReference = UIObjects.PauseMenuGameObject;
    }

    private void RenameAndMovetoUIScene(GameObject obj)
    {
        obj.name = obj.name.Replace("(Clone)", "");
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(UISceneName));
    }

    private void Remove(AssetReference assetReference, NotifyOnDestroy obj)
    {
        Addressables.ReleaseInstance(obj.gameObject);
        Debug.Log("RELEASING");
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
}
