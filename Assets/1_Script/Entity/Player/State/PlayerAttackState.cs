using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerAttackState : BasePlayerState
    {
        private readonly IReadOnlyList<AnimationParameterSO> comboParamHash;
        private readonly IReadOnlyList<Vector3> comboForceList;
        private readonly IReadOnlyList<float> perioids;
        private readonly PlayerMovement playerMovement;

        private bool allowListening;
        private bool allowNextAttack;
        private bool inputBuffer;

        private float deadPeriod;
        private int currentIdx;
        private readonly int maxIdx;
        private bool IsIndexValid => currentIdx < maxIdx;
        public PlayerAttackState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null)
            : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            comboParamHash = entity.GetComboHashAtk;
            comboForceList = entity.GetComboForceList;
            perioids = entity.GetPeriods;
            playerMovement = entity.GetPlayerMovement;
            maxIdx = comboParamHash.Count - 1;
        }

        public override void Enter()
        {
            base.Enter();
            if (IsIndexValid && deadPeriod > Time.time)
                currentIdx++;
            else currentIdx = 0;

            Attack();
        }
        public override void Update()
        {
            UI_DebugPlayer.Instance.GetList[1].text = $"indx {currentIdx}";
            UI_DebugPlayer.Instance.GetList[2].text = $"{deadPeriod > Time.time}";
            //UI_DebugPlayer.Instance.GetList[3].text = $"indx {allowListening}";
            UI_DebugPlayer.Instance.GetList[3].text = $"dp   {deadPeriod}, {Time.time}";
            //if (Input.GetKeyDown(KeyCode.K) && allowListening)
            //    inputBuffer = true;
            //if (allowNextAttack && inputBuffer && IsIndexValid)
            //{
            //    Attack();
            //    currentIdx++;
            //}
        }
        private void Attack()
        {
            inputBuffer = false;
            deadPeriod = perioids[currentIdx] + Time.time;
            AnimationParameterSO param = comboParamHash[currentIdx];
            PlayAnimation(param);
        }
        protected override void OnAnimationEndTrigger()
        {
            GetOwnerFsm.ChangeState(PlayerStateEnum.Idle);
        }
        protected override void OnAnimationEndTriggerListen()
        {
            allowListening = true;
        }
        protected override void OnAnimationEndableTrigger()
        {
            allowNextAttack = true;
        }
        protected override void OnForceEventTrigger(float force)
        {
            Vector3 result = comboForceList[currentIdx] * force;
            Debug.Log("idx" + currentIdx);
            Debug.Log(result);
            playerMovement.AdditionalForce = result;
        }
    }
}
