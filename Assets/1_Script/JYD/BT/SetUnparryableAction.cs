using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Swift_Blade.Combat.Caster;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetUnparryable", story: "SetUnparryable", category: "Action", id: "29a337b686e33b6b38898a201bd0df71")]
public partial class SetUnparryableAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseBossCaster> damageCaster;

    protected override Status OnStart()
    {
        damageCaster.Value.DisableParryForCurrentAttack();
        
        return Status.Success;
    }

    
}

