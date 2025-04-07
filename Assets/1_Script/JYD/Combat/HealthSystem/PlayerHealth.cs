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
        public UnityEvent OnHealEvent;
        public event Action<float, float> OnHealthUpdateEvent;
        
        [SerializeField] private StatComponent _statCompo;
        [FormerlySerializedAs("_healthStat")] [SerializeField] private StatSO healthStat;
        
        public float GetCurrentHealth => CurrentHealth;
        public static float CurrentHealth;
        
        public StatSO GetHealthStat => healthStat;
        public bool IsPlayerInvincible { get; set; }
        
        private const float DAMAGE_INTERVAL = 0.75f;
        private float lastDamageTime;
        private Player _player;
        
        public void EntityComponentAwake(Entity entity)
        {
            _player = entity as Player;
        }
        
        public void EntityComponentStart(Entity entity)
        {
            healthStat = _statCompo.GetStat(StatType.HEALTH);
            maxHealth = healthStat.Value;
            
            HealthUpdate();
        }

        public void HealthUpdate()
        {
            maxHealth = healthStat.Value;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
            
            OnHealthUpdateEvent?.Invoke(maxHealth, CurrentHealth);
        }
        
        public override void TakeDamage(ActionData actionData)
        {
            if (lastDamageTime + DAMAGE_INTERVAL > Time.time || isDead || IsPlayerInvincible) return;
                        
            float damageAmount = actionData.damageAmount;
            CurrentHealth -= damageAmount;
            lastDamageTime = Time.time;
                        
            _player.GetSkillController.UseSkill(SkillType.Hit);
            OnHitEvent?.Invoke(actionData);
            
            if (CurrentHealth <= 0)
            {
                Dead();
                _player.GetSkillController.UseSkill(SkillType.Dead);
            }
        }
        
        public void TakeHeal(float healAmount) //힐 받으면 현재 체력에 HealAmount 더한 값으로 변경
        {
            if(Mathf.Approximately(CurrentHealth, healthStat.Value))
                return;
            
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Min(CurrentHealth, healthStat.Value);
            
            OnHealEvent?.Invoke();
            
            HealthUpdate();
        }

        public override void Dead()
        {
            base.Dead();
            
            StatComponent.InitOnce = false;
            CurrentHealth = 4; //기본 체력 4로 하드 코딩 해놓을게
            
            PopupManager.Instance.AllPopDown();
            PopupManager.Instance.PopUp(PopupType.GameOver);
        }

        public bool IsFullHealth => Mathf.Approximately(CurrentHealth , healthStat.Value);
        
    }
}
