using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    public enum StyleMeterState 
    {
        First, //�̸� ���� �ؾ��� �� �𸣰���;;
        Second,
        Third
    }
    
    [Serializable]
    public struct TargetStatTypes
    {
        public List<StatType> targetStats;
    }
    
    [CreateAssetMenu(fileName = "StyleMeter", menuName = "SO/StyleMeter")]
    public class StyleMeter : ScriptableObject
    {
        public event Action OnSuccessHitEvent;
        public event Action OnDamagedEvent;

        [SerializeField] private float maxMultiplier;
        
        public float appliedMultiplier;
        private const float StatMultiplier = 1f;
        private float _addedMultiplier = 0f; //���� ������ �÷��ִ� multiplier 
        private StyleMeterState _styleMeterState = StyleMeterState.First;
        
        [SerializeField] private SerializableDictionary<StyleMeterState,TargetStatTypes> targetStatTypes = new();
        public PlayerStatCompo PlayerStat;

        private void OnEnable()
        {
            appliedMultiplier = 1f;
            _addedMultiplier = 0f;
        }

        public void SuccessHit()
        {
            const float initialValue = 0.1f;
            float amount = initialValue + PlayerStat.GetStat(StatType.STYLE_METER_INCREASE_INCREMENT).BaseValue;
            IncreaseMultiplier(amount); //Stat���� ������ �޾Ƽ� Increase��Ű��
            
            OnSuccessHitEvent?.Invoke();
        }

        public void TakeDamage()
        {
            const float initialValue = 0.1f;
            float amount = initialValue + PlayerStat.GetStat(StatType.STYLE_METER_DECREASE_DECREMENT).BaseValue;
            DecreaseMultiplier(amount); //Stat���� ���ҷ� �޾Ƽ� Decrease��Ű��

            OnDamagedEvent?.Invoke();
        }

        public List<StatType> GetTargetStats()
        {
            return targetStatTypes[_styleMeterState].targetStats;
        }
        
        private void IncreaseMultiplier(float increaseAmount)
        {
            _addedMultiplier += increaseAmount;
            ApplyMultiplier();
        }

        private void DecreaseMultiplier(float decreaseAmount)
        {
            _addedMultiplier -= decreaseAmount;
            ApplyMultiplier();
        }
        
        private void ApplyMultiplier()
        {
            _addedMultiplier = Mathf.Clamp(_addedMultiplier, 0f, maxMultiplier);
            appliedMultiplier = Mathf.Clamp(StatMultiplier + _addedMultiplier, 1f, maxMultiplier + 1f);

            _styleMeterState = _addedMultiplier switch
            {
                < 1f => StyleMeterState.First,
                >= 1f and < 2f => StyleMeterState.Second,
                >= 2f => StyleMeterState.Third,
                _ => _styleMeterState
            };
        }
    }
}
