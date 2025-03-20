using Swift_Blade.Combat.Caster;
using System.Collections.Generic;
using Swift_Blade.FSM.States;
using Swift_Blade.Combat;
using Swift_Blade.Audio;
using Swift_Blade.Skill;
using Swift_Blade.FSM;
using UnityEngine;
using DG.Tweening;
using System;

namespace Swift_Blade
{
    public enum PlayerStateEnum
    {
        Move,
        Attack,
        Roll,
        Parry,
        Dead,
        HitStun,
        Interact
    }
    public class Player : Entity
    {
        public static Player Instance { get; private set; }
        public static event Action OnDead;
        public bool IsPlayerDead { get; private set; }
        
        public static event Action Debug_Updt;
        private readonly FiniteStateMachine<PlayerStateEnum> playerStateMachine = new();
        private PlayerAttackState playerAttackState;
        
        public static LevelStat level = new LevelStat();
        
        private readonly RaycastHit[] buffer_overlapSphereResult = new RaycastHit[4];
        
        private IInteractable GetClosestInteractable => interactable != null ? interactable.GetComponent<IInteractable>() : null;
        private GameObject interactable;
        private Tween playerInvincibleTween;
        
        [Header("General")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform mousePosition;
        [SerializeField] private LayerMask lm_interactable;

        [Header("Audio")]
        [SerializeField] private AudioSOCollection audioCollection;

        [Header("EventChannels")]
        [SerializeField] private EquipmentChannelSO onHitChannel;
        
        [Header("Anim_Param")]
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
        [SerializeField] protected List<AttackComboSO> comboList;
        public IReadOnlyList<AttackComboSO> GetComboList => comboList;

        [Header("SceneManager")] 
        [SerializeField] private SceneManagerSO SceneManagerSO;
        
        public void AddCombo(AttackComboSO attackComboSO)
        {
            comboList.Add(attackComboSO);
        }
        public void RemoveCombo(AttackComboSO attackComboSO)
        {
            comboList.Remove(attackComboSO);
        }
        #region PlayerComponentGetter
        public PlayerCamera GetPlayerCamera => GetEntityComponent<PlayerCamera>();
        public PlayerMovement GetPlayerMovement => GetEntityComponent<PlayerMovement>();
        public PlayerInput GetPlayerInput => GetEntityComponent<PlayerInput>();
        public PlayerRenderer GetPlayerRenderer => GetEntityComponent<PlayerRenderer>();
        public PlayerAnimator GetPlayerAnimator => GetEntityComponent<PlayerAnimator>();
        public PlayerDamageCaster GetPlayerDamageCaster => GetEntityComponent<PlayerDamageCaster>();
        public PlayerParryController GetPlayerParryController => GetEntityComponent<PlayerParryController>();
        public PlayerHealth GetPlayerHealth => GetEntityComponent<PlayerHealth>();
        public PlayerSkillController GetSkillController => GetEntityComponent<PlayerSkillController>();
        
        #endregion

        public class LevelStat
        {
            public static event Action<LevelStat> OnLevelUp;
            public int Experience { get; private set; }
            public int Level { get; private set; }
            public int StatPoint { get; set; }

            private SceneManagerSO sceneManager;

            public void Init(SceneManagerSO sceneManagerSo)
            {
                sceneManager = sceneManagerSo;
                sceneManager.LevelClearEvent += OnLevelClear;
            }
            ~LevelStat()
            {
                sceneManager.LevelClearEvent -= OnLevelClear;
            }
            private void OnLevelClear()
            {
                const int maxRequiredExperience = 2;
                Experience++;
                                
                if (Experience >= maxRequiredExperience)
                {
                    Experience = Experience - maxRequiredExperience;
                    Level++;
                    StatPoint += 3;
                    OnLevelUp?.Invoke(this);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (Instance == null)
                Instance = this;
            level.Init(SceneManagerSO);
            Animator playerAnimator = GetPlayerRenderer.GetPlayerAnimator.GetAnimator;

            playerStateMachine.AddState(PlayerStateEnum.Move, new PlayerMoveState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_move));
            playerAttackState = new PlayerAttackState(playerStateMachine, playerAnimator, this, animEndTrigger, null);
            playerStateMachine.AddState(PlayerStateEnum.Attack, playerAttackState);
            PlayerRollState playerRollState = new PlayerRollState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_roll);
            playerStateMachine.AddState(PlayerStateEnum.Roll, playerRollState);
            playerStateMachine.AddState(PlayerStateEnum.Parry, new PlayerParryState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_parry));
            playerStateMachine.AddState(PlayerStateEnum.Dead, new PlayerDeadState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_death));
            playerStateMachine.AddState(PlayerStateEnum.HitStun, new PlayerHitStunState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_hitStun));
            playerStateMachine.AddState(PlayerStateEnum.Interact, new PlayerInteractState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_move));
            playerStateMachine.SetStartState(PlayerStateEnum.Move);
            playerRollState.OnRollEnd +=
                () =>
                {
                    GetPlayerHealth.IsPlayerInvincible = true;
                    StatSO dashInvincibleTimeStat = GetEntityComponent<PlayerStatCompo>().GetStat(StatType.DASH_INVINCIBLE_TIME);
                    float delay = dashInvincibleTimeStat.Value;
                    delay = Mathf.Max(delay, 0.2f);
                    Debug.Log(delay);
                    playerInvincibleTween = DOVirtual.DelayedCall(delay,
                        () =>
                        {
                            GetPlayerHealth.IsPlayerInvincible = false;
                        }, false);
                };

            PlayerHealth playerHealth = GetPlayerHealth;

            playerHealth.OnHitEvent.AddListener(
                (ActionData data) =>
                {
                    if (IsPlayerDead) return;
                    bool isHitStun = data.stun;
                    AudioManager.PlayWithInit(audioCollection.GetRandomAudio, true);
                    if (isHitStun)
                        playerStateMachine.ChangeState(PlayerStateEnum.HitStun);
                });
            playerHealth.OnDeadEvent.AddListener(
                () =>
                {
                    playerStateMachine.ChangeState(PlayerStateEnum.Dead);
                    IsPlayerDead = true;
                });
            playerStateMachine.OnChangeState +=
                (PlayerStateEnum type) =>
                {
                    if (type == PlayerStateEnum.Roll || type == PlayerStateEnum.HitStun)
                    {
                        playerInvincibleTween?.Kill();
                    }
                };
            GetEntityComponent<PlayerDamageCaster>().OnCastDamageEvent.AddListener(
                (action) =>
                {
                    onHitChannel.RaiseEvent(this);
                });
        }
        private void Update()
        {
            playerStateMachine.UpdateState();

            //if (Input.GetKeyDown(KeyCode.Z))
            //    AudioEmitter.Dbg2();
            
            mousePosition.position = GetPlayerInput.GetMousePositionWorld;
            
            UI_DebugPlayer.DebugText(0, GetPlayerHealth.IsPlayerInvincible, "invincible");
            UI_DebugPlayer.DebugText(1, playerStateMachine.CurrentState, "cs");
            UI_DebugPlayer.DebugText(2, GetEntityComponent<PlayerStatCompo>().GetStat(StatType.DAMAGE).Value, "atkBase");
            //UI_DebugPlayer.DebugText(3, GetEntityComponent<PlayerStatCompo>().GetStat(StatType.STYLE_METER_INCREASE_INCREMENT).Value, "dec");

            if (Input.GetKeyDown(KeyCode.E))
            {
                IInteractable interactable = GetClosestInteractable;
                if (interactable != null)
                {
                    if (false && Input.GetKey(KeyCode.LeftShift))
                    {
                        interactable.OnEndCallbackUnsubscribe(OnEndCallback);
                        Debug.Log("unsub");
                    }
                    else
                    {
                        interactable.Interact();
                        interactable.OnEndCallbackSubscribe(OnEndCallback);
                        //playerStateMachine.ChangeState(PlayerStateEnum.Interact);
                    }
                    void OnEndCallback()
                    {
                        interactable.OnEndCallbackUnsubscribe(OnEndCallback);
                        //playerStateMachine.ChangeState(PlayerStateEnum.Move);
                    }
                }
            }

            Debug_Updt?.Invoke();
            if (Input.GetKeyDown(KeyCode.F1))
                UI_DebugPlayer.Instance.ShowDebugUI = !UI_DebugPlayer.Instance.ShowDebugUI;
        }
        private void FixedUpdate()
        {
            const int radius = 2;

            int hitCount = Physics.SphereCastNonAlloc(playerTransform.position, radius, Vector3.up, buffer_overlapSphereResult, 0.1f, lm_interactable);
            UI_DebugPlayer.DebugText(5, hitCount, "length");
            interactable = null;

            if (hitCount > 0)
            {
                float tempHighest = Mathf.Infinity;
                GameObject result = null;

                for (int i = 0; i < hitCount; i++)
                {
                    RaycastHit item = buffer_overlapSphereResult[i];
                    Vector3 distance = item.transform.position - playerTransform.position;
                    Debug.DrawLine(item.transform.position, playerTransform.position, Color.blue);
                    if (tempHighest > distance.sqrMagnitude)
                    {
                        tempHighest = distance.sqrMagnitude;
                        result = item.transform.gameObject;
                    }
                }
                interactable = result;
                Debug.DrawRay(result.transform.position, Vector3.up, Color.red);
            }
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
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(playerTransform.position, 2);
        }
    }
}
