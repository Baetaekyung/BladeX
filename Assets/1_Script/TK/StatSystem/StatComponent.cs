using System;
using System.Linq;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class StatComponent : MonoBehaviour
    {
        [SerializeField] private StatOverride[] _statOverrides;

        protected StatSO[] _stats;
        
        protected virtual void Initalize()
        {
            _stats = _statOverrides.Select(x => x.CreateStat()).ToArray();
        }

        public StatSO GetStat(StatSO stat)
        {
            StatSO findStat = _stats.FirstOrDefault(x => x.statName == stat.statName);

            return findStat;
        }

        public StatSO GetStatByType(StatType statType)
        {
            StatSO findStat = _stats.FirstOrDefault(x => x.statType == statType);

            return findStat;
        }

        public void SetBaseValue(StatSO stat, float value)
            => GetStat(stat).BaseValue = value;
        
        public float GetBaseValue(StatSO stat)
            => GetStat(stat).BaseValue;
        
        public float IncreaseBaseValue(StatSO stat, float value)
            => GetStat(stat).BaseValue += value;

        public void AddModifier(StatSO stat, object key, float value)
            => GetStat(stat).AddModifier(key, value);

        public void AddModifier(StatType statType, object key, float value)
            => GetStatByType(statType).AddModifier(key, value);

        public void RemoveModifier(StatSO stat, object key)
            => GetStat(stat).RemoveModifier(key);

        public void RemoveModifier(StatType statType, object key)
            => GetStatByType(statType).RemoveModifier(key);

        public void ClearAllModifiers()
        {
            foreach (var stat in _stats)
            {
                stat.ClearModifier();
            }
        }
    }
}
