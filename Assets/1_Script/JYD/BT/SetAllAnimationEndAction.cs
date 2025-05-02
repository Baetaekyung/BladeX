using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetAllAnimationEnd", story: "AllAnimationEnd", category: "Action", id: "8392a5304c43b2991d0ff671a2cc3a34")]
public partial class SetAllAnimationEndAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> animator;
    
    protected override Status OnStart()
    {
        int count = animator.Value.parameterCount;

        for (int i = 0; i < count; i++)
        {
            AnimatorControllerParameter parameter = animator.Value.GetParameter(i);
            
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Bool:
                    animator.Value.SetBool(parameter.name, false);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    animator.Value.ResetTrigger(parameter.name);
                    break;
            }
        }
        
        return Status.Success;
    }

    
}

