using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTarget", story: "Move To [Target]", category: "Action", id: "fe3d7f0e9abad8283274f183328a793d")]
public partial class MoveToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;
    [SerializeReference] public BlackboardVariable<float> stopDistance;
    [SerializeReference] public BlackboardVariable<float> moveSpeed;
    
    private float distance;
    
    protected override Status OnStart()
    {
        distance = Vector3.Distance(Target.Value.transform.position , Agent.Value.transform.position);
        return distance <= stopDistance.Value ? Status.Success : Status.Running;
    }

    protected override Status OnUpdate()
    {
        distance = Vector3.Distance(Target.Value.transform.position , Agent.Value.transform.position);
        Agent.Value.SetDestination(Target.Value.transform.position);
        if (distance> stopDistance)
        {
            Agent.Value.speed = moveSpeed.Value;
            return Status.Running;
        }
                
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

