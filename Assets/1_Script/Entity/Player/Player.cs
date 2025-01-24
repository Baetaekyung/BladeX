using Swift_Blade.FSM;
using Swift_Blade.FSM.States;
using System;
using System.Collections.Generic;
using Swift_Blade.Combat;
using Swift_Blade.Combat.Caster;
using UnityEngine;

namespace Swift_Blade
{
    public enum PlayerStateEnum
    {
        Move,
        Attack,
        Roll,
        Parry,
        Dead,
        hit,
        HitStun
    }
    public class Player : Entity
    {
        [Header("Debug_Params")]
        [SerializeField] private AnimationTriggers animEndTrigger;
        [SerializeField, Space(10)] private AnimationParameterSO anim_idle;
        [SerializeField] private AnimationParameterSO anim_move;
        [SerializeField] private AnimationParameterSO anim_parry;
        [SerializeField] private AnimationParameterSO anim_roll;
        [SerializeField] private AnimationParameterSO anim_rollAttack;
        [SerializeField] private AnimationParameterSO anim_death;
        [SerializeField] private AnimationParameterSO anim_hitStun;

        [SerializeField] private AnimationParameterSO anim_dbg;

        [Header("Combo")]
        [SerializeField] protected AttackComboSO[] comboList;
        public EComboState[] dbg_comboHistory;
        public IReadOnlyList<AttackComboSO> GetComboList => comboList;

        [Header("Roll")]
        [SerializeField] private float invinciblePeriod;
        public float GetInvinciblePeriod => invinciblePeriod;
        //[SerializeField] private AnimationParameterSO[] comboParamHash;
        //[SerializeField] private Vector3[] comboForceList;
        //[SerializeField] private float[] periods;
        //
        //public IReadOnlyList<AnimationParameterSO> GetComboHashAtk => comboParamHash;
        //public IReadOnlyList<Vector3> GetComboForceList => comboForceList;
        //public IReadOnlyList<float> GetPeriods => periods;
        public bool IsParryState { get; set; }
        public bool IsPlayerDead { get; private set; }
        #region PlayerComponentGetter
        public PlayerCamera GetPlayerCamera => GetEntityComponent<PlayerCamera>();
        public PlayerMovement GetPlayerMovement => GetEntityComponent<PlayerMovement>();
        public PlayerInput GetPlayerInput => GetEntityComponent<PlayerInput>();
        public PlayerRenderer GetPlayerRenderer => GetEntityComponent<PlayerRenderer>();
        public PlayerAnimator GetPlayerAnimator => GetEntityComponent<PlayerAnimator>();
        public PlayerDamageCaster GetPlayerDamageCaster => GetEntityComponent<PlayerDamageCaster>();
        public PlayerParryController GetPlayerParryController => GetEntityComponent<PlayerParryController>();
        public PlayerHealth GetPlayerHealth => GetEntityComponent<PlayerHealth>();
        #endregion

        
        public static event Action Debug_Updt;
        private readonly FiniteStateMachine<PlayerStateEnum> playerStateMachine = new();
        private PlayerAttackState playerAttackState;
        protected override void Awake()
        {
            base.Awake();
            Animator playerAnimator = GetPlayerRenderer.GetPlayerAnimator.GetAnimator;
            playerStateMachine.AddState(PlayerStateEnum.Move, new PlayerMoveState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_move));
            playerAttackState = new PlayerAttackState(playerStateMachine, playerAnimator, this, animEndTrigger, null);
            playerStateMachine.AddState(PlayerStateEnum.Attack, playerAttackState);
            playerStateMachine.AddState(PlayerStateEnum.Roll, new PlayerRollState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_roll));
            playerStateMachine.AddState(PlayerStateEnum.Parry, new PlayerParryState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_parry));
            PlayerDeadState playerDeadState = new PlayerDeadState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_death);
            playerStateMachine.AddState(PlayerStateEnum.Dead, playerDeadState);
            playerStateMachine.AddState(PlayerStateEnum.HitStun, new PlayerHitStunState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_hitStun));
            playerStateMachine.SetStartState(PlayerStateEnum.Move);
            playerDeadState.OnPlayerDead += () =>
            {
                IsPlayerDead = true;
            };
            PlayerHealth playerHealth = GetPlayerHealth;
            playerHealth.OnHitEvent.AddListener((data) => 
            {
                if (IsPlayerDead || GetPlayerHealth.IsPlayerInvincible) return;
                bool isHitStun = true;
                if (isHitStun)
                    playerStateMachine.ChangeState(PlayerStateEnum.HitStun);
            });
            playerHealth.OnDeadEvent.AddListener(() => { playerStateMachine.ChangeState(PlayerStateEnum.Dead); });
        }
        private void Update()
        {
            playerStateMachine.UpdateState();

            Debug_Updt?.Invoke();
            if (Input.GetKeyDown(KeyCode.F1))
                UI_DebugPlayer.Instance.ShowDebugUI = !UI_DebugPlayer.Instance.ShowDebugUI;
            UI_DebugPlayer.DebugText(0, playerStateMachine.CurrentState.ToString(), "cs", DBG_UI_KEYS.Keys_PlayerAction);
            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    GetPlayerAnimator.GetAnimator.Rebind();
            //    GetPlayerAnimator.GetAnimator.Play(anim_death.GetAnimationHash, -1);
            //}
        }
        public void Attack(EComboState previousState, EComboState nonImmediateState = EComboState.None)
        {
            playerAttackState.PreviousComboState = previousState;
            playerAttackState.NonImmediateComboState = nonImmediateState;
            playerStateMachine.ChangeState(PlayerStateEnum.Attack);
        }
        public void ClearComboHistory()
        {
            playerAttackState.ClearComboHistory();
        }


        public PlayerStateEnum GetCurrentState() => playerStateMachine.GetState();
        
    }
}
