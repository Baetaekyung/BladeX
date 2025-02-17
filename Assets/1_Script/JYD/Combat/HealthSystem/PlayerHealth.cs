using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade
{
    public class PlayerHealth : MonoBehaviour, IDamageble, IEntityComponent
        ,IEntityComponentStart
    {

        [SerializeField] private StatComponent _statCompo;
        [SerializeField] private StatSO _healthStat;
        [SerializeField] private float _currentHealth;
        public UnityEvent OnDeadEvent;
        public UnityEvent<ActionData> OnHitEvent;

        public float GetCurrentHealth => _currentHealth;
        public StatSO GetHealthStat => _healthStat;

        private const float DamageInterval = 0.71f;
        private float lastDamageTime;
        private float _maxHealth;
        private bool deadFlag;
        
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
            if (deadFlag) return;
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
            deadFlag = true;
            //Debug.Log("플레이어 죽었슴");
        }

        /// <param name="value">추가할 체력 값</param>
        public void AddBaseHealth(float value) //기본 값 변경이라 키 값이 필요 없음.
        {
            _statCompo.SetBaseValue(_healthStat, _maxHealth + value);
            _maxHealth = _healthStat.Value;
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _healthStat.Value);
        }

        //public PlayerStateEnum GetCurrentState() => _player.GetCurrentState();
    }
}
