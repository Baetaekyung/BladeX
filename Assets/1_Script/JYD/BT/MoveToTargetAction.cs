using Action = Unity.Behavior.Action;
using Swift_Blade.Enemy;
using Unity.Properties;
using UnityEngine.AI;
using Unity.Behavior;
using UnityEngine;
using System;

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
        
    private LayerMask whatIsObstacle;
    private Vector3 targetPos;
    protected override Status OnStart()
    {
        if (Target.Value == null) 
            return Status.Failure;
        
        whatIsObstacle = LayerMask.GetMask("Wall" , "Obstacle");
        
        Agent.Value.speed = MoveSpeed.Value;
        
        targetPos = Target.Value.transform.position;
        distance = Vector3.Distance(targetPos, Agent.Value.transform.position);
        distance = Vector3.Distance(targetPos, Agent.Value.transform.position);
        
        bool isNotObstacleLine = IsNotObstacleLine();
        bool isNearTarget = (distance <= attackDistance.Value || distance <= meleeAttackDistance.Value);
        
        if (isNearTarget && isNotObstacleLine)
            return Status.Success;
        
        return Status.Running;
    }
    
    protected override Status OnUpdate()
    {
        targetPos = Target.Value.transform.position;
        
        Boss.Value.FactToTarget(Boss.Value.GetNextPathPoint());
        Agent.Value.SetDestination(targetPos);
        
        distance = Vector3.Distance(targetPos, Agent.Value.transform.position);
        distance = Vector3.Distance(targetPos, Agent.Value.transform.position);
        
        bool isNotObstacleLine = IsNotObstacleLine();
        bool isNearTarget = (distance <= attackDistance.Value || distance <= meleeAttackDistance.Value);
        
        //Debug.Log($"is Near target : {isNearTarget}, " + $"IsObstacleLine : {isNotObstacleLine}");
        
        if (isNearTarget && isNotObstacleLine)
            return Status.Success;
        
        return Status.Running;
    }
    
    private bool IsNotObstacleLine()
    {
        Vector3 direction = (Target.Value.transform.position - Agent.Value.transform.position);
        Vector3 start = Agent.Value.transform.position + new Vector3(0, 1f, 0);
        
        //Debug.DrawRay(start, direction * 100, Color.red);
        
        if (Physics.Raycast(start, direction.normalized,out  RaycastHit hit,direction.magnitude, whatIsObstacle))
        {
            Debug.Log(hit.collider.gameObject.name);
            return false;
        }
        
        return true;
    }
}