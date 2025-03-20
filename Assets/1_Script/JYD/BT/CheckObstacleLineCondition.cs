using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Obstacle Line", story: "Check [LayTrm] To [Target] Obstacle Line [CheckObstacleLine]", category: "Conditions", id: "9cf1c8bd09f0c320fa00df19338847a6")]
public partial class CheckObstacleLineCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> LayTrm;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<bool> CheckObstacleLine;
    
    private LayerMask whatIsTarget;
    
    public override bool IsTrue()
    {
        if(CheckObstacleLine.Value == false)
            whatIsTarget = 1 << Target.Value.gameObject.layer;
        else
            whatIsTarget = 1 << LayerMask.NameToLayer("Obstacle");
        
        if (LayTrm.Value == null || Target.Value == null)
            return false;
        
        Vector3 direction = (Target.Value.position - LayTrm.Value.position).normalized;

        if (Physics.Raycast(LayTrm.Value.position, direction, out RaycastHit hit, Mathf.Infinity, whatIsTarget))
        {
            return CheckObstacleLine.Value;
        }
        
        return CheckObstacleLine.Value;
    }



}
