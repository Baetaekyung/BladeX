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
        
        private float _maxHealth;
        public float GetMaxHealth => _maxHealth;
        [SerializeField] private float _currentHealth;
        public UnityEvent OnDeadEvent;
        public UnityEvent<ActionData> OnHitEvent;
        public event Action<float, float> OnHealthUpdateEvent;

        public float GetCurrentHealth => _currentHealth;
        public StatSO GetHealthStat => _healthStat;
        
        private const float DamageInterval = 0.72f;
        private float lastDamageTime;
        //private float _maxHealth;
        private bool isDead;
        
        public bool IsPlayerInvincible { get; set; }
        //private Player _player;
        
        public void EntityComponentAwake(Entity entity) { }
        
        public void EntityComponentStart(Entity entity)
        {
            _healthStat = _statCompo.GetStat(StatType.HEALTH);
            _maxHealth = _healthStat.Value;
            _currentHealth = _healthStat.Value;
        }

        public void HealthUpdate()
        {
            _maxHealth = _healthStat.Value;
            
            OnHealthUpdateEvent?.Invoke(_maxHealth, _currentHealth);
        }

        public void TakeDamage(ActionData actionData)
        {
            if (isDead) return;
            if (lastDamageTime + DamageInterval > Time.time) return;
            if (IsPlayerInvincible) return;
            
            float damageAmount = actionData.damageAmount;
            _currentHealth -= damageAmount;

            lastDamageTime = Time.time;
            
            OnHitEvent?.Invoke(actionData);

            if (_currentHealth <= 0)
            {
                Dead();
            }
        }
        
        public void TakeHeal(float healAmount) //힐 받으면 현재 체력에 HealAmount 더한 값으로 변경
        {
            _currentHealth += healAmount;
            _currentHealth = Mathf.Clamp(_currentHealth, _healthStat.MinValue, _healthStat.MaxValue);
            
            HealthUpdate();
        }

        public void Dead()
        {
            OnDeadEvent?.Invoke();
            isDead = true;
            
            PopupManager.Instance.AllPopDown();
            PopupManager.Instance.PopUp(PopupType.GameOver);
        }
    }
}
