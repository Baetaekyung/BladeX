using Swift_Blade.FSM;
using Swift_Blade.FSM.States;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public enum PlayerStateEnum
    {
        Idle,
        Attack,
        Dash,
        Parry
    }
    public class Player : Entity
    {
        [Header("Debug Fields")]
        [SerializeField] private float debug_cameraDistance;

        [Header("Params")]
        [SerializeField] private AnimationParameterSO anim_idle;
        [SerializeField] private AnimationParameterSO anim_jumpAttack;
        [SerializeField] private AnimationParameterSO anim_attack1;
        [SerializeField] private AnimationParameterSO anim_parry;

        [Header("General")]
        [SerializeField] private AnimationTriggers animEndTrigger;

        [Header("Combo")]
        //should be serializeable struct later
        [SerializeField] private AnimationParameterSO[] comboParamHash;
        [SerializeField] private Vector3[] comboForceList;
        [SerializeField] private float[] periods;
        public IReadOnlyList<AnimationParameterSO> GetComboHashAtk => comboParamHash;
        public IReadOnlyList<Vector3> GetComboForceList => comboForceList;
        public IReadOnlyList<float> GetPeriods => periods;

        private readonly FiniteStateMachine<PlayerStateEnum> playerStateMachine = new();
        public PlayerCamera GetPlayerCamera => GetEntityComponent<PlayerCamera>();
        public PlayerMovement GetPlayerMovement => GetEntityComponent<PlayerMovement>();
        public PlayerInput GetPlayerInput => GetEntityComponent<PlayerInput>();
        public PlayerRenderer GetPlayerRenderer => GetEntityComponent<PlayerRenderer>();
        public static event Action Updt;
        protected override void Awake()
        {
            base.Awake();
            Animator playerAnimator = GetPlayerRenderer.GetPlayerAnimator;
            playerStateMachine.AddState(PlayerStateEnum.Idle, new PlayerIdleState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_idle));
            playerStateMachine.AddState(PlayerStateEnum.Attack, new PlayerAttackState(playerStateMachine, playerAnimator, this, animEndTrigger, null));
            playerStateMachine.AddState(PlayerStateEnum.Dash, new PlayerDashState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_idle));
            playerStateMachine.AddState(PlayerStateEnum.Parry, new PlayerParryState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_parry));
            playerStateMachine.SetStartState(PlayerStateEnum.Idle);
        }
        private void Update()
        {
            playerStateMachine.UpdateState();
            Updt?.Invoke();
            //debug input
            //if(Input.GetKeyDown(KeyCode.P))
            //    GetPlayerCamera.CameraTargetDistance = debug_cameraDistance;

            void ProcessInput()
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    playerStateMachine.ChangeState(PlayerStateEnum.Dash);

                Vector3 input = GetPlayerInput.InputDirectionRaw.normalized;
                GetPlayerMovement.InputDirection = input;

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

    }
}
