using System;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerRollState : BasePlayerState
    {
        protected override bool BaseAllowParryInput => false;
        protected override bool BaseAllowDashInput => false;
        private readonly PlayerRenderer playerRenderer;
        private bool allowListening;
        private bool inputBuffer;
        private bool allowChangeToAttack;
        public PlayerRollState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            playerRenderer = player.GetPlayerRenderer;
#if UNITY_EDITOR
            Player.Debug_Updt += () =>
            {
                UI_DebugPlayer.DebugText(3, inputBuffer, "inputBuffer", DBG_UI_KEYS.Keys_PlayerMovement);
                UI_DebugPlayer.DebugText(4, allowListening, "allowListen", DBG_UI_KEYS.Keys_PlayerMovement);
                UI_DebugPlayer.DebugText(5, allowChangeToAttack, "allowNext", DBG_UI_KEYS.Keys_PlayerMovement);
            };
#endif
        }
        public override void Enter()
        {
            base.Enter();
            player.ClearComboHistory();
            inputLocalLerp = Vector3.zero;
            allowListening = false;
            inputBuffer = false;
            allowChangeToAttack = false;
            bool mouseMove = false;
            Vector3 direction = mouseMove == true ?
                player.GetPlayerInput.GetMousePositionWorld - playerMovement.transform.position :
                player.GetPlayerInput.GetInputDirectionRawRotated;
            playerRenderer.LookAtDirection(direction);

            player.GetPlayerMovement.AllowInputMove = false;
            entity.GetPlayerMovement.Dash(entity.GetPlayerInput.GetInputDirection.normalized, 10);
        }
        protected override void OnAttackInput(EComboState previousState, EComboState nonImeediateState = EComboState.None)
        {
            if (!allowListening) return;
            inputBuffer = true;
            if (allowChangeToAttack)
                ChangeToAttack();
        }
        private void ChangeToAttack()
        {
            base.OnAttackInput(EComboState.AnyAttack, EComboState.Dash);
        }
        protected override void OnAnimationEndTriggerListen() => allowListening = true;
        protected override void OnAnimationEndTriggerStoplisten() => allowListening = false;
        protected override void OnAnimationEndableTrigger()
        {
            allowChangeToAttack = true;
            if (inputBuffer) ChangeToAttack();
        }

        public override void Exit()
        {
            player.GetPlayerMovement.AllowInputMove = true;
            base.Exit();
        }
    }
}
