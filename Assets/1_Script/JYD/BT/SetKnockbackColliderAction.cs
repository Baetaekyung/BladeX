using System;
using Swift_Blade.Enemy.Boss.Reaper;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set KnockbackCollider", story: "Set KnockbackCollider [Param]", category: "Action", id: "ea1744d0dfd718741d66c394b7b7bd7b")]
public partial class SetKnockbackColliderAction : Action
{
    [SerializeReference] public BlackboardVariable<ReaperBoss> ReaperBoss;
    [SerializeReference] public BlackboardVariable<bool> Param;

    protected override Status OnStart()
    {
        ReaperBoss.Value.SetKnockbackCollider(Param.Value);
        return Status.Success;
    }
    
}

