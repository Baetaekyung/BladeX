using System;
using Swift_Blade.Enemy.Boss.Goblin;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

[Serializable]
[GeneratePropertyBag]
[Condition("CheckSummon", story: "CheckSummon [GoblinBoss]", category: "Conditions",
    id: "03f4fabf01a3095eec5bd5a74043d2eb")]
public class CheckSummonCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GoblinBoss> goblinBoss;

    public override bool IsTrue()
    {
        return goblinBoss.Value.CanCreateSummon();
    }
}