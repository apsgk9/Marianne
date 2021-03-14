using UnityEngine;
using UnityEditor;
using StateMachinePattern;
using System;
using System.Collections.Generic;

//https://answers.unity.com/questions/696091/how-do-you-instantiate-a-script-from-a-monoscript.html
public class StateMachinePatternEditor
{
    private static List<string> TypesThatCanBeConverted;
    private static bool success;

    [MenuItem("Assets/Create StateMachine SO")]
    public static void Create()
    {
        success = false;
        success = CheckifMonoScript();

        if (!success)
        {
            Debug.LogError("StateMachinePattern ScriptableObject could not be created.");
            return;
        }


        MonoScript monoScript = Selection.activeObject as MonoScript;

        try
        {
            success = CreateType<State>(monoScript);
            if(!success)
            {
                success = CreateType<StateTransitionCondition>(monoScript);
            }
            
        }
        catch (Exception e)
        {
            throw e;
        }
        if (success)
            return;
        Debug.LogError("StateMachinePattern ScriptableObject could not be created.");

    }

    private static bool CheckifMonoScript()
    {
        System.Type TypeFound = Selection.activeObject.GetType();
        if (typeof(UnityEditor.MonoScript) != TypeFound)
        {
            Debug.LogError("Not a monoscript.");
            return false;
        }
        return true;
    }

    private static bool CreateType<T>(MonoScript monoScript) where T : ScriptableObject
    {
        try
        {            
            T newScript= (T) ScriptableObject.CreateInstance(monoScript.GetClass());            
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            path=path.Replace(".cs", ".asset");
            ProjectWindowUtil.CreateAsset(newScript, path);
            return true;
        }
        catch
        {
            return false;
        }
        
        
    }
}