using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInit
{
    public abstract class GameInitializerSO : ScriptableObject
    {
        public abstract void Initialize();
        public abstract bool HasInitialized { get; }
    }
}