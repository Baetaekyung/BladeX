using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade
{
    public class PlayerHealth : MonoBehaviour, IDamageble, IEntityComponent
    {

        [SerializeField] private StatComponent _statCompo;
        [SerializeField] private StatSO _healthStat;

        private float _maxHealth;
        [SerializeField] private float _currentHealth;

        public UnityEvent OnDeadEvent;
        public UnityEvent<ActionData> OnHitEvent;

        private const float DamageInterval = 0.1f;
        private float lastDamageTime;
        
        public bool IsPlayerInvincible { get; set; }
        //private Player _player;
        
        public void EntityComponentAwake(Entity entity)
        {
            //_player = entity as Player;
        }
        
        private void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.P))
            {
                ActionData action = new ActionData(Vector3.zero, 0.5f, 10f, 20f, transform, AttackType.Melee);
                TakeDamage(action);
            }*/
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
