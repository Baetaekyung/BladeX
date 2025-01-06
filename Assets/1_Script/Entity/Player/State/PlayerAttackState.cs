using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerAttackState : BasePlayerMovementState
    {
        protected override bool BaseAllowStateChangeToAttack { get; } = false;

        //todo : change this to non-readonly variables and init when weapon is changed
        private readonly IReadOnlyList<AnimationParameterSO> comboParamHash;
        private readonly IReadOnlyList<Vector3> comboForceList;
        private readonly IReadOnlyList<float> perioids;
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
        public PlayerAttackState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null)
            : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            comboParamHash = entity.GetComboHashAtk;
            comboForceList = entity.GetComboForceList;
            perioids = entity.GetPeriods;
            playerDamageCaster = entity.GetPlayerDamageCaster;
            playerRenderer = entity.GetPlayerRenderer;
            maxIdx = comboParamHash.Count - 1;
            Player.Debug_Updt += () =>
            {
                UI_DebugPlayer.DebugText(3, inputBuffer, "inputBuffer", DBG_UI_KEYS.Keys_PlayerAction);
                UI_DebugPlayer.DebugText(4, allowListening, "allowListen", DBG_UI_KEYS.Keys_PlayerAction);
                UI_DebugPlayer.DebugText(5, allowNextAttack, "allowNext", DBG_UI_KEYS.Keys_PlayerAction);
            };
        }

        public override void Enter()
        {
            base.Enter();
            bool mouseMove = true;
            Vector3 direction = mouseMove == true ?
                player.GetPlayerInput.GetMousePositionWorld - playerMovement.transform.position :
                player.GetPlayerInput.GetInputDirectionRawRotated;
            playerRenderer.LookAtDirection(direction);
            playerMovement.UseMouseLock = true;
            Attack();
        }
        public override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Mouse0) && allowListening)
                inputBuffer = true;
            if (inputBuffer && allowNextAttack && IsIndexValid)
                Attack();
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
        protected override void OnAnimationEndTriggerListen() => allowListening = true;
        protected override void OnAnimationEndableTrigger() => allowNextAttack = true;
        protected override void OnForceEventTrigger(float force)
        {
            Vector3 result = comboForceList[currentIdx] * force;
            playerMovement.AddForceFacingDirection(result);
        }
        public override void Exit()
        {
            Debug.Log("exit");
            playerMovement.SpeedMultiplierDefault = 1;
            playerMovement.UseMouseLock = false;
            base.Exit();
        }
    }
}
