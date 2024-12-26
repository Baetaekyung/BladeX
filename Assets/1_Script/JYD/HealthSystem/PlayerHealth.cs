using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerHealth : MonoBehaviour,IDamageble, IEntityComponentRequireInit
    {
        private Player _player;
        [SerializeField] private StatComponent _statCompo;
        [SerializeField] private StatSO _healthStat;

        private float _maxHealth;
        private float _currentHealth;

        public event Action OnDeadEvent;
        public event Action<ActionData> OnHitByVicinityEvent; //근접 공격으로 맞았을 때
        public event Action<ActionData> OnHitByRangeEvent; //원거리 공격으로 맞았을 때

        private bool _afterInitialize = false;
        
        public void EntityComponentAwake(Entity entity)
        {
            _player = entity as Player;
            
            OnDeadEvent += Dead;
        }

        private void OnDestroy()
        {
            OnDeadEvent -= Dead;
        }

        public void TakeDamage(ActionData actionData)
        {
            if (!_afterInitialize)
            {
                _maxHealth = _statCompo.GetStat(_healthStat).Value;
                _currentHealth = _maxHealth;
            
                Debug.Log($"Health Init to : {_currentHealth}");

                _afterInitialize = true;
            }
            
            float damageAmount = actionData.damageAmount;
            _currentHealth -= damageAmount;

            switch (actionData.attackType)
            {
                case AttackType.VICINITY:
                    OnHitByVicinityEvent?.Invoke(actionData);
                    break;
                case AttackType.RANGE:
                    OnHitByRangeEvent?.Invoke(actionData);
                    break;
                default:
                    OnHitByVicinityEvent?.Invoke(actionData);
                    break;
            }

            if (_currentHealth <= 0)
                Dead();
        }

        public void TakeHeal()
        {
            //이거 매개변수 없으면 안되는거 아닌가?
        }

        public void TakeHeal(float healAmount) //힐 받으면 현재 체력에 HealAmount 더한 값으로 변경
        {
            _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _maxHealth);
        }

        public void Dead()
        {
            //OnDeadEvent?.Invoke();
            Debug.Log("플레이어 죽었슴");
        }

        /// <param name="value">추가할 체력 값</param>
        public void AddBaseHealth(float value) //기본 값 변경이라 키 값이 필요 없음.
        {
            _statCompo.SetBaseValue(_healthStat, _maxHealth + value);
            _maxHealth = _healthStat.Value;
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _healthStat.Value);
        }
    }
}
