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
        private AssetReferenceGameObject UserSettingsReference;

        //initialize checks
        public override bool HasInitialized { get { return _hasInitialized; } }
        private bool _hasInitialized;
        private bool _UIManagerInitialized;
        private bool _inputSystemInitialized;
        private bool _userSettingsInitialized;
        public override void Initialize()
        {
            ResetInitializationToFalse();

            if (ServiceLocator.Current == null)
                ServiceLocator.Initialize();
            CreateUserSettings();
            CreateInputSystem();
            CreateUIManager();
        }

        private void ResetInitializationToFalse()
        {
            _hasInitialized = false;
            _UIManagerInitialized = false;
            _inputSystemInitialized = false;
            _userSettingsInitialized = false;
        }

        private void CreateUserSettings()
        {
            if (!GameObject.FindObjectOfType<SettingsManager>())
            {
                Addressables.InstantiateAsync(UserSettingsReference).Completed += SettingsManagerLoaded<SettingsManager>;
            }
            else
            {
                _userSettingsInitialized = true;
                Debug.LogWarning("SettingsManager already exists.");
            }
        }

        private void CreateInputSystem()
        {
            if (!GameObject.FindObjectOfType<UserInput>())
            {
                Addressables.InstantiateAsync(UserInputSystemReference).Completed += SettingsManagerLoaded<UserInput>;
            }
            else
            {
                _inputSystemInitialized = true;
                Debug.LogWarning("UserInput already exists.");
            }
        }

        private void CreateUIManager()
        {
            if (!GameObject.FindObjectOfType<UIManager>())
            {
                Addressables.InstantiateAsync(UIManagerReference).Completed += SettingsManagerLoaded<UIManager>;
            }
            else
            {
                _UIManagerInitialized = true;
                Debug.LogWarning("UIManager already exists.");
            }
        }


        private void SettingsManagerLoaded<T>(AsyncOperationHandle<GameObject> obj) where T : MonoBehaviour
        {
            var ComponentReference = obj.Result.GetComponent<T>();
            Type type = typeof(T);
            if (ComponentReference == null)
            {
                Debug.LogError($"{typeof(T).Name} component not found.");
            }

            ComponentReference.gameObject.name = ComponentReference.gameObject.name.Replace("(Clone)", "");

            var notify = ComponentReference.gameObject.AddComponent<NotifyOnDestroy>();
            notify.Destroyed += Remove;
            notify.AssetReference = GetAssetReference(ComponentReference);
            if (notify.AssetReference == null)
            {
                Debug.LogWarning("AssetReference is null on notify");
            }
            UpdateInitializationCheck(ComponentReference);
            Debug.Log($"{typeof(T).Name} has initialized.");
        }

        private AssetReference GetAssetReference<T>(T componentReference) where T : MonoBehaviour
        {
            if (componentReference is SettingsManager)
                return UserSettingsReference;
            if (componentReference is InputSettings)
                return UserInputSystemReference;
            if (componentReference is UIManager)
                return UIManagerReference;
            return null;
        }

        private void UpdateInitializationCheck<T>(T componentReference) where T : MonoBehaviour
        {
            if (componentReference is SettingsManager)
                _userSettingsInitialized = true;
            if (componentReference is InputSettings)
                _inputSystemInitialized = true;
            if (componentReference is UIManager)
                _userSettingsInitialized = true;
        }

        private void Remove(AssetReference assetReference, NotifyOnDestroy obj)
        {
            Addressables.ReleaseInstance(obj.gameObject);
        }
    }

}