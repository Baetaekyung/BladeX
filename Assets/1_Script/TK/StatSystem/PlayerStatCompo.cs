using System;
using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    [Serializable]
    public class ColorStat
    {
        public ColorType colorType;
        public int colorValue;
    }

    public class PlayerStatCompo : StatComponent, IEntityComponent, IEntityComponentStart
    {
        public static List<ColorStat> colorStats = new List<ColorStat>();
        public event  Action ColorValueChangedAction;

        public List<ColorStat> defaultColorStat = new List<ColorStat>();

        private PlayerHealth _playerHealth;

        public void EntityComponentAwake(Entity entity)
        {
            if (IsNewGame == false)
                colorStats = defaultColorStat;

            Initialize();
        }

        public void EntityComponentStart(Entity entity)
        {
            Player player = entity as Player;

            _playerHealth = entity.GetEntityComponent<PlayerHealth>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            UpdateColorValueToStat();
        }

        private void UpdateColorValueToStat()
        {
            foreach (StatSO stat in _defaultStats)
            {
                foreach (ColorStat colorStat in colorStats)
                {
                    if(stat.colorType == colorStat.colorType)
                        stat.ColorValue = colorStat.colorValue;
                }
            }

#if UNITY_EDITOR // For Debuging
            foreach (StatSO stat in _defaultStats)
                stat.dbgValue = stat.Value;
#endif
        }

        public void BuffToStat(StatType statType, string buffKey, float buffTime, float buffAmount
            , Action startCallback = null, Action endCallback = null)
        {
            StatSO stat = GetStat(statType);

            // Effect.., Sound.., etc..
            startCallback?.Invoke(); 

            if (stat.currentBuffDictionary.TryGetValue(buffKey, out var buffRoutine))
                RemoveDuplicatedBuff(buffKey, stat, buffRoutine);

            if (stat.statType == StatType.HEALTH)
                _playerHealth.ShieldAmount += Mathf.RoundToInt(buffAmount);

            // Start coroutine
            Coroutine newBuffRoutine = StartCoroutine(stat.DelayBuffRoutine(buffKey, buffTime, buffAmount));

            stat.currentBuffDictionary.Add(buffKey, newBuffRoutine);
            // register buff end event
            stat.OnBuffEnd += HandleBuffEnd; 

            _playerHealth.HealthUpdate();

            void HandleBuffEnd()
            {
                stat.currentBuffDictionary.Remove(buffKey);
                stat.OnBuffEnd = null;

                //Invoke after buff end
                switch(stat.statType)
                {
                    case StatType.HEALTH:
                        _playerHealth.ShieldAmount -= Mathf.RoundToInt(buffAmount);
                        _playerHealth.HealthUpdate();
                        break;

                    default:
                        break;
                }

                endCallback?.Invoke();
            }
            void RemoveDuplicatedBuff(string buffKey, StatSO stat, Coroutine buffRoutine)
            {
                StopCoroutine(buffRoutine);

                stat.RemoveModifier(buffKey);
                stat.currentBuffDictionary.Remove(buffKey);
                stat.buffTimer = 0;
                stat.OnBuffEnd = null;
            }
        }

        public void IncreaseColorValue(ColorType colorType, int increaseAmount)
        {
            ColorStat colorStat = GetColorStat(colorType);

            //if (colorType == ColorType.GREEN)
            //{
            //    float healthHandler = GetStat(StatType.HEALTH).colorMultiplier * increaseAmount;

            //    PlayerHealth.CurrentHealth++;
            //}

            colorStat.colorValue += increaseAmount;
            ColorValueChange();
        }

        public void DecreaseColorValue(ColorType colorType, int decreaseAmount)
        {
            var colorStat = GetColorStat(colorType);

            colorStat.colorValue -= decreaseAmount;
            ColorValueChange();
        }

        private void ColorValueChange()
        {
            ColorValueChangedAction?.Invoke();

            UpdateColorValueToStat();
            Player.Instance.GetEntityComponent<PlayerHealth>().HealthUpdate();
        }

        public int GetColorStatValue(ColorType colorType) => GetColorStat(colorType).colorValue;
        public ColorStat GetColorStat(ColorType colorType)
        {
            foreach (var stat in colorStats)
            {
                if (stat.colorType == colorType)
                {
                    return stat;
                }
            }

            return default;
        }
    }
}
