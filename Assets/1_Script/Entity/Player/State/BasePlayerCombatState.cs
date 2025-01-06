using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public abstract class BasePlayerCombatState : BasePlayerState
    {
        public BasePlayerCombatState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
    }
}
