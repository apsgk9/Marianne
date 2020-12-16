using UnityEditor.Animations;
using UnityEngine;

public static class AnimatorAddParameters
{
    public static AnimatorControllerParameter TryAddingBooleanParameter(Animator animator,
    AnimatorControllerParameter Parameter,string GivenParameterName,bool SetTo=true)
    {
        AnimatorController animatorController = (AnimatorController)animator.runtimeAnimatorController;

        if(Parameter==null)
        {
            Parameter = FindParameter(animator, GivenParameterName);
        }

        if (Parameter==null)
        {
            AnimatorControllerParameter parameter = new AnimatorControllerParameter();
            parameter.type = AnimatorControllerParameterType.Bool;
            parameter.name = GivenParameterName;
            parameter.defaultBool = SetTo;
            animatorController.AddParameter(parameter);
            return parameter;
        }
        return Parameter;
    }

    private static AnimatorControllerParameter FindParameter(Animator animator, string GivenParameterName)
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            AnimatorControllerParameter tempParemeter = animator.GetParameter(i);
            if (tempParemeter.name == GivenParameterName)
            {
                return tempParemeter;
            }
        }
        return null;
    }

}
