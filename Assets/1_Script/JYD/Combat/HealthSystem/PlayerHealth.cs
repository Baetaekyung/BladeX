using System;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade
{
    public class PlayerHealth : MonoBehaviour, IDamageble, IEntityComponent
        ,IEntityComponentStart
    {
        [SerializeField] private StatComponent _statCompo;
        [SerializeField] private StatSO _healthStat;
        
        public static float CurrentHealth;
        
        public UnityEvent OnDeadEvent;
        public UnityEvent<ActionData> OnHitEvent;
        public UnityEvent OnHealEvent;
        
        public event Action<float, float> OnHealthUpdateEvent;
        
        public float GetCurrentHealth => CurrentHealth;
        public StatSO GetHealthStat => _healthStat;
        
        private const float DamageInterval = 0.75f;
        private float lastDamageTime;
        //private float _maxHealth;
        private bool isDead;
        private float _maxHealth;
        
        public bool IsPlayerInvincible { get; set; }
        private Player _player;
        
        
        public void EntityComponentAwake(Entity entity)
        {
            _player = entity as Player;
        }
        
        public void EntityComponentStart(Entity entity)
        {
            _healthStat = _statCompo.GetStat(StatType.HEALTH);
            _maxHealth = _healthStat.Value;
            
            HealthUpdate();
        }

        public void HealthUpdate()
        {
            _maxHealth = _healthStat.Value;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, _maxHealth);
            
            OnHealthUpdateEvent?.Invoke(_maxHealth, CurrentHealth);
        }

        public void TakeDamage(ActionData actionData)
        {
            if (isDead) return;
            if (lastDamageTime + DamageInterval > Time.time) return;
            if (IsPlayerInvincible) return;
            
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
            if(Mathf.Approximately(CurrentHealth, _healthStat.Value))
                return;
            
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Min(CurrentHealth, _healthStat.Value);
            
            OnHealEvent?.Invoke();
            
            HealthUpdate();
        }

        public void Dead()
        {
            OnDeadEvent?.Invoke();
            isDead = true;

            StatComponent.InitOnce = false;
            CurrentHealth = 4; //기본 체력 4로 하드 코딩 해놓을게
            
            PopupManager.Instance.AllPopDown();
            PopupManager.Instance.PopUp(PopupType.GameOver);
        }
        
        public bool IsFullHealth => CurrentHealth == _healthStat.Value;
        
    }
}
