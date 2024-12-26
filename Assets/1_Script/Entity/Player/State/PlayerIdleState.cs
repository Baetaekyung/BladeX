using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerIdleState : BasePlayerMovementState
    {
        public PlayerIdleState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
    }
}
