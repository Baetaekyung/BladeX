using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerAttackState : BasePlayerState
    {
        private readonly IReadOnlyList<AnimationParameterSO> comboParamHash; 
        private readonly IReadOnlyList<Vector3> comboForceList; 
        private bool allowNextAttack;
        private int currentIdx;
        private readonly int maxIdx;
        public PlayerAttackState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) 
            : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            comboParamHash = entity.GetComboHashAtk;
            comboForceList = entity.GetComboForceList;
            maxIdx = comboParamHash.Count;
        }

        public override void Enter()
        {
            base.Enter();
            currentIdx = 0;
            Attack();
        }
        public override void Update()
        {
            UI_DebugPlayer.Instance.GetList[2].text = $"indx {currentIdx}";
            if (Input.GetKeyDown(KeyCode.K) && allowNextAttack && currentIdx < maxIdx)
            {
                int hash = comboParamHash[currentIdx].GetAnimationHash;
                PlayAnimation(hash);
                Attack();
                currentIdx++;
            }
        }
        private void Attack()
        {
            Vector3 a = comboForceList[currentIdx];
            Debug.Log(a);
            allowNextAttack = false;
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
