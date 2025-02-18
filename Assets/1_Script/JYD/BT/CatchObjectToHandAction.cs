using System;
using Swift_Blade.Combat.Projectile;
using Swift_Blade.Enemy;
using Swift_Blade.Enemy.Boss.Golem;
using Swift_Blade.Enemy.Throw;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CatchObjectToHand", story: "Catch [Object] To [Hand]", category: "Action", id: "8baec28340a68019b24f6634b586daf6")]
public partial class CatchObjectToHandAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseEnemyAnimationController> animationController;
    [SerializeReference] public BlackboardVariable<Transform> Object;
    
    private ThrowAnimatorController animator;
        
    protected override Status OnStart()
    {
        if (Object.Value == null)
            return Status.Failure;
        if(animator == null)
            animator = (animationController.Value as ThrowAnimatorController);
        if (animator == null)
            return Status.Failure;
        
        animator.SetStone(Object.Value.GetComponent<BaseThrow>());
        
        return Status.Success;
    }

    protected override void OnEnd()
    {
        Object.Value = null;
    }
}

