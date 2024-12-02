using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerIdleState : BaseStateAnimation<PlayerStateEnum, Player>
    {
        public PlayerIdleState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, animTrigger, animParamSO)
        {
        }
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
                GetOwnerFsm.ChangeState(PlayerStateEnum.Attack);
        }
    }
}
