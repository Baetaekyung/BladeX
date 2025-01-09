using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public enum EPlayerAttackPreviousState
    {
        None,
        Dash,
        WeakAttack,
        PowerAttack
    }
    public class PlayerAttackState : BasePlayerMovementState
    {
        protected override bool BaseAllowAttackInput { get; } = false;

        //todo : change this to non-readonly variables and init when weapon is changed
        private readonly AnimationParameterSO dashAttack;
        private readonly IReadOnlyList<AnimationParameterSO> comboParamHash;
        private readonly IReadOnlyList<Vector3> comboForceList;
        private readonly IReadOnlyList<float> dealyArr;

        private readonly PlayerDamageCaster playerDamageCaster;
        private readonly PlayerRenderer playerRenderer;

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
        public EPlayerAttackPreviousState PreviousState { get; set; }
        public PlayerAttackState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO anim_dashAttack, AnimationParameterSO animParamSO = null)
            : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            comboParamHash = entity.GetComboHashAtk;
            comboForceList = entity.GetComboForceList;
            dealyArr = entity.GetPeriods;
            playerDamageCaster = entity.GetPlayerDamageCaster;
            playerRenderer = entity.GetPlayerRenderer;
            maxIdx = comboParamHash.Count - 1;
            dashAttack = anim_dashAttack;
        }

        public override void Enter()
        {
            base.Enter();
            if (PreviousState == EPlayerAttackPreviousState.Dash)
            {
                Attack(dashAttack);
            }
            else
            {
                //bool mouseMove = true;
                //Vector3 direction = mouseMove == true ?
                //    player.GetPlayerInput.GetMousePositionWorld - playerMovement.transform.position :
                //    player.GetPlayerInput.GetInputDirectionRawRotated;
                //playerRenderer.LookAtDirection(direction);
                //playerMovement.UseMouseLock = true;
                ComboAttack();
            }
        }
        public override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Mouse0) && allowListening)
                inputBuffer = true;
            if (inputBuffer && allowNextAttack && IsIndexValid)// && !dashAttackTrigger)
                ComboAttack();
        }
        private void Attack(AnimationParameterSO param)
        {
            currentIdx = 0;
            inputBuffer = false;
            allowListening = false;
            allowNextAttack = false;
            PlayAnimation(param);
        }
        private void ComboAttack()
        {
            if (IsIndexValid && IsDeadPeriodOver)
                currentIdx++;
            else
                currentIdx = 0;

            inputBuffer = false;
            allowListening = false;
            allowNextAttack = false;
            deadPeriod = dealyArr[currentIdx] + Time.time;

            //playerDamageCaster.CastDamage();

            AnimationParameterSO param = comboParamHash[currentIdx];
            PlayAnimation(param);
        }
        protected override void OnAnimationEndTriggerListen() => allowListening = true;
        protected override void OnAnimationEndableTrigger() => allowNextAttack = true;
        protected override void OnForceEventTrigger(float force)
        {
            Vector3 result = comboForceList[currentIdx] * force;
            playerMovement.AddForceFacingDirection(result);
        }
        public override void Exit()
        {
            playerMovement.SpeedMultiplierDefault = 1;
            //playerMovement.UseMouseLock = false;
            base.Exit();
        }
    }
}
