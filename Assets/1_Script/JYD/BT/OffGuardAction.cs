using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "OffGuard", story: "OffGuard", category: "Action", id: "8a2a7e1e982b69291c2504e83973d1b5")]
public partial class OffGuardAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyHealth> enemyHealth;
    protected override Status OnStart()
    {
        enemyHealth.Value.OffGuarding();
        return Status.Success;
    }

}

