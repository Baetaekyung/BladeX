using System;
using Swift_Blade.Boss.Goblin;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckSummon", story: "CheckSummon [GoblinBoss]", category: "Conditions", id: "03f4fabf01a3095eec5bd5a74043d2eb")]
public partial class CheckSummonCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GoblinBoss> goblinBoss;
    
    public override bool IsTrue()
    {
        return goblinBoss.Value.CanCreateSummon();
    }
    
}
