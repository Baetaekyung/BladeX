using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerIdleState : BasePlayerState
    {
        public PlayerIdleState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
                GetOwnerFsm.ChangeState(PlayerStateEnum.Attack);
        }
    }
}
