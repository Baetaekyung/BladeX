using System;
using Swift_Blade.Enemy;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using UnityEngine.Serialization;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTargetAndCheckPlayer", story: "Move To [Target] in [Radius] [Player]", category: "Action", id: "b96944a995977f3bce8c42518732d19f")]
public partial class MoveToTargetAndCheckPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseEnemy> BaseEnemy;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<Transform> Player;
    
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;
    
    [SerializeReference] public BlackboardVariable<float> RadiusToTarget;
    [SerializeReference] public BlackboardVariable<float> RadiusToPlayer;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;
    
    private float disToTarget;
    private float disToPlayer;
    
    protected override Status OnStart()
    {
        if (Target.Value == null || Player.Value == null) 
            return Status.Failure;
    
        Agent.Value.speed = MoveSpeed.Value;
    
        Vector3 agentPos = Agent.Value.transform.position;
        disToTarget = Vector3.Distance(Target.Value.position, agentPos);
        disToPlayer = Vector3.Distance(Player.Value.position, agentPos);
        
        if (disToPlayer < RadiusToPlayer.Value)
            return Status.Failure;
        
        if (disToTarget <= RadiusToTarget.Value) 
            return Status.Success;
    
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var targetPos = Target.Value.position;
        var playerPos = Player.Value.position;
        var agentPos = Agent.Value.transform.position;

        disToTarget = Vector3.Distance(targetPos, agentPos);
        disToPlayer = Vector3.Distance(playerPos, agentPos);

        if (disToPlayer < RadiusToPlayer.Value)
            return Status.Failure;

        BaseEnemy.Value.FactToTarget(BaseEnemy.Value.GetNextPathPoint());
        Agent.Value.SetDestination(targetPos);
        
        if (disToTarget <= RadiusToTarget.Value) 
            return Status.Success;
    
        return Status.Running;
    }

}
