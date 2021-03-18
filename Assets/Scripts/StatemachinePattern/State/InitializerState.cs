

using UnityEngine;

public class InitializerState : StateMachinePattern.State
{
    public bool Initialized {get{return _gameInitializer.HasInitialized;}}
    [SerializeField]
    private GameInit.GameInitializerSO _gameInitializer;

    public override void OnEnter()
    {
        Debug.Log("Intializing");
        _gameInitializer.Initialize();
    }
    

    public override void OnExit()
    {
        Debug.Log("Finshed Intializing");
    }

    public override void Tick()
    {
        
    }
}
