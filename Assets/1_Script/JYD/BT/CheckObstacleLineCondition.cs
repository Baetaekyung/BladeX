using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Obstacle Line", story: "CheckObstacle Is in Line", category: "Conditions", id: "9cf1c8bd09f0c320fa00df19338847a6")]
public partial class CheckObstacleLineCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    
    private LayerMask whatIsTarget;
    
    public override bool IsTrue()
    {
        Vector3 direction = (Target.Value.transform.position - Agent.Value.transform.position).normalized;
        Vector3 start = Agent.Value.transform.position + new Vector3(0, 1f, 0);
        
        Debug.DrawRay(start, direction * 100, Color.red);
        
        if (Physics.Raycast(start, direction, 100, whatIsTarget))
        {
            return true;
        }
        
        return false;
    }



}
