using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerMoveState : BasePlayerState
    {
        public PlayerMoveState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
        
    }
}
