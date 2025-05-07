using UnityEngine.Events;
using Swift_Blade.UI;
using UnityEngine;
using System;
using System.Collections;

namespace Swift_Blade.Combat.Health
{
    public class PlayerHealth : BaseEntityHealth, IEntityComponent
        ,IEntityComponentStart
    {
        public event Action<float, float, int> OnHealthUpdateEvent;
        public event Action<int> OnShieldBreakEvent;
        
        private const float DAMAGE_INTERVAL = 0.75f;
        public static float CurrentHealth;
        public UnityEvent OnHealEvent;
        
        
        [SerializeField] private StatSO         healthStat;
        [SerializeField] private float          defaultHealth = 4;
        [SerializeField] private ShieldEffect _shieldEffect;

        private float _lastDamageTime;
        private int   _shieldAmount;
        
        private Player          _player;
        private PlayerStatCompo statCompo;

        public float GetCurrentHealth => CurrentHealth;

        private Rigidbody rigidbody;
        private bool isKnockback;
        private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        
        public int ShieldAmount
        {
            get => _shieldAmount;
            set
            {
                _shieldAmount = Mathf.Max(value, 0);
                _shieldEffect.SetShield(_shieldAmount);
            }
        }
        
        public StatSO GetHealthStat => healthStat;
        public bool IsPlayerInvincible { get; set; }
        
        public void EntityComponentAwake(Entity entity)
        {
            _player = entity as Player;
        }
        
        public void EntityComponentStart(Entity entity)
        {
            statCompo = _player.GetEntityComponent<PlayerStatCompo>();
            rigidbody = _player.GetComponentInChildren<Rigidbody>();
            
            healthStat = statCompo.GetStat(StatType.HEALTH);
            maxHealth = healthStat.Value;
            
            HealthUpdate();
        }

        public void HealthUpdate()
        {
            maxHealth = healthStat.Value;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
            
            OnHealthUpdateEvent?.Invoke(maxHealth, CurrentHealth, ShieldAmount);
        }

        /*private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ActionData actionData = new ActionData();
                actionData.knockbackDirection = -_player.GetPlayerTransform.forward;
                actionData.knockbackForce = 3200;
                
                TakeDamage(actionData);
            }
        }
        */
        
        public override void TakeDamage(ActionData actionData)
        {
            if (_lastDamageTime + DAMAGE_INTERVAL > Time.time || isDead || IsPlayerInvincible) return;
            
            StartKnockback(actionData);
            
            if(ShieldAmount > 0)
            {
                int tempHealth = ShieldAmount - Mathf.RoundToInt(actionData.damageAmount);
                ShieldAmount -= Mathf.RoundToInt(actionData.damageAmount);
                OnShieldBreakEvent?.Invoke(ShieldAmount);
                
                if(tempHealth < 0)
                {
                    CurrentHealth -= tempHealth;
                }

                HitEvent();

                return;
            }

            float damageAmount = actionData.damageAmount;
            CurrentHealth -= damageAmount;
            CurrentHealth = Mathf.Max(CurrentHealth, -0.1f);

            HitEvent();

            //Local
            void HitEvent()
            {
                _player.GetSkillController.UseSkill(SkillType.Hit);
                OnHitEvent?.Invoke(actionData);
                _lastDamageTime = Time.time;

                if (CurrentHealth <= 0)
                {
                    Dead();
                    _player.GetSkillController.UseSkill(SkillType.Dead);
                }
            }
        }
        
        public override void TakeHeal(float healAmount) //힐 받으면 현재 체력에 HealAmount 더한 값으로 변경
        {
            if(Mathf.Approximately(CurrentHealth, healthStat.Value))
                return;
            
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Min(CurrentHealth, maxHealth);
            
            OnHealEvent?.Invoke();
            
            HealthUpdate();
        }

        public override void Dead()
        {
            base.Dead();

            Menu.IsNewGame = true;
            CurrentHealth = defaultHealth;
            
            PopupManager.Instance.AllPopDown();
            PopupManager.Instance.PopUp(PopupType.GameOver);
        }

        public bool IsFullHealth => Mathf.Approximately(CurrentHealth , healthStat.Value);

        #region KnockBack
        private void StartKnockback(ActionData actionData)
        {
            if(actionData.knockbackDirection == default || actionData.knockbackForce == 0 || isKnockback)return;
            
            StartCoroutine(
                Knockback(actionData.knockbackDirection , actionData.knockbackForce));
        }
        
        private IEnumerator Knockback(Vector3 knockbackDirection, float knockbackForce)
        {
            isKnockback = true;
                        
            rigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            
            yield return waitForFixedUpdate;
            
            float timeout = 0.5f; 
            float timer = 0f;
            
            while (rigidbody.linearVelocity.sqrMagnitude > 0.01f && timer < timeout) 
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            
            yield return new WaitForFixedUpdate();
                    
            isKnockback = false;
        }
        
        #endregion
    }
}
