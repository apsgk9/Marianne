using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Service;

public class UIManager : MonoBehaviour, IGameService
{
    private const string UISceneName = "UI";
    public UIPauseMenu PauseMenuPrefab;
    public UIQuickMenu QuickMenuPrefab;

    [HideInInspector]
    public UIPauseMenu PauseMenuReference {get; private set;}
    [HideInInspector]
    public UIQuickMenu QuickMenuReference { get; private set; }
    public InputSystemUIInputModule CurrentInputSystemUIInputModule {get; private set;}
    public bool UIHasBeenBuilt { get; private set; }

    //todo: update this so that its hooked
    public bool isInMenu=false;

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
            var obj=GameObject.Instantiate(PauseMenuPrefab);
            PauseMenuReference=obj;
            RenameAndMovetoUIScene(PauseMenuReference.gameObject);
        }

        
        if (!GameObject.FindObjectOfType<UIQuickMenu>())
        {            
            var obj=GameObject.Instantiate(QuickMenuPrefab);
            QuickMenuReference=obj;
            RenameAndMovetoUIScene(QuickMenuReference.gameObject);
        }
    }

    private void RenameAndMovetoUIScene(GameObject obj)
    {
        obj.name = obj.name.Replace("(Clone)", "");
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(UISceneName));
    }

    private void Remove(AssetReference assetReference, NotifyOnDestroy obj)
    {
        Addressables.ReleaseInstance(obj.gameObject);
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
