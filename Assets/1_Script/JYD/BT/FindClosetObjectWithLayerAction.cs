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
    [SerializeReference] public BlackboardVariable<string> Layer;

    private LayerMask whatIsStone;
        
    protected override Status OnStart()
    {
        whatIsStone = 1 << LayerMask.NameToLayer(Layer.Value); 
        
        Collider[] nearbyObjects = Physics.OverlapSphere(Agent.Value.position, radius.Value, whatIsStone);
        
        if (nearbyObjects.Length == 0)
        {
            return Status.Failure; 
        }

        Transform closestObject = null;
        float closestDistance = float.MaxValue;
        
        foreach (Collider collider in nearbyObjects)
        {
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