using Swift_Blade.UI;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public List<ColorStat> defaultColorStat  = new List<ColorStat>();
        public static List<ColorStat> colorStats = new List<ColorStat>();

        private PlayerHealth _playerHealth;

        public event Action ColorValueChangedAction;

        public void EntityComponentAwake(Entity entity)
        {
            if(InitOnce == false)
                colorStats = defaultColorStat;

            Initialize();
        }

        public void EntityComponentStart(Entity entity)
        {
            Player player = entity as Player;

            _playerHealth = entity.GetEntityComponent<PlayerHealth>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.B))
            {
                BuffToStat(StatType.HEALTH, "damageBuff", 1f, 1f);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            UpdateColorValueToStat();
        }

        private void UpdateColorValueToStat()
        {
            //Init value
            foreach (var stat in _stats)
                stat.ColorValue = 0;

            //hmm...no way../Max: O(8 or 9 * 7 * 3) = Max: O(168 or 189)
            foreach (var stat in _stats) //Max O(8 ~ 9)
            {
                foreach (ColorStat colorStat in colorStats) //Max O(7)
                {
                    if (ColorUtils.GetCotainColors(colorStat.colorType).Contains(stat.colorType)) //Max O(3)
                        stat.ColorValue += colorStat.colorValue;
                }
            }

#if UNITY_EDITOR

            foreach(StatSO stat in _stats)
                stat.dbgValue = stat.Value;

#endif
        }

        public void BuffToStat(StatType statType, string buffKey, float buffTime, float buffAmount)
        {
            StatSO stat = GetStat(statType);

            if(stat.currentBuffDictionary.TryGetValue(buffKey, out var buffRoutine))
            {
                stat.RemoveModifier(buffKey);
                stat.currentBuffDictionary.Remove(buffKey);
                stat.buffTimer = 0;
                stat.OnBuffEnd -= HandleBuffEnd;
                _playerHealth.HealthUpdate();

                StopCoroutine(buffRoutine);
            }

            stat.OnBuffEnd += HandleBuffEnd;
            buffRoutine = StartCoroutine(stat.DelayBuffRoutine(buffKey, buffTime, buffAmount));
            stat.currentBuffDictionary.Add(buffKey, buffRoutine);

            if (stat.statType == StatType.HEALTH)
            {
                PlayerHealth.CurrentHealth += buffAmount;
                _playerHealth.HealthUpdate();
            }

            void HandleBuffEnd()
            {
                if(stat.statType == StatType.HEALTH)
                {
                    _playerHealth.HealthUpdate();
                }
            }
        }

        public void IncreaseColorValue(ColorType colorType, int increaseAmount)
        {
            ColorStat colorStat = GetColorStat(colorType);

            Debug.Log(colorStat.colorType.ToString());

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
            UpdateColorValueToStat();
            ColorValueChangedAction?.Invoke();
        }

        public int GetColorStatValue(ColorType colorType) => GetColorStat(colorType).colorValue;
        public ColorStat GetColorStat(ColorType colorType)
        {
            foreach (var stat in colorStats)
            {
                if(stat.colorType == colorType)
                {
                    return stat;
                }
            }

            return default;
        }
    }
}
