using System;
using Swift_Blade.Boss;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetApplyRootMotion", story: "RootMotion [Param]", category: "Action", id: "58403f578a4504714ddc80a114b7e9c7")]
public partial class SetApplyRootMotionAction : Action
{
    [SerializeReference] public BlackboardVariable<BossAnimationController> Animator;
    [SerializeReference] public BlackboardVariable<bool> Param;
    
    protected override Status OnStart()
    {
        if(Param.Value)
            Animator.Value.StartApplyRootMotion();
        else
            Animator.Value.StopApplyRootMotion();
        
        return Status.Success;
    }
    
}

