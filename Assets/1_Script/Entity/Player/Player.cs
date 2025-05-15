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
using Swift_Blade.Combat.Health;
using UnityEngine.Serialization;
using Swift_Blade.Inputs;

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
        //public static event Action OnDead;
        public bool IsPlayerDead { get; private set; }
        private readonly FiniteStateMachine<PlayerStateEnum> playerStateMachine = new();
        public FiniteStateMachine<PlayerStateEnum> GetStateMachine => playerStateMachine;
        private PlayerAttackState playerAttackState;

        public static LevelStat level = new LevelStat();

        private readonly Collider[] buffer_overlapSphereResult = new Collider[5];

        private IInteractable GetClosestInteractable => lastInteractable != null ? lastInteractable.GetComponent<IInteractable>() : null;

        private GameObject lastInteractable;
        private GameObject lastInteractableOrb;
        private Tween playerInvincibleTween;

        private BaseOrb lastOrb;

        [Header("General")]
        [SerializeField] private Transform visualTransform;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform mousePosition;
        [SerializeField] private LayerMask lm_interactable;

        [FormerlySerializedAs("audioCollection")]
        [Header("Audio")]
        [SerializeField] private AudioCollectionSO audioCollection;

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

        private bool isItemFadeTweening;
        private bool flagInteractable;

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
        public Transform GetPlayerTransform => visualTransform;
        public PlayerStatCompo GetPlayerStat => GetEntityComponent<PlayerStatCompo>();

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
            //~LevelStat()
            //{
            //    sceneManager.LevelClearEvent -= OnLevelClear;
            //}
            private void OnLevelClear()
            {
                const int maxRequiredExperience = 2;
                Experience++;

                if (Experience >= maxRequiredExperience)
                {
                    int m = Experience / maxRequiredExperience;
                    Experience = Experience - m * maxRequiredExperience;
                    Level++;
                    StatPoint += 1;
                    OnLevelUp?.Invoke(this);
                }
            }

        }
        //private Quaternion d;
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
                    GetEntityComponent<PlayerWeaponManager>().SetDefaultWeapon();
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
                    //onHitChannel.RaiseEvent(this);
                });
        }
        protected override void Start()
        {
            base.Start();
            InGameUIManager.Instance.SetInfoBoxAlpha(0, true);
        }
        private void Update()
        {
            playerStateMachine.UpdateState();

            mousePosition.position = GetPlayerInput.GetMousePositionWorld;

            if (lastOrb != null)
            {
                InGameUIManager.Instance.SetInfoBoxPosition(lastOrb.transform.position);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                IInteractable currentInteractable = GetClosestInteractable;
                bool flagHasInteractable = currentInteractable != null;
                if (flagHasInteractable)
                {
                    bool isHurt = currentInteractable.IsHurtWhenInteracting();

                    if (isHurt)
                    {
                        ActionData data = new ActionData();
                        data.damageAmount = 1;
                        data.stun = true;
                        GetPlayerHealth.TakeDamage(data);
                    }

                    currentInteractable.Interact();
                    
                    if (lastInteractable.TryGetComponent(out BaseOrb orb))
                    {
                        lastOrb = orb;
                        InGameUIManager.Instance.SetInfoBoxAlpha(1, true);
                        IPlayerEquipable equipable = orb.GetEquipable;
                        InGameUIManager.Instance.SetInfoBox(equipable);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.F1))
                UI_DebugPlayer.Instance.ShowDebugUI = !UI_DebugPlayer.Instance.ShowDebugUI;
        }
        private void FixedUpdate()
        {
            const float radius = 1.3f;
            Vector3 playerPosition = visualTransform.position;
            int interactableHitCount = Physics.OverlapSphereNonAlloc(playerPosition, radius, buffer_overlapSphereResult, lm_interactable);

            float smallestDistance = Mathf.Infinity;

            GameObject closestInteractableGameObject = null;
            GameObject closestOrbGameObject = null;
            BaseOrb closestOrb = null;

            for (int i = 0; i < interactableHitCount; i++)
            {
                Collider item = buffer_overlapSphereResult[i];
                Vector3 pPos = playerPosition;
                pPos.y = 0;

                Vector3 itemPos = item.transform.position;
                itemPos.y = 0;

                float sqrDistance = (itemPos - pPos).sqrMagnitude;
                Debug.DrawLine(itemPos, playerPosition, Color.blue);
                if (smallestDistance > sqrDistance)
                {
                    smallestDistance = sqrDistance;
                    closestInteractableGameObject = item.gameObject;
                    if (item.TryGetComponent(out BaseOrb outOrb))
                    {
                        closestOrb = outOrb;
                        closestOrbGameObject = closestOrb.gameObject;
                    }
                }
            }

            bool flagIsDifferentInteractable = lastInteractable != closestInteractableGameObject;//different interactable (including nul compare)
            if (flagIsDifferentInteractable)
            {
                bool isUnityObjectDestroyed = lastInteractable == null;
                if (flagInteractable && !isUnityObjectDestroyed)
                {
                    IInteractable interactable = lastInteractable.GetComponent<IInteractable>();
                    Debug.Assert(interactable != null, "interactable is null");
            
                    const int k_defaultLayer = 0;
                    GameObject meshObject = interactable.GetMeshGameObject();
                    if (meshObject != null)
                    {
                        meshObject.layer = k_defaultLayer;
                    }
            
                    flagInteractable = false;
                }
                if (closestInteractableGameObject != null)
                {
                    IInteractable interactable = closestInteractableGameObject.GetComponent<IInteractable>();
                    Debug.Assert(interactable != null, "interactable is null");
            
                    const int k_outlineLayer = 16;
                    GameObject meshObject = interactable.GetMeshGameObject();
                    if (meshObject != null)
                    {
                        meshObject.layer = k_outlineLayer;
                    }
                    flagInteractable = true;
                }
            }

            bool isOtherOrb = closestOrbGameObject != lastInteractableOrb;

            if (closestOrbGameObject != null)
            {
                Debug.DrawRay(closestOrbGameObject.transform.position + Vector3.up, Vector3.up, Color.magenta);
                if (isOtherOrb)
                {
                    lastOrb = closestOrb;
                    InGameUIManager.Instance.SetInfoBoxAlpha(1, false);
                    IPlayerEquipable equipable = closestOrb.GetEquipable;
                    InGameUIManager.Instance.SetInfoBox(equipable);
                    isItemFadeTweening = true;
                }
            }
            else if(isItemFadeTweening)
            {
                isItemFadeTweening = false;
                lastOrb = null;
                InGameUIManager.Instance.SetInfoBoxAlpha(0, false);
            }

            lastInteractable = closestInteractableGameObject;
            lastInteractableOrb = closestOrbGameObject;
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
        private void OnDestroy()
        {
            playerStateMachine.Exit();
        }
    }
}
