using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class StatComponent : MonoBehaviour
    {
        [SerializeField] protected StatSO[] _stats;
        protected static StatSO[] _statDatas;

        public static Dictionary<ColorType, float> colorModValues = new();

        public static bool InitOnce = false;

        public event Action OnStatChanged;
        
        protected virtual void Initialize()
        {
            if (InitOnce == false)
            {
                StatSO[] tempStatSO = new StatSO[_stats.Length];
                
                for (int i = 0; i < _stats.Length; i++)
                {
                    tempStatSO[i] = _stats[i].Clone();
                    
                    if (tempStatSO[i].statType == StatType.HEALTH)
                        PlayerHealth._currentHealth = tempStatSO[i].Value;
                }

                _stats = tempStatSO;
                _statDatas = tempStatSO;
                InitOnce = true;
                return;
            }
            
            StatSO[] tempStatSo = new StatSO[_statDatas.Length];

            for (int i = 0; i < _statDatas.Length; i++)
            {
                tempStatSo[i] = _statDatas[i];
            }

            _stats = tempStatSo;
        }

        private void OnDisable()
        {
            _statDatas = _stats;
        }

        public StatSO GetStat(StatSO stat)
        {
            StatSO findStat = _stats.FirstOrDefault(x => x.statName == stat.statName);
            Debug.Assert(findStat != null, "stat can't find");

            return findStat;
        }

        public StatSO GetStat(StatType statType)
        {
            StatSO findStat = _stats.FirstOrDefault(x => x.statType == statType);
            Debug.Assert(findStat != null, "stat can't find");
            
            return findStat;
        }

        public void SetColorValue(StatSO stat, int value)
        {
            GetStat(stat).ColorValue = value;
            OnStatChanged?.Invoke();
        }
        
        public float GetColorValue(StatSO stat)
            => GetStat(stat).ColorValue;

        public float IncreaseColorValue(StatSO stat, int value)
        {
            GetStat(stat).ColorValue += value;
            OnStatChanged?.Invoke();
            
            return GetStat(stat).ColorValue;
        }

        public void AddModifier(StatSO stat, object key, float value)
        {
            GetStat(stat).AddModifier(key, value);
            
            OnStatChanged?.Invoke();
        }

        public void AddModifier(StatType statType, object key, float value)
        {
            GetStat(statType).AddModifier(key, value);
            OnStatChanged?.Invoke();
        }

        public void RemoveModifier(StatSO stat, object key)
        {
            GetStat(stat).RemoveModifier(key);
            OnStatChanged?.Invoke();
        }

        public void RemoveModifier(StatType statType, object key)
        {
            GetStat(statType).RemoveModifier(key);
            OnStatChanged?.Invoke();
        }

        public void ClearAllModifiers()
        {
            foreach (StatSO stat in _stats)
            {
                stat.ClearModifier();
            }
        }
    }
}
