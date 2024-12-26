using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerDashState : BasePlayerState
    {
        public PlayerDashState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
        public override void Enter()
        {
            base.Enter();

            entity.GetPlayerMovement.Dash(entity.GetPlayerInput.InputDirection.normalized, 7);
            GetOwnerFsm.ChangeState(PlayerStateEnum.Movement);
        }
    }
}
