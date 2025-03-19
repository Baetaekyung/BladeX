using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerInteractState : BasePlayerState
    {
        public PlayerInteractState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
        public override void Enter()
        {
            player.GetPlayerMovement.InputDirection = Vector3.zero;
            if (baseAnimParam != null)
                PlayAnimationOnEnter();
            player.GetPlayerAnimator.GetAnimator.SetFloat("X", 0);
            player.GetPlayerAnimator.GetAnimator.SetFloat("Z", 0);

            OnAllowRotateDisallowTrigger();
            OnSpeedMultiplierDefaultTrigger(0);
        }
        public override void Update()
        {

        }
        public override void Exit()
        {
            OnAllowRotateAllowTrigger();
            OnSpeedMultiplierDefaultTrigger(1);
        }
    }
}
