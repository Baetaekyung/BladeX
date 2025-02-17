using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade
{
    public class PlayerHealth : MonoBehaviour, IDamageble, IEntityComponent
        ,IEntityComponentStart
    {
        [SerializeField] private StatComponent _statCompo;

        private float _maxHealth;
        public float GetMaxHealth => _maxHealth;
        [SerializeField] private float _currentHealth;
        public float GetCurrentHealth => _currentHealth;

        public UnityEvent OnDeadEvent;
        public UnityEvent<ActionData> OnHitEvent;

        private const float DamageInterval = 0.71f;
        private float lastDamageTime;
        
        public bool IsPlayerInvincible { get; set; }
        //private Player _player;
        
        public void EntityComponentAwake(Entity entity) { }
        
        public void EntityComponentStart(Entity entity)
        {
            _maxHealth = _statCompo.GetStatByType(StatType.HEALTH).Value;
            _currentHealth = _statCompo.GetStatByType(StatType.HEALTH).Value;
        }

        public void TakeDamage(ActionData actionData)
        {
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
            _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _maxHealth);
        }

        public void Dead()
        {
            OnDeadEvent?.Invoke();
            //Debug.Log("플레이어 죽었슴");
        }
    }
}
