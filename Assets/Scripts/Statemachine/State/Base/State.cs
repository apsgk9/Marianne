using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace StateMachinePattern
{
    
    public abstract class State : ScriptableObject
    {    
        public abstract void Tick();
        public abstract void OnEnter();
        public abstract void OnExit();
    }
}