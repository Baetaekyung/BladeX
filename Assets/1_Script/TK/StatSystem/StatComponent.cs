using System;
using System.Linq;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class StatComponent : MonoBehaviour
    {
        [SerializeField] protected StatSO[] _stats;
        public static StatSO[] myStats;
        public static bool InitOnce = false;
        
        protected virtual void Initialize()
        {
            if (InitOnce == false)
            {
                myStats = new StatSO[_stats.Length];
                
                for (int i = 0; i < _stats.Length; i++)
                {
                    myStats[i] = _stats[i].Clone();

                    //no more thing...
                    if (myStats[i].statType == StatType.HEALTH)
                        PlayerHealth._currentHealth = myStats[i].Value;
                }

                InitOnce = true;
            }
        }

        public StatSO GetStat(StatSO stat)
        {
            StatSO findStat = myStats.FirstOrDefault(x => x.statName == stat.statName);
            Debug.Assert(findStat != null, "stat can't find");

            return findStat;
        }

        public StatSO GetStat(StatType statType)
        {
            StatSO findStat = myStats.FirstOrDefault(x => x.statType == statType);
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
            foreach (StatSO stat in myStats)
            {
                stat.ClearModifier();
            }
        }
    }
}
