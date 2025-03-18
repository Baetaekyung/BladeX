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
        foreach (AnimatorControllerParameter parameter in animator.Value.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool ||
                parameter.type == AnimatorControllerParameterType.Trigger)
            {
                animator.Value.SetBool(parameter.name, false);
            }
        }
        
        
                
        return Status.Success;
    }

    
}

