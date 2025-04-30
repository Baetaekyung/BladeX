using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Closet Object With Layer", story: "Find Closet [Object] With [Layer]", category: "Action", id: "bc26d545c6a395d2f9354aa82c61448b")]
public partial class FindClosetObjectWithLayerAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Object;
    [SerializeReference] public BlackboardVariable<float> radius;
            
    private LayerMask whatIsStone ;//= 1 << LayerMask.NameToLayer("Throwable"); 
    private readonly Collider[] nearTargets = new Collider[10];
    
    protected override Status OnStart()
    {
        whatIsStone = 1 << LayerMask.NameToLayer("Throwable"); 
        
        int count = Physics.OverlapSphereNonAlloc(Agent.Value.position, radius.Value, nearTargets, whatIsStone);
        
        if (count == 0)
            return Status.Failure;
        
        Transform closestObject = null;
        float closestDistance = float.MaxValue;
        
        for (int i = 0; i < count; i++)
        {
            var collider = nearTargets[i];
            float distance = Vector3.Distance(Agent.Value.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = collider.transform;
            }
        }
        
        if (closestObject != null)
        {
            Object.Value = closestObject;
            return Status.Success;
        }
        
        return Status.Failure;
    }
    
}