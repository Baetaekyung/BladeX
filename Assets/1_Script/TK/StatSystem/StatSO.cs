using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public enum StatType
    {
        HEALTH,
        DAMAGE,
        MINATTACK_INC, //최소 공격력 보정
        ATTACKSPEED,
        MOVESPEED,
        DASH_INVINCIBLE_TIME,
        PARRY_CHANCE,
    }
    
    [CreateAssetMenu(fileName = "Stat_", menuName = "SO/StatSO")]
    public class StatSO : ScriptableObject
    {
        public delegate void ValueChangeHandler(StatSO stat, float current, float prev);
        public ValueChangeHandler OnValueChange;
        
        public StatType statType;
        public string statName;
        [TextArea(4, 5)] public string description;
        public string displayName;
        [SerializeField] private float _baseValue, _minValue, _maxValue;
        public float increaseAmount;
        
        private Dictionary<object, float> _modifyValueByKeys = new Dictionary<object, float>();
        
        public float _modifiedValue = 0;

        public float MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        public float MinValue
        {
            get => _minValue;
            set => _minValue = value;
        }

        public float Value => Mathf.Clamp((_baseValue + _modifiedValue), MinValue, MaxValue);
        
        public bool IsMax => Mathf.Approximately(Value, MaxValue);
        public bool IsMin => Mathf.Approximately(Value, MinValue);

        public float BaseValue
        {
            get => _baseValue;
            set => _baseValue = Mathf.Clamp(value, MinValue, MaxValue);
        }

        public void AddModifier(object key, float value)
        {
            if (_modifyValueByKeys.ContainsKey(key)) return;

            _modifiedValue += value;
            _modifyValueByKeys.Add(key, value);
        }

        public void RemoveModifier(object key)
        {
            if (_modifyValueByKeys.TryGetValue(key, out float value))
            {
                _modifiedValue -= value; 
                _modifyValueByKeys.Remove(key);
            }
        }

        public void ClearModifier()
        {
            _modifyValueByKeys.Clear();
            _modifiedValue = 0;
        }

        public StatSO Clone()
        {
            StatSO statSo = Instantiate(this);

            return statSo;
        }
    }
}
