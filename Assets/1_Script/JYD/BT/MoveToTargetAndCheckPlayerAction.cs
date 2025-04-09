using Action = Unity.Behavior.Action;
using Swift_Blade.Enemy;
using Unity.Properties;
using UnityEngine.AI;
using Unity.Behavior;
using UnityEngine;
using System;

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

    private LayerMask whatIsObstacle;
    protected override Status OnStart()
    {
        if (Target.Value == null || Player.Value == null)
            return Status.Failure;
        
        whatIsObstacle = LayerMask.GetMask("Wall" , "Obstacle");
        
        Agent.Value.speed = MoveSpeed.Value;
        UpdateDistances();
        
        bool isNotObstacleLine = IsNotObstacleLine();
        
        if (IsPlayerTooClose() && isNotObstacleLine)
            return Status.Failure;
        
        if (IsTargetInRange() && isNotObstacleLine)
            return Status.Success;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        UpdateDistances();

        if (IsPlayerTooClose())
            return Status.Failure;
        
        BaseEnemy.Value.FactToTarget(BaseEnemy.Value.GetNextPathPoint());
        Agent.Value.SetDestination(Target.Value.position);
        
        bool isNotObstacleLine = IsNotObstacleLine();
        
        if (IsTargetInRange() && isNotObstacleLine)
            return Status.Success;
        
        return Status.Running;
    }

    private void UpdateDistances()
    {
        var agentPos = Agent.Value.transform.position;
        disToTarget = Vector3.Distance(Target.Value.position, agentPos);
        disToPlayer = Vector3.Distance(Player.Value.position, agentPos);
    }

    private bool IsPlayerTooClose()
    {
        return disToPlayer < RadiusToPlayer.Value;
    }

    private bool IsTargetInRange()
    {
        return disToTarget <= RadiusToTarget.Value;
    }
    
    private bool IsNotObstacleLine()
    {
        Vector3 direction = (Target.Value.transform.position - Agent.Value.transform.position);
        Vector3 start = Agent.Value.transform.position + new Vector3(0, 25f, 0);
        
        //Debug.DrawRay(start, direction * 100, Color.red);
        
        if (Physics.Raycast(start, direction.normalized,direction.magnitude, whatIsObstacle))
        {
            return false;
        }
        
        return true;
    }
}
