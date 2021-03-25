using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimatorItem;

namespace ComboSystem
{
    public class ComboPiece : ScriptableObject
    {  
        public string ComboPieceName;
        public Motion Motion;        
        public StateMachineBehaviour StateMachineBehaviour;
        [Header("Exit Combo")]
        
        [Range(0f,1f)]
        public float DashCancelTime=0.2f;
        public bool DashCancelFixedDuration=true;
        public float DashCancelDuration=0.05f;
        [Range(0f,1f)]
        public float WalkRunCancelTime=0.5f;
        public bool WalkRunCancelFixedDuration=true;
        public float WalkRunCancelDuration=0.05f;
        [Header("Next Piece")]
        [Range(0f,1f)]
        public float NextMoveCancelTime=0.5f;
        public bool NextMoveCancelFixedDuration=true;
        public float NextMoveCancelDuration=0.05f;
        [SerializeField][Multiline(10)]
        private string Description;
    }
}

