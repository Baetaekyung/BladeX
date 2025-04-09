using UnityEngine.Events;
using Swift_Blade.UI;
using UnityEngine;
using System;
using UnityEngine.Serialization;

namespace Swift_Blade.Combat.Health
{
    public class PlayerHealth : BaseEntityHealth, IEntityComponent
        ,IEntityComponentStart
    {
        public event Action<float, float, int> OnHealthUpdateEvent;

        private const float DAMAGE_INTERVAL = 0.75f;
        public static float CurrentHealth;
        public UnityEvent OnHealEvent;
        
        [SerializeField] private StatSO         healthStat;
        [SerializeField] private float          defaultHealth = 4;

        private float _lastDamageTime;
        private int   _shieldAmount;
        
        private Player          _player;
        private PlayerStatCompo statCompo;

        public float GetCurrentHealth => CurrentHealth;

        public int ShieldAmount
        {
            get => _shieldAmount;
            set
            {
                _shieldAmount = Mathf.Max(value, 0);
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
        
        public override void TakeDamage(ActionData actionData)
        {
            if (_lastDamageTime + DAMAGE_INTERVAL > Time.time || isDead || IsPlayerInvincible) return;
                        
            //repac...
            if(ShieldAmount > 0)
            {
                int tempHealth = ShieldAmount - Mathf.RoundToInt(actionData.damageAmount);
                ShieldAmount -= Mathf.RoundToInt(actionData.damageAmount);

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
            
            StatComponent.IsNewGame = false;
            SkillManager.IsNewGame = false;
            InventoryManager.IsNewGame = false;

            CurrentHealth = defaultHealth;
            
            PopupManager.Instance.AllPopDown();
            PopupManager.Instance.PopUp(PopupType.GameOver);
        }

        public bool IsFullHealth => Mathf.Approximately(CurrentHealth , healthStat.Value);
    }
}
