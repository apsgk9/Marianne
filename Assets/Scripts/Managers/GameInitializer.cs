using System;
using System.Collections;
using Service;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameInit
{

    [CreateAssetMenu(fileName = "GameInitializer", menuName = "GameInitializer", order = 0)]
    public class GameInitializer : GameInitializerSO
    {
        [SerializeField]
        private AssetReferenceGameObject UIManagerReference;
        [SerializeField]
        private AssetReferenceGameObject UserInputSystemReference;
        [SerializeField]
        private AssetReferenceGameObject SettingsManagerReference;
        private GameObject _managerParent;

        //initialize checks
        public override bool HasInitialized { get { return _hasInitialized; } }
        

        private bool _hasInitialized=false;
        private bool _UIManagerInitialized;
        private bool _inputSystemInitialized;
        private bool _userSettingsInitialized;
        public override void Initialize()
        {
            ResetInitializationToFalse();

            if (ServiceLocator.Current == null)
                ServiceLocator.Initialize();
            CreateManagerParent();

            Create<SettingsManager>();
            Create<UserInput>();
            Create<UIManager>();
        }

        private void CreateManagerParent()
        {
            if (!GameObject.Find("--MANAGERS--"))
            {
                _managerParent = new GameObject("--MANAGERS--");
                GameObject.DontDestroyOnLoad(_managerParent);
            }
        }

        private void ResetInitializationToFalse()
        {
            _hasInitialized = false;
            _UIManagerInitialized = false;
            _inputSystemInitialized = false;
            _userSettingsInitialized = false;
        }
        private void Create<T>() where T: MonoBehaviour
        {
            var gameObject = GameObject.FindObjectOfType<T>();
            if (gameObject==null)
            {
                Addressables.InstantiateAsync(GetAssetReference<T>()).Completed += AssetReferenceLoaded<T>;
            }
            else
            {
                WrapUpObject(gameObject.GetComponent<T>());

                //Debug.LogWarning($"{typeof(T)} already exists.");
            }
        }

        

        private void AssetReferenceLoaded<T>(AsyncOperationHandle<GameObject> obj) where T : MonoBehaviour
        {
            var ComponentReference = obj.Result.GetComponent<T>();
            Type type = typeof(T);
            if (ComponentReference == null)
            {
                Debug.LogError($"{typeof(T).Name} component not found.");
                return;
            }

            AddNotifyOnDestroy(ComponentReference);

            GameObject.DontDestroyOnLoad(ComponentReference.gameObject);

            WrapUpObject(ComponentReference);

            //Debug.Log($"{typeof(T).Name} has initialized.");
        }

        private void AddNotifyOnDestroy<T>(T ComponentReference) where T : MonoBehaviour
        {
            var notify = ComponentReference.gameObject.AddComponent<NotifyOnDestroy>();
            notify.Destroyed += Remove;
            notify.AssetReference = GetAssetReference<T>();

            if (notify.AssetReference == null)
            {
                Debug.LogWarning("AssetReference is null on notify");
            }
        }

        public void WrapUpObject<T>(T Component) where T : MonoBehaviour
        {
            UpdateInitializationCheck(Component);
            ParentToManagers(Component.gameObject);
            UpdateGameService(Component);
            CheckifHasBeenInitialized(Component);
            Rename(Component);
        }

        private void Rename<T>(T component) where T : MonoBehaviour
        {
            component.name= typeof(T).ToString();
        }

        private static void UpdateGameService<T>(T ComponentReference) where T : MonoBehaviour
        {            
            if (ComponentReference is IGameService)
            {
                if(ComponentReference is SettingsManager)
                {
                    ServiceLocator.Current.Register<SettingsManager>(ComponentReference as SettingsManager);
                }
                
            }
        }

        private void ParentToManagers(GameObject gameObject)
        {
            gameObject.transform.SetParent(_managerParent.transform);
        }

        private void CheckifHasBeenInitialized<T>(T componentReference) where T : MonoBehaviour
        {
            if(_userSettingsInitialized && _inputSystemInitialized && _userSettingsInitialized)
            {
                _hasInitialized=true;
            }
        }

        private AssetReference GetAssetReference<T>() where T : MonoBehaviour
        {
            if (typeof(T).Name == typeof(SettingsManager).Name)
            {
                return SettingsManagerReference;
            }
            if (typeof(T).Name == typeof(UserInput).Name)
            {
                return UserInputSystemReference;
            }
                
            if (typeof(T).Name == typeof(UIManager).Name)
            {
                return UIManagerReference;
            }
                
            return null;
        }

        private void UpdateInitializationCheck<T>(T componentReference) where T : MonoBehaviour
        {
            if (componentReference is SettingsManager)
                _userSettingsInitialized = true;
            if (componentReference is UserInput)
                _inputSystemInitialized = true;
            if (componentReference is UIManager)
                _UIManagerInitialized = true;
        }

        private void Remove(AssetReference assetReference, NotifyOnDestroy obj)
        {
            Addressables.ReleaseInstance(obj.gameObject);
        }
    }

}