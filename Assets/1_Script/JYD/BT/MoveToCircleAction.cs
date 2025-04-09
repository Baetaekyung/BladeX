using System;
using Swift_Blade.Enemy.Boss.Reaper;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToCircle", story: "Move To Circle", category: "Action", id: "fdf665612d039d9b038a944419f53037")]
public partial class MoveToCircleAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<float> MinDistance;
    
    [SerializeReference] public BlackboardVariable<Transform> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
        
    [SerializeReference] public BlackboardVariable<int> moveDirection;
    
    private ReaperBoss reaperBoss;
    
    private float radius;
    private float angle;
    private bool isCircling = false;
    private float fixedY;
    
    protected override Status OnStart()
    {
        float currentDistance = Vector3.Distance(Agent.Value.position, Target.Value.position);
        radius = Mathf.Max(currentDistance, MinDistance.Value);
        
        Vector3 directionToAgent = Agent.Value.position - Target.Value.position;
        angle = Mathf.Atan2(directionToAgent.z, directionToAgent.x);
        
        fixedY = Agent.Value.position.y;

        reaperBoss = Agent.Value.GetComponent<ReaperBoss>();
        reaperBoss.SetCollision(false);
        
        return Status.Running;
    }
    
    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }
        
        Vector3 agentXZ = new Vector3(Agent.Value.position.x, 0, Agent.Value.position.z);
        Vector3 targetXZ = new Vector3(Target.Value.position.x, 0, Target.Value.position.z);
        float currentDistance = Vector3.Distance(agentXZ, targetXZ);
        
        if (currentDistance <= MinDistance.Value)
        {
            isCircling = false;
            MoveAwayFromTarget();
        }
        else
        {
            if (!isCircling)
            {
                radius = Mathf.Max(MinDistance.Value, currentDistance);
                isCircling = true;
            }
            UpdateCircularPosition();
        }

        return Status.Running;
    }

    private void MoveAwayFromTarget()
    {
        Vector3 directionFromTarget = (Agent.Value.position - Target.Value.position);
        directionFromTarget.y = 0;
        Vector3 targetPosition = Target.Value.position + directionFromTarget * (MinDistance.Value + 1);
        
        Agent.Value.position = Vector3.MoveTowards(Agent.Value.position, targetPosition, Speed.Value / 2 * Time.deltaTime);
    }
    
    private void UpdateCircularPosition()
    {
        float rotationSpeed = Speed.Value / radius; 
        angle += moveDirection.Value * rotationSpeed * Time.deltaTime;
        
        
        Vector3 targetPosition = new Vector3(
            Target.Value.position.x + radius * Mathf.Cos(angle),
            fixedY,
            Target.Value.position.z + radius * Mathf.Sin(angle)
        );
        
        Agent.Value.position = Vector3.MoveTowards(
            Agent.Value.position, 
            targetPosition, 
            Speed.Value * Time.deltaTime
        );
    }

    
    
    protected override void OnEnd()
    {
        reaperBoss.SetCollision(true);
    }
}