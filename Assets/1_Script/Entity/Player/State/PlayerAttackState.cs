using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerAttackState : BasePlayerMovementState
    {
        protected override bool BaseAllowStateChangeToAttack { get; } = false;

        private readonly IReadOnlyList<AnimationParameterSO> comboParamHash;
        private readonly IReadOnlyList<Vector3> comboForceList;
        private readonly IReadOnlyList<float> perioids;
        private readonly PlayerMovement playerMovement;
        private readonly PlayerDamageCaster playerDamageCaster;

        private bool allowListening;
        private bool allowNextAttack;
        private bool inputBuffer;

        private float deadPeriod;
        private int currentIdx;
        private readonly int maxIdx;
        private bool IsIndexValid => currentIdx < maxIdx;
        /// <summary>
        /// bad name
        /// </summary>
        private bool IsDeadPeriodOver => deadPeriod > Time.time;
        public PlayerAttackState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null)
            : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            comboParamHash = entity.GetComboHashAtk;
            comboForceList = entity.GetComboForceList;
            perioids = entity.GetPeriods;
            playerMovement = entity.GetPlayerMovement;
            playerDamageCaster = entity.GetPlayerDamageCaster;
            maxIdx = comboParamHash.Count - 1;
            Player.Debug_Updt += () =>
            {
                //UI_DebugPlayer.Instance.GetList[2].text = $"inp     {inputBuffer}";
                //UI_DebugPlayer.Instance.GetList[3].text = $"allowLis{allowListening}, {allowNextAttack}";
            };
        }

        public override void Enter()
        {
            base.Enter();
            Attack();
        }
        public override void Update()
        {
            base.Update();
            //UI_DebugPlayer.Instance.GetList[1].text = $"indx {currentIdx}";
            if (Input.GetKeyDown(KeyCode.K) && allowListening)
                inputBuffer = true;
            if (inputBuffer && allowNextAttack && IsIndexValid)
            {
                Attack();
                Debug.Log("dwadawda");
            }
        }
        private void Attack()
        {
            if (IsIndexValid && IsDeadPeriodOver)
                currentIdx++;
            else
                currentIdx = 0;

            inputBuffer = false;
            allowListening = false;
            allowNextAttack = false;
            deadPeriod = perioids[currentIdx] + Time.time;

            playerDamageCaster.CastDamage();
            
            AnimationParameterSO param = comboParamHash[currentIdx];
            PlayAnimation(param);
        }
        protected override void OnAnimationEndTrigger() => GetOwnerFsm.ChangeState(PlayerStateEnum.Movement);
        protected override void OnAnimationEndTriggerListen() => allowListening = true;
        protected override void OnAnimationEndableTrigger() => allowNextAttack = true;
        protected override void OnSpeedMultiplierDefaultTrigger(float set) => playerMovement.SpeedMultiplierDefault = set;
        protected override void OnMovementSetTrigger(Vector3 value) => playerMovement.SetVelocitiy(Vector3.zero);
        public override void Exit()
        {
            Debug.Log("exit");
            playerMovement.SpeedMultiplierDefault = 1;
            base.Exit();
        }
        protected override void OnForceEventTrigger(float force)
        {
            Vector3 result = comboForceList[currentIdx] * force;
            playerMovement.AddForceLocaly(result);
        }
    }
}
