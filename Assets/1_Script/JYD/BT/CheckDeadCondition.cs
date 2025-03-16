using Swift_Blade.Combat.Health;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckDead ", story: "Check Dead [EnemyHealth]", category: "Conditions", id: "0871b6ec483d5ef542da5349820fcc4d")]
public partial class CheckDeadCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseEnemyHealth> EnemyHealth;

    public override bool IsTrue()
    {
        return EnemyHealth.Value.isDead;
    }
}
