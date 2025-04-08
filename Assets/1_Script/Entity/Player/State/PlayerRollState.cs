using System;
using Swift_Blade.Skill;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerRollState : BasePlayerState
    {
        public event Action OnRollEnd;

        protected override bool BaseAllowSpecialInput => false;
        protected override bool BaseAllowDashInput => false;
        private readonly PlayerRenderer playerRenderer;
        private readonly PlayerHealth playerHealth;

        private bool allowListening;
        private bool inputBuffer;
        private bool allowChangeToAttack;
        private bool forceChangeToAttack;

        private Vector3 movementVectorInterpolated;
        private Vector3 targetVectorInterpoated;
        private Quaternion initialQuaternion;
        private Quaternion initialInverseQuaternion;

        private EComboState lastPreviousState;
        private EComboState lastNonImeediateState;

        private float timeEntered = 0;
        public float TimeSinceEntered => Time.time - timeEntered;
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
            timeEntered = Time.time;
            player.GetPlayerRenderer.LookAtDirection(playerInput.GetInputDirectionRawRotated);
            player.GetPlayerMovement.SetAngleMultiplier(PlayerMovement.EAngleMultiplier.Slow);
            //Vector3 worldEuler = playerRenderer.GetPlayerVisualTrasnform.eulerAngles;
            //worldEuler.x = 0;
            //worldEuler.z = 0;
            targetVectorInterpoated = Vector3.zero;
            movementVectorInterpolated = Vector3.zero;//player.GetPlayerTransform.forward;
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
            forceChangeToAttack = false;

            bool mouseMove = false;
            Vector3 direction = mouseMove == true ?
                player.GetPlayerInput.GetMousePositionWorld - playerMovement.transform.position :
                player.GetPlayerInput.GetInputDirectionRawRotated;
            playerRenderer.LookAtDirection(direction);

            //player.GetPlayerMovement.AllowInputMove = false;
            entity.GetPlayerMovement.Dash(entity.GetPlayerInput.GetInputDirectionRawRotated.normalized, 8f);

            playerHealth.IsPlayerInvincible = true;

            player.GetSkillController.UseSkill(SkillType.Rolling);
        }
        protected override void OnApplyMovement(Vector3 resultVector)
        {
            Vector3 localInput = initialInverseQuaternion * resultVector;
            Vector3 inputClamped = localInput;
            inputClamped.z = 0;
            float movementPenalty = Mathf.Abs(inputClamped.x);
            movementPenalty = Mathf.Min(movementPenalty, 1);
            inputClamped.Normalize();

            inputClamped = initialQuaternion * inputClamped;

            // 0 ~ 0.4 (roll anim time)
            float delta = TimeSinceEntered * 3f;
            //delta = Mathf.Min(delta, 1);//doesntmatter
            float multiplier = 1;// Mathf.Lerp(0.1f, 0.09f, delta);

            Vector3 targetVector = inputClamped;
            targetVectorInterpoated = Vector3.MoveTowards(targetVectorInterpoated, targetVector, Time.deltaTime * 0.15f);
            movementVectorInterpolated = Vector3.MoveTowards(movementVectorInterpolated, targetVectorInterpoated, multiplier * Time.deltaTime);
            Vector3 inversedForward = -player.GetPlayerTransform.forward;
            playerMovement.SetAdditionalVelocity(inversedForward * movementPenalty);
            base.OnApplyMovement(movementVectorInterpolated * 0.35f);
        }
        protected override void OnAnimationEndTrigger()
        {
            if(forceChangeToAttack)
            {
                player.Attack(lastPreviousState, lastNonImeediateState);
                return;
            }
            base.OnAnimationEndTrigger();
        }
        protected override void OnAttackInput(EComboState previousState, EComboState nonImeediateState = EComboState.None)
        {
            lastPreviousState = previousState;
            lastNonImeediateState = nonImeediateState;
            if (!allowListening)
            {
                forceChangeToAttack = true;
                return;
            }
            inputBuffer = true;
            if (allowChangeToAttack)
            {
                ChangeToAttack();
            }

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
            if (inputBuffer)
            {
                ChangeToAttack();
            }
        }

        public override void Exit()
        {
            //player.GetPlayerMovement.AllowInputMove = true;
            //anim_inputLocalLerp = Vector3.zero;
            //playerHealth.IsPlayerInvincible = false;
            player.GetPlayerMovement.SetAngleMultiplier(PlayerMovement.EAngleMultiplier.Normal);
            OnRollEnd?.Invoke();
            base.Exit();
        }
    }
}
