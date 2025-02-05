using System;
using Swift_Blade.Enemy.Boss.Reaper;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DisappearGround", story: "DisappearGround", category: "Action", id: "773eb57e5da47d8325b90ac9f903ea92")]
public partial class DisappearGround : Action
{
    [SerializeReference] public BlackboardVariable<ReaperBoss> boss;
    [SerializeReference] public BlackboardVariable<Transform> target;
    [SerializeReference] public BlackboardVariable<float> radius;
    [SerializeReference] public BlackboardVariable<float> speed;
    
    private Vector3 randomPosition;
    private MoveState moveState;
    
    private float originYpos;
    private float groundYpos;
    private enum MoveState
    {
        Down,
        Move,
        Up
    }
    
    protected override Status OnStart()
    {
        var position = boss.Value.transform.position;
        boss.Value.MoveInGround();
        moveState = MoveState.Down;

        originYpos = position.y;
        groundYpos = originYpos - 6;
        
        Vector2 randomCircle = Random.insideUnitCircle.normalized * radius;
        
        randomPosition = new Vector3(
            target.Value.position.x + randomCircle.x,
            groundYpos, 
            target.Value.position.z + randomCircle.y
        );
        
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var position = boss.Value.transform.position;
        
        if (moveState == MoveState.Down)
        {
            Vector3 destination = new Vector3(position.x, groundYpos, position.z);
            position = Vector3.MoveTowards(position, destination, Time.deltaTime * speed.Value);
            boss.Value.transform.position = position;
            
            if (Vector3.Distance(position, destination) <= 0.1f)
            {
                moveState = MoveState.Move;
            }
        }
        else if (moveState == MoveState.Move)
        {
            position = Vector3.MoveTowards(position, randomPosition, Time.deltaTime * speed.Value);
            boss.Value.transform.position = position;
            if (Vector3.Distance(position, randomPosition) <= 0.5f)
            {
                moveState = MoveState.Up;
            }
        }
        else if (moveState == MoveState.Up)
        {
            boss.Value._reaperAnimatorController.MoveUp();
            Vector3 destination = new Vector3(position.x, originYpos, position.z);
            
            position = Vector3.MoveTowards(position, destination, Time.deltaTime * speed.Value);
            boss.Value.transform.position = position;
            if (Vector3.Distance(position, destination) <= 0.1f)
            {
                return Status.Success;
            }
        }
        
        return Status.Running;
    }

    protected override void OnEnd()
    {
        boss.Value.MoveOutGround();
    }
}
