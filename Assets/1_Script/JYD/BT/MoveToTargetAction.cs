using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Swift_Blade.Boss;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTarget", story: "Move To [Target]", category: "Action", id: "fe3d7f0e9abad8283274f183328a793d")]
public partial class MoveToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<BossBase> Boss;
            
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;
    [SerializeReference] public BlackboardVariable<float> stopDistance;
        
    private float distance;
    
    protected override Status OnStart()
    {
        distance = Vector3.Distance(Target.Value.transform.position , Agent.Value.transform.position);
        return distance <= stopDistance.Value ? Status.Success : Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 targetPos = Target.Value.transform.position;
        
        Agent.Value.SetDestination(targetPos);
        Boss.Value.FactToTarget(targetPos);
        distance = Vector3.Distance(targetPos , Agent.Value.transform.position);
        if (distance> stopDistance)
        {
            return Status.Running;
        }
                
        return Status.Success;
    }
}

