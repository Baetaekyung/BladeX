using System;
using System.Linq;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class StatComponent : MonoBehaviour
    {
        [SerializeField] protected StatSO[] _stats;
        
        protected virtual void Initialize()
        {
            StatSO[] _tempStats = new StatSO[_stats.Length];

            for (int i = 0; i < _stats.Length; i++)
            {
                _tempStats[i] = _stats[i].Clone();
            }

            _stats = _tempStats;
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

        public void SetBaseValue(StatSO stat, float value)
            => GetStat(stat).BaseValue = value;
        
        public float GetBaseValue(StatSO stat)
            => GetStat(stat).BaseValue;
        
        public float IncreaseBaseValue(StatSO stat, float value)
            => GetStat(stat).BaseValue += value;

        public void AddModifier(StatSO stat, object key, float value)
            => GetStat(stat).AddModifier(key, value);

        public void AddModifier(StatType statType, object key, float value)
            => GetStat(statType).AddModifier(key, value);

        public void RemoveModifier(StatSO stat, object key)
            => GetStat(stat).RemoveModifier(key);

        public void RemoveModifier(StatType statType, object key)
            => GetStat(statType).RemoveModifier(key);

        public void ClearAllModifiers()
        {
            foreach (StatSO stat in _stats)
            {
                stat.ClearModifier();
            }
        }
    }
}
