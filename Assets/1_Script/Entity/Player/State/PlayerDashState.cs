using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerDashState : BasePlayerMovementState
    {
        protected override bool BaseAllowStateChangeToDash => false;
        protected override bool BaseAllowStateChangeToAttack => false;
        protected override bool BaseAllowStateChangeToParry => false;
        public PlayerDashState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
        public override void Enter()
        {
            base.Enter();
            player.GetPlayerMovement.AllowInputMove = false;
            entity.GetPlayerMovement.Dash(entity.GetPlayerInput.GetInputDirection.normalized, 10);
        }
        public override void Exit()
        {
            player.GetPlayerMovement.AllowInputMove = true;
            base.Exit();
        }
    }
}
