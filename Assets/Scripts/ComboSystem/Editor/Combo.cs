using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimatorItem;
namespace ComboSystem
{
    public class Combo : ScriptableObject
    {
        public string ComboName;
        public string ComboTriggerParameterName;
        public AnimatorControllerParameter AnimatorControllerParameter{get { return GetAnimatorControllerParameter(); } }
        private AnimatorControllerParameter _animatorControllerParameter;

        public bool Loopable=true;
        public ComboPiece[] ComboPieces;   
                
        public ComboPiece StartingComboPiece;
        [SerializeField][Multiline(10)]
        private string Description;
        private void OnValidate()
        {
            if(ComboPieces.Length>0)
            {
                StartingComboPiece=ComboPieces[0];
            }
            else
            {
                StartingComboPiece=null;
            }       
        }

        private AnimatorControllerParameter GetAnimatorControllerParameter()
        {
            if(_animatorControllerParameter==null)
            {
                _animatorControllerParameter=new AnimatorControllerParameter();
                _animatorControllerParameter.name=ComboTriggerParameterName;
                _animatorControllerParameter.type=AnimatorControllerParameterType.Trigger;
            }
            return _animatorControllerParameter;
        }

    }

}