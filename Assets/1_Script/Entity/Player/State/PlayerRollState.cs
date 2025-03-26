using System;
using Swift_Blade.Skill;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerRollState : BasePlayerState
    {
        public event Action OnRollEnd;

        protected override bool BaseAllowParryInput => false;
        protected override bool BaseAllowDashInput => false;
        private readonly PlayerRenderer playerRenderer;
        private readonly PlayerHealth playerHealth;
        private bool allowListening;
        private bool inputBuffer;
        private bool allowChangeToAttack;
        private Vector3 movementVectorInterpolated;
        private Quaternion initialQuaternion;
        private Quaternion initialInverseQuaternion;
        public PlayerRollState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            playerRenderer = player.GetPlayerRenderer;
            playerHealth = player.GetPlayerHealth;
            //#if UNITY_EDITOR
            //            Player.Debug_Updt += () =>
            //            {
            //                UI_DebugPlayer.DebugText(3, inputBuffer, "inputBuffer", DBG_UI_KEYS.Keys_PlayerMovement);
            //                UI_DebugPlayer.DebugText(4, allowListening, "allowListen", DBG_UI_KEYS.Keys_PlayerMovement);
            //                UI_DebugPlayer.DebugText(5, allowChangeToAttack, "allowNext", DBG_UI_KEYS.Keys_PlayerMovement);
            //            };
            //#endif
        }
        public override void Enter()
        {
            base.Enter();
            player.GetPlayerRenderer.LookAtDirection(playerInput.GetInputDirectionRawRotated);
            //Vector3 worldEuler = playerRenderer.GetPlayerVisualTrasnform.eulerAngles;
            //worldEuler.x = 0;
            //worldEuler.z = 0;
            movementVectorInterpolated = player.GetPlayerTransform.forward;
            initialInverseQuaternion = player.GetPlayerTransform.rotation;
            initialQuaternion = initialInverseQuaternion;
            initialInverseQuaternion = Quaternion.Inverse(initialInverseQuaternion);

            //UI_DebugPlayer.DebugText(1, movementVectorInterpolated, "bInput", DBG_UI_KEYS.Keys_PlayerMovement);
            //UI_DebugPlayer.DebugText(2, movementVectorInterpolated.magnitude, "bInput2", DBG_UI_KEYS.Keys_PlayerMovement);

            player.ClearComboHistory();
            //anim_inputLocalLerp = Vector3.zero;
            allowListening = false;
            inputBuffer = false;
            allowChangeToAttack = false;
            bool mouseMove = false;
            Vector3 direction = mouseMove == true ?
                player.GetPlayerInput.GetMousePositionWorld - playerMovement.transform.position :
                player.GetPlayerInput.GetInputDirectionRawRotated;
            playerRenderer.LookAtDirection(direction);

            //player.GetPlayerMovement.AllowInputMove = false;
            entity.GetPlayerMovement.Dash(entity.GetPlayerInput.GetInputDirectionRawRotated.normalized, 8.7f);

            playerHealth.IsPlayerInvincible = true;

            player.GetSkillController.UseSkill(SkillType.Rolling);
        }
        public override void Update()
        {
            base.Update();

        }
        protected override void OnApplyMovement(Vector3 resultVector)
        {
            //Vector3 targetVector = resultVector.normalized;
            //Debug.DrawRay(playerMovement.transform.position + Vector3.up * 0.1f, movementVectorInterpolated, Color.red, 0.1f);
            //Debug.DrawRay(playerMovement.transform.position + Vector3.up * 0.2f, targetVector, Color.blue, 0.1f);
            //UI_DebugPlayer.DebugText(3, movementVectorInterpolated, "mvInt", DBG_UI_KEYS.Keys_PlayerMovement);
            //UI_DebugPlayer.DebugText(4, movementVectorInterpolated.magnitude, "length", DBG_UI_KEYS.Keys_PlayerMovement);

            //Transform visualTransform = player.GetPlayerTransform;

            //Vector3 result = visualTransform.InverseTransformDirection(resultVector);

            //initialInverseQuaternion = player.GetPlayerTransform.rotation;
            //initialQuaternion = initialInverseQuaternion;
            //initialInverseQuaternion = Quaternion.Inverse(initialInverseQuaternion);
            Vector3 result2 = initialInverseQuaternion * player.GetPlayerInput.GetInputDirectionRawRotated;
            result2.z = 0;
            Debug.DrawRay(Vector3.zero + Vector3.up * 1.5f, result2, Color.blue);
            result2 = player.GetPlayerTransform.TransformDirection(result2);  
            Debug.DrawRay(Vector3.zero + Vector3.up * 1.3f, result2, Color.red);
            Vector3 targetVector = result2;
            movementVectorInterpolated = Vector3.RotateTowards(movementVectorInterpolated, targetVector, 2f * Time.deltaTime, 6);
            Debug.DrawRay(Vector3.zero + Vector3.up * 1, movementVectorInterpolated, Color.magenta);
            base.OnApplyMovement(Vector3.zero);
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
            //player.GetPlayerMovement.AllowInputMove = true;
            //anim_inputLocalLerp = Vector3.zero;
            //playerHealth.IsPlayerInvincible = false;
            OnRollEnd?.Invoke();
            base.Exit();
        }
    }
}
