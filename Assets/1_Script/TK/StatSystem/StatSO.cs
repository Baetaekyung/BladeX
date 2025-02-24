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
        AGILITY,
        DASH_INVINCIBLE_TIME,
        PARRY_CHANCE,
        ADDITINAL_STYLE_METER
    }
    
    [CreateAssetMenu(fileName = "Stat_", menuName = "SO/StatSO")]
    public class StatSO : ScriptableObject
    {
        public delegate void ValueChangeHandler(StatSO stat, float current, float prev);
        public ValueChangeHandler OnValueChange;

        [FormerlySerializedAs("stlyeMeter")] [SerializeField] private StyleMeter styleMeter;

        public StatType statType;
        public string statName;
        [TextArea(4, 5)] public string description;
        public string displayName;
        [SerializeField] private float _baseValue, _minValue, _maxValue;
        
        private readonly Dictionary<object, float> _modifyValueByKeys = new Dictionary<object, float>();
        
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

        public float Value
        {
            get
            {
                bool isStyleMeterTargetStat =
                    styleMeter.TargetStatTypes[Mathf.FloorToInt(styleMeter.appliedMultiplier - 1f)]
                        .targetStats.Contains(statType);

                if (isStyleMeterTargetStat)
                {
                    //현재 스타일 미터 계수에 따라서 적용되는 스텟 다르게 하기
                    return Mathf.Clamp((_baseValue + _modifiedValue) *
                                       styleMeter.appliedMultiplier , MinValue, MaxValue);
                }
                
                return Mathf.Clamp((_baseValue + _modifiedValue), MinValue, MaxValue);
            }
        }
        
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
        
        public StatSO Clone() => Instantiate(this);
    }
}
