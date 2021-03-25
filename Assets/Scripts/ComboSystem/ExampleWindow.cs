using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using ComboSystem;
using PC;

public class ExampleWindow : EditorWindow
{
    private AnimatorController _animatorController;
    private Combo _combo;
    private CharacterAnimatorNamingList _CharacterAnimatorNamingList;
    private bool _hasSetupParameters=false;

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        _animatorController = (AnimatorController)EditorGUILayout.ObjectField("Animator Controller", _animatorController, typeof(AnimatorController),true);
        _CharacterAnimatorNamingList = (CharacterAnimatorNamingList)EditorGUILayout.ObjectField("Animator Controller", _CharacterAnimatorNamingList, typeof(CharacterAnimatorNamingList),true);
        
        if (_animatorController && _CharacterAnimatorNamingList)
        {
            ConfigureAnimatorController();
        }
        EditorGUILayout.EndVertical();
    }

    private void ConfigureAnimatorController()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("ConfigureAnimatorController:");
        _combo = (Combo)EditorGUILayout.ObjectField("Combo", _combo, typeof(Combo),true);
        
        if (_combo)
        {
            GUILayout.Label("Parameters:");
            if(GUILayout.Button("Setup Parameters"))
            {
                SetUpParameters();
            }
            if(GUILayout.Button("Remove Parameters"))
            {
                RemoveParameters();
            }
            
            if(_hasSetupParameters)
            {
                GUILayout.Label("StateMachine:");
                if(GUILayout.Button("Add Combo"))
                {
                    AddCombo(_combo);
                }
                if(GUILayout.Button("Remove Combo"))
                {
                    RemoveCombo(_combo);
                }     
            }
                   
        }

        
        
        EditorGUILayout.EndVertical();
    }

    private void RemoveParameters()
    {
        List<AnimatorControllerParameter> AnimatorControllerParameters = GetAnimatorControllerParameter();
        
        RemoveParametersFromController(_animatorController, AnimatorControllerParameters);
        _hasSetupParameters=false;
    }

    

    private void SetUpParameters()
    {
        List<AnimatorControllerParameter> AnimatorControllerParameters = GetAnimatorControllerParameter();

        InsertParameters(_animatorController, AnimatorControllerParameters);
        _hasSetupParameters=true;
    }
    

    private List<AnimatorControllerParameter> GetAnimatorControllerParameter()
    {
        if (_combo == null)
        {
            Debug.LogError("Combo is null.");
            return null;
        }
        if (_CharacterAnimatorNamingList == null)
        {
            Debug.LogError("CharacterAnimatorNamingList is null.");
            return null;
        }
        List<AnimatorControllerParameter> AnimatorControllerParameters = new List<AnimatorControllerParameter>();

        AnimatorControllerParameter _normalizedParam = ParameterCreator.CreateParameter(_CharacterAnimatorNamingList.NormalizedTimeParameterName,AnimatorControllerParameterType.Float);
        AnimatorControllerParameter _speedParam = ParameterCreator.CreateParameter(_CharacterAnimatorNamingList.SpeedParameterName,AnimatorControllerParameterType.Float);
        

        AnimatorControllerParameters.Add(_combo.AnimatorControllerParameter);
        AnimatorControllerParameters.Add(_normalizedParam);
        AnimatorControllerParameters.Add(_speedParam);
        return AnimatorControllerParameters;
    }

    

    private void InsertParameters(AnimatorController animatorController, List<AnimatorControllerParameter> Parameters)
    {
        foreach(var animParam in animatorController.parameters)
        {
            var type = animParam.type;
            var name = animParam.name;
            for(int i=0;i<Parameters.Count;i++)
            {
                if(type == Parameters[i].type && name == Parameters[i].name)
                {
                    Parameters.RemoveAt(i);
                    break;
                }
            }       
        }

        if(Parameters.Count>0)
        {
            for(int i=0;i<Parameters.Count;i++)
            {
                animatorController.AddParameter(Parameters[i].name,Parameters[i].type);                
            }
        }
    }

    private void RemoveParametersFromController(AnimatorController animatorController, List<AnimatorControllerParameter> Parameters)
    {
        
        List<AnimatorControllerParameter> ToRemove= new List<AnimatorControllerParameter>();
        foreach(var animParam in animatorController.parameters)
        {
            var type = animParam.type;
            var name = animParam.name;
            for(int i=0;i<Parameters.Count;i++)
            {
                if(type == Parameters[i].type && name == Parameters[i].name)
                {
                    ToRemove.Add(animParam);
                    Parameters.RemoveAt(i);
                    break;
                }
            }       
        }

        foreach(var paramToRemove in ToRemove)
        {
            animatorController.RemoveParameter(paramToRemove);       
        }
    }

    private void RemoveCombo(Combo combo)
    {
        var rootStateMachine = _animatorController.layers[0].stateMachine;
        foreach(var SM in rootStateMachine.stateMachines)
        {
            if(SM.stateMachine.name==combo.ComboName)
            {
                rootStateMachine.RemoveStateMachine(SM.stateMachine);
                break;
            }
        }
    }

    private void AddCombo(Combo combo)
    {
        var rootStateMachine  = _animatorController.layers[0].stateMachine;
        if(!_hasSetupParameters)
        {
            Debug.LogError("Setup Parameters First.");
            return;
        }
        
        //CheckDuplicate
        bool isDuplicate=false;
        foreach(var SM in rootStateMachine.stateMachines)
        {
            if(SM.stateMachine.name==combo.ComboName)
            {
                isDuplicate=true;
                break;
            }
        }
        if(isDuplicate)
        {
            Debug.LogError("Cannot add duplicate combos");
            return;
        }
            

        //Add
        var ComboStateMachine = rootStateMachine.AddStateMachine(combo.ComboName,rootStateMachine.anyStatePosition+Vector3.down*100);

        List<AnimatorState> ComboStates= new List<AnimatorState>();
        List<AnimatorStateTransition> ComboTransitions= new List<AnimatorStateTransition>();
        List<AnimatorStateTransition> ExitTransitions= new List<AnimatorStateTransition>();
        List<AnimatorStateTransition> ExitTransitionsDashCancel= new List<AnimatorStateTransition>();
        List<AnimatorStateTransition> ExitTransitionsWalkRunCancel= new List<AnimatorStateTransition>();

        //Create States
        foreach(var cp in combo.ComboPieces)
        {
            var state=ComboStateMachine.AddState(cp.ComboPieceName);
            state.motion=cp.Motion;
            ComboStates.Add(state);
        }


        //AddTransitions Between Combos
        for(int i=1;i<ComboStates.Count;i++)
        {
            ComboTransitions.Add(ComboStates[i-1].AddTransition(ComboStates[i]));
        }
        ////AddExit Transitions From Each Combo
        foreach(var state in ComboStates)
        {
            ExitTransitions.Add(state.AddExitTransition());
            ExitTransitionsDashCancel.Add(state.AddExitTransition());
            ExitTransitionsWalkRunCancel.Add(state.AddExitTransition());
        }

        //Loop
        if(combo.Loopable && combo.ComboPieces.Length>1)
        {
            ComboTransitions.Add(ComboStates[ComboStates.Count-1].AddTransition(ComboStates[0]));
        }

        ////AddConditions
        //InBetween
        for(int i=0;i<ComboTransitions.Count;i++)
        {
            ComboTransitions[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0,combo.ComboTriggerParameterName);
            ComboTransitions[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Greater, combo.ComboPieces[i].NextMoveCancelTime,_CharacterAnimatorNamingList.NormalizedTimeParameterName);
            ComboTransitions[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Less, 1,_CharacterAnimatorNamingList.NormalizedTimeParameterName);

            
            ComboTransitions[i].duration= combo.ComboPieces[i].NextMoveCancelDuration;
            ComboTransitions[i].hasFixedDuration=combo.ComboPieces[i].NextMoveCancelFixedDuration;
        }

        //Exits
        for(int i=0;i<ExitTransitionsDashCancel.Count;i++)
        {
            //Normal Exit
            ExitTransitions[i].hasExitTime=true;
            ExitTransitions[i].exitTime=1f;
            //Dash
            ExitTransitionsDashCancel[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Greater, ((float)LocomotionEnmus.LocomotionMode.Sprint)-0.001f,_CharacterAnimatorNamingList.SpeedParameterName);
            ExitTransitionsDashCancel[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Greater, combo.ComboPieces[i].DashCancelTime,_CharacterAnimatorNamingList.NormalizedTimeParameterName);
            ExitTransitionsDashCancel[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Less, 1,_CharacterAnimatorNamingList.NormalizedTimeParameterName);

            
            ExitTransitionsDashCancel[i].duration= combo.ComboPieces[i].DashCancelDuration;
            ExitTransitionsDashCancel[i].hasFixedDuration=combo.ComboPieces[i].DashCancelFixedDuration;


            //WalkRun
            ExitTransitionsWalkRunCancel[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Greater, ((float)LocomotionEnmus.LocomotionMode.Walk)-0.001f,_CharacterAnimatorNamingList.SpeedParameterName);
            ExitTransitionsWalkRunCancel[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Less, ((float)LocomotionEnmus.LocomotionMode.Sprint),_CharacterAnimatorNamingList.SpeedParameterName);
            ExitTransitionsWalkRunCancel[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Greater, combo.ComboPieces[i].WalkRunCancelTime,_CharacterAnimatorNamingList.NormalizedTimeParameterName);
            ExitTransitionsWalkRunCancel[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Less, 1,_CharacterAnimatorNamingList.NormalizedTimeParameterName);

            
            ExitTransitionsWalkRunCancel[i].duration= combo.ComboPieces[i].WalkRunCancelDuration;
            ExitTransitionsWalkRunCancel[i].hasFixedDuration=combo.ComboPieces[i].WalkRunCancelFixedDuration;

        }
    }

    [MenuItem("Window/ComboMaker")]
    public static void ShowWindow()
    {
        GetWindow<ExampleWindow>("ComboMaker");
    }
}
