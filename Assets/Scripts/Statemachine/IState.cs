using System.Collections.Generic;
using UnityEngine.AI;

public interface IState
{
    void Tick();
    void OnEnter();
    void OnExit();
}
