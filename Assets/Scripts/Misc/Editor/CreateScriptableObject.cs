using UnityEditor;
using UnityEngine;

public class CreateScriptableObject : MonoBehaviour
{
    [MenuItem("Assets/CreateScriptableObject")]
    public static void createScriptableObject()
    {
        if (Selection.activeObject is MonoScript)
        {
            MonoScript ms = (MonoScript)Selection.activeObject;
            ScriptableObject so = ScriptableObject.CreateInstance(ms.name);
 
            string path = System.IO.Directory.GetParent(AssetDatabase.GetAssetPath(ms.GetInstanceID())) + "/" + ms.name + ".asset";
            int cntr = 0;
            while (!createIfDoesntExists(path, so))
            {
                path = System.IO.Directory.GetParent(AssetDatabase.GetAssetPath(ms.GetInstanceID())) + "/" + ms.name + cntr.ToString() + ".asset";
 
                cntr++;
                if (cntr > 10)
                {
                    break;
                }
            }
            AssetDatabase.Refresh();
        }
    }
    public static bool createIfDoesntExists(string path, Object o)
    {
        var ap = AssetDatabase.LoadAssetAtPath(path, o.GetType());
        if (ap == null)
        {
            AssetDatabase.CreateAsset(o, path);
            return true;
        }
        else
        {
            return false;
        }
    }
}
