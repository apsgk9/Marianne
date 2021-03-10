using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UIList", order = 1)]
public class UIObjects : ScriptableObject
{
    public AssetReferenceGameObject PauseMenuGameObject;
    public AssetReferenceGameObject QuickMenuGameObject;
}