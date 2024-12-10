using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckCanAttack", story: "Check Can Attack [CoolDown]", category: "Conditions", id: "f5bcb111427e5f49ce41f7f54b87b874")]
public partial class CheckCanAttackCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> CoolDown;
    private float lastAttackTime = 0;
    
    public override bool IsTrue()
    {
        float currentTime = Time.time; 
        if (currentTime > lastAttackTime + CoolDown.Value)
        {
            lastAttackTime = currentTime;
            return true;
        }
        return false;
    }
}