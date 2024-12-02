using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForAnimationEnd", story: "Wait for AnimationEnd", category: "Action", id: "76468958c1353048a0f3623f15a9a17b")]
public partial class WaitForAnimationEndAction : Action
{
    [SerializeReference] public BlackboardVariable<BossAnimationController> animationController;
    
    protected override Status OnUpdate()
    {
        if (animationController.Value.animationEnd)
        {
            return Status.Success;
        }
        
        return Status.Running;
    }

    protected override void OnEnd()
    {
        animationController.Value.StopAnimationEnd();
    }
}

