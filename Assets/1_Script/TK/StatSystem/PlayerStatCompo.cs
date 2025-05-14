using System;
using System.Collections;
using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    [Serializable]
    public class ColorStat
    {
        public ColorType colorType;
        public int       colorValue;
    }

    [Serializable]
    public struct DebugStat
    {
        public string name;
        public float value;
        
        public DebugStat(string n, float v)
        {
            name = n;
            value = v;
        }
    }

    public class PlayerStatCompo : StatComponent, IEntityComponent, IEntityComponentStart
    {
        public static List<ColorStat> colorStats = new List<ColorStat>();
        public event  Action ColorValueChangedAction;

        public List<ColorStat> defaultColorStat = new List<ColorStat>();
        [SerializeField] private List<DebugStat> DebugStats = new();

        private PlayerHealth _playerHealth;
        private Action OnDisableEvent;

        public void EntityComponentAwake(Entity entity)
        {
            if (Menu.IsNewGame == true)
                colorStats = defaultColorStat;

            Initialize();
        }

        public void EntityComponentStart(Entity entity)
        {
            Player player = entity as Player;

            _playerHealth = entity.GetEntityComponent<PlayerHealth>();
        }

        private void OnDisable() => OnDisableEvent?.Invoke();

        protected override void Initialize()
        {
            base.Initialize();

            UpdateStat();
        }

        private void UpdateStat()
        {
            foreach (StatSO stat in _defaultStats)
            {
                foreach (ColorStat colorStat in colorStats)
                {
                    if(stat.colorType == colorStat.colorType)
                        GetStat(stat).ColorValue = colorStat.colorValue;
                }
            }

            DebugStats.Clear();
            foreach(StatSO stat in _statDatas)
            {
                DebugStats.Add(new DebugStat(stat.statName, stat.Value));
            }
        }

        public void BuffToStat(StatType statType, string buffKey, float buffTime, float buffAmount
            , Action startCallback = null, Action endCallback = null)
        {
            StatSO stat = GetStat(statType);

            // Effect.., Sound.., etc..
            startCallback?.Invoke();

            IEnumerator handler = stat.DelayBuffRoutine(buffKey, buffTime, buffAmount);

            if (stat.currentBuffDictionary.ContainsKey(buffKey))
                HandleBuffRemove();

            if (stat.statType == StatType.HEALTH)
                _playerHealth.ShieldAmount += Mathf.RoundToInt(buffAmount);

            OnDisableEvent += HandleBuffRemove;

            StartCoroutine(handler);

            stat.currentBuffDictionary.Add(buffKey, handler);
            // register buff end event
            stat.OnBuffEnd += HandleBuffEnd; 

            _playerHealth.HealthUpdate();
            UpdateStat();

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
                UpdateStat();
            }
            void HandleBuffRemove()
            {
                StopCoroutine(handler);

                if (stat.statType == StatType.HEALTH)
                {
                    _playerHealth.ShieldAmount -= Mathf.RoundToInt(buffAmount);
                    _playerHealth.HealthUpdate();
                }

                stat.RemoveModifier(buffKey);
                stat.currentBuffDictionary.Remove(buffKey);
                stat.buffTimer = 0;
                stat.OnBuffEnd = null;
                UpdateStat();
            }
        }

        public void IncreaseColorValue(ColorType colorType, int increaseAmount)
        {
            ColorStat colorStat = GetColorStat(colorType);

            colorStat.colorValue += increaseAmount;
            //Debug.Log($"{colorType}이 {increaseAmount}만큼 상승했습니다.");

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

            UpdateStat();
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

            Debug.Log($"Can't find match stat, colorType: {colorType}");
            return default;
        }
    }
}
