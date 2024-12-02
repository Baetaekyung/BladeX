using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerAttackState : BasePlayerState
    {
        private bool allowNextAttack;

        public PlayerAttackState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) 
            : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }

        public override void Enter()
        {
            allowNextAttack = false;
        }
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.K) && allowNextAttack)
                GetOwnerFsm.ChangeState(PlayerStateEnum.Idle);
        }
        public override void OnAnimationEndTrigger()
        {
            GetOwnerFsm.ChangeState(PlayerStateEnum.Idle);
        }
        public override void OnAnimationEndableTrigger()
        {
            allowNextAttack = true;
        }
    }
}
