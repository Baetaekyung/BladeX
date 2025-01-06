using System;
using Swift_Blade.Boss;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LookatTarget", story: "LookAt [Param]", category: "Action", id: "31c46e8d3f8eb164ea3a28143b96b7cf")]
public partial class LookatTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Param;
    [SerializeReference] public BlackboardVariable<BossAnimationController> AnimatorController;
    protected override Status OnStart()
    {
        AnimatorController.Value.isManualRotate = Param.Value;
        return Status.Success;
    }
    
}

