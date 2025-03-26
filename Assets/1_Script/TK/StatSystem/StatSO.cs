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
        public StatType  statType;
        public ColorType colorType;
        public string    statName;
        [TextArea(4, 5)] public string description;
        public string displayName;
        private int   _colorValue;
        [SerializeField] private float _minValue, _maxValue, _baseValue;
        public float increaseAmount;
        public float colorMultiplier; //if color value == 1, value increase 1 * colorMultiplier

        public float debugVal;
        
        private Dictionary<object, float> _modifyValueByKeys = new Dictionary<object, float>();
        
        public float modifiedValue = 0;

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

        public float Value => Mathf.Clamp((GetCalculatedValue(ColorValue) + modifiedValue), MinValue, MaxValue);
        
        public bool IsMax => Mathf.Approximately(Value, MaxValue);
        public bool IsMin => Mathf.Approximately(Value, MinValue);

        public int ColorValue
        {
            get => _colorValue;
            set => _colorValue = value;
        }

        public void AddModifier(object key, float value)
        {
            if (_modifyValueByKeys.ContainsKey(key)) return;

            modifiedValue += value;
            _modifyValueByKeys.Add(key, value);
        }

        public void RemoveModifier(object key)
        {
            if (_modifyValueByKeys.TryGetValue(key, out float value))
            {
                modifiedValue -= value; 
                _modifyValueByKeys.Remove(key);
            }
        }

        public void ClearModifier()
        {
            _modifyValueByKeys.Clear();
            modifiedValue = 0;
        }

        private float GetCalculatedValue(int colorVal)
        {
            if(statType == StatType.HEALTH)
            {
                float amount = (colorVal * colorMultiplier);

                return (Mathf.RoundToInt(amount / 1) + _baseValue); //(amount / 1) is make 5.38 to 5
            }

            return ((colorVal * colorMultiplier) + _baseValue);
        }


        public StatSO Clone()
        {
            StatSO statSo = Instantiate(this);

            Dictionary<object, float> modTemp = new();
            foreach (var mod in _modifyValueByKeys)
            {
                object modeKey = mod.Key;
                float modeValue = mod.Value;
                modTemp.Add(modeKey,modeValue);
            }

            statSo._modifyValueByKeys = modTemp;
            
            return statSo;
        }
    }
}
