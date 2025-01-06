using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToCircle", story: "Move To Circle", category: "Action", id: "fdf665612d039d9b038a944419f53037")]
public partial class MoveToCircleAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<Transform> Agent ;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    private float radius;
    private float angle; 
    private Vector3 targetPosition; 
    
    protected override Status OnStart()
    {
        radius = Vector3.Distance(Agent.Value.position , Target.Value.position);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        Agent.Value.position = Vector3.MoveTowards(Agent.Value.position, targetPosition, Speed.Value * Time.deltaTime);

        if (Vector3.Distance(Agent.Value.position, targetPosition) < 0.1f)
        {
            UpdateTargetPosition();
        }

        return Status.Running;
    }

    private void UpdateTargetPosition()
    {
        angle += Mathf.PI / 16;

        float x = Target.Value.position.x + radius * Mathf.Cos(angle);
        float z = Target.Value.position.z + radius * Mathf.Sin(angle);
        targetPosition = new Vector3(x, Agent.Value.position.y, z);
    }
    
    protected override void OnEnd()
    {
    }
}

