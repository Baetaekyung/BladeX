using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        CRITICAL_CHANCE,
        CRITICAL_DAMAGE
    }
    
    [CreateAssetMenu(fileName = "Stat_", menuName = "SO/StatSO")]
    public class StatSO : ScriptableObject
    {
        public event Action OnValueChanged;
        public Action OnBuffEnd;

        public StatType  statType;
        public ColorType colorType;

        [TextArea(4, 5)]
        public string description;
        public string statName;
        public string displayName;
        private int   _colorValue;

        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        [SerializeField] private float _baseValue;

        public float increaseAmount;
        public float colorMultiplier; //if color value == 1, value increase 1 * colorMultiplier
        public float modifiedValue = 0;
        public float dbgValue = 0;

        public float buffTimer = 0f;

        public Dictionary<string, Coroutine> currentBuffDictionary = new Dictionary<string, Coroutine>();
        
        private Dictionary<object, float> modifyValueByKeys = new Dictionary<object, float>();

        public int ColorValue
        {
            get => _colorValue;
            set
            {
                _colorValue = value;

                OnValueChanged?.Invoke();
            }
        }
        
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

        public void AddModifier(object key, float value)
        {
            if (modifyValueByKeys.TryGetValue(key, out var val))
            {
                if(val > value)
                {
                    return;
                }
                else
                {
                    RemoveModifier(key);
                    modifiedValue += value;
                    modifyValueByKeys.Add(key, value);

                    OnValueChanged?.Invoke();
                }

                return;
            }
            
            modifiedValue += value;
            modifyValueByKeys.Add(key, value);
                        
            OnValueChanged?.Invoke();
        }

        public void RemoveModifier(object key)
        {
            if (modifyValueByKeys.TryGetValue(key, out float value))
            {
                modifiedValue -= value; 
                modifyValueByKeys.Remove(key);
                
                OnValueChanged?.Invoke();
            }
        }

        public void ClearModifier()
        {
            modifyValueByKeys.Clear();
            modifiedValue = 0;
        }

        private float GetCalculatedValue(int colorVal)
        {
            if (statType == StatType.HEALTH)
            {
                float amount = colorVal * colorMultiplier;

                return Mathf.FloorToInt(amount) + _baseValue;
            }

            return (colorVal * colorMultiplier) + _baseValue;
        }


        public StatSO Clone()
        {
            StatSO statSo = Instantiate(this);

            Dictionary<object, float> modTemp = new();

            foreach (var mod in modifyValueByKeys)
            {
                var   modeKey   = mod.Key;
                float modeValue = mod.Value;

                modTemp.Add(modeKey, modeValue);
            }

            statSo.modifyValueByKeys = modTemp;
            
            return statSo;
        }

        public IEnumerator DelayBuffRoutine(string buffKey, float buffTime, float buffAmount)
        {
            //todo: Refectoring..
            if (statType == StatType.HEALTH)
            {
                buffTimer = buffTime;
                while (this.buffTimer > 0)
                {
                    this.buffTimer -= Time.deltaTime;
                    yield return null;
                }

                OnBuffEnd?.Invoke();

                yield break;
            }

            AddModifier(buffKey, buffAmount);

            buffTimer = buffTime;
            while(this.buffTimer > 0)
            {
                this.buffTimer -= Time.deltaTime;
                yield return null;
            }

            RemoveModifier(buffKey);

            OnBuffEnd?.Invoke();
        }
    }
}
