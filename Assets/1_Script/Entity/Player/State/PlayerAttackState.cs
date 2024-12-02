using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerAttackState : BaseStateAnimation<PlayerStateEnum, Player>
    {
        public PlayerAttackState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, animTrigger, animParamSO)
        {
        }
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
                GetOwnerFsm.ChangeState(PlayerStateEnum.Idle);
        }
        public override void OnAnimationEndTrigger()
        {
            GetOwnerFsm.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
