using Swift_Blade.FSM;
using Swift_Blade.FSM.States;
using UnityEngine;

namespace Swift_Blade
{
    public enum PlayerStateEnum
    {
        Idle,
        Attack
    }
    public class Player : Entity
    {
        [Header("Debug Fields")]
        [SerializeField] private float debug_cameraDistance;

        [Header("Params")]
        [SerializeField] private AnimationParameterSO anim_idle;
        [SerializeField] private AnimationParameterSO anim_jumpAttack;
        [SerializeField] private AnimationParameterSO anim_attack1;
        [SerializeField] private AnimationParameterSO anim_attack2;
        [SerializeField] private AnimationParameterSO anim_attack3;

        [Header("General")]
        [SerializeField] private AnimationTriggers animEndTrigger;

        private readonly FiniteStateMachine<PlayerStateEnum> playerStateMachine = new();
        public PlayerCamera GetPlayerCamera => GetEntityComponent<PlayerCamera>();
        public PlayerMovement GetPlayerMovement => GetEntityComponent<PlayerMovement>();
        public PlayerInput GetPlayerInput => GetEntityComponent<PlayerInput>();
        public PlayerRenderer GetPlayerRenderer => GetEntityComponent<PlayerRenderer>();
        protected override void Awake()
        {
            base.Awake();
            PlayerInput.OnParryInput += HandleOnParryInput;
            Animator playerAnimator = GetPlayerRenderer.GetPlayerAnimator;
            playerStateMachine.AddState(PlayerStateEnum.Idle, new PlayerIdleState(playerStateMachine, playerAnimator, animEndTrigger, anim_idle));
            playerStateMachine.AddState(PlayerStateEnum.Attack, new PlayerAttackState(playerStateMachine, playerAnimator, animEndTrigger, anim_attack1));
            playerStateMachine.SetStartState(PlayerStateEnum.Idle);

        }
        private void Update()
        {
            playerStateMachine.UpdateState();
            //debug input
            //if(Input.GetKeyDown(KeyCode.P))
            //    GetPlayerCamera.CameraTargetDistance = debug_cameraDistance;

            void ProcessInput()
            {
                Vector3 input = GetPlayerInput.InputDirectionRaw.normalized;
                GetPlayerMovement.InputDirection = input;

                Transform playerVisualTransform = GetPlayerRenderer.GetPlayerVisualTrasnform;
                if (input.sqrMagnitude > 0)
                {
                    Quaternion visLookDirResult = Quaternion.LookRotation(input, Vector3.up);
                    float angle = Vector3.Angle(input, playerVisualTransform.forward);
                    const float angleMultiplier = 20;
                    float maxDegreesDelta = Time.deltaTime * angle * angleMultiplier;
                    visLookDirResult = Quaternion.RotateTowards(playerVisualTransform.rotation, visLookDirResult, maxDegreesDelta);
                    playerVisualTransform.rotation = visLookDirResult;
                }
                UpdateDebugUI();
                void UpdateDebugUI()
                {
                    if (Input.GetKeyDown(KeyCode.F1))
                        UI_DebugPlayer.Instance.ShowDebugUI = !UI_DebugPlayer.Instance.ShowDebugUI;
                    UI_DebugPlayer.Instance.GetList[0].text = $"Current State {playerStateMachine.CurrentState}";
                }
            }
            ProcessInput();
        }
        private void HandleOnParryInput()
        {

        }

    }
}
