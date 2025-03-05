using System;
using Swift_Blade.Enemy;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable]
[GeneratePropertyBag]
[NodeDescription("MoveToTarget", story: "Move To [Target]", category: "Action", id: "fe3d7f0e9abad8283274f183328a793d")]
public class MoveToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseEnemy> Boss;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;

    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;

    [SerializeReference] public BlackboardVariable<float> meleeAttackDistance;
    [SerializeReference] public BlackboardVariable<float> attackDistance;

    private float distance;

    protected override Status OnStart()
    {
        if (Target.Value == null) 
            return Status.Failure;
        
        Agent.Value.speed = MoveSpeed.Value;

        distance = Vector3.Distance(Target.Value.transform.position, Agent.Value.transform.position);

        if (distance <= attackDistance.Value || distance <= meleeAttackDistance.Value) 
            return Status.Success;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var targetPos = Target.Value.transform.position;

        Boss.Value.FactToTarget(Boss.Value.GetNextPathPoint());
        Agent.Value.SetDestination(targetPos);

        distance = Vector3.Distance(targetPos, Agent.Value.transform.position);
        distance = Vector3.Distance(targetPos, Agent.Value.transform.position);
        
        if (distance <= attackDistance.Value || distance <= meleeAttackDistance.Value) 
            return Status.Success;
        
        return Status.Running;
    }
}