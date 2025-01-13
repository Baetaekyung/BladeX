using System;
using Swift_Blade.Boss;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckBehind", story: "CheckBehind", category: "Action", id: "c46f8d85b6857b0486a530b47c06d814")]
public partial class CheckBehindAction : Action
{
    [SerializeReference] public BlackboardVariable<SwordBoss> agent;
    
    protected override Status OnStart()
    {
        if (agent.Value.CheckBehind())
            return Status.Success;
        
        return Status.Failure;
    }
    
}

