using System;
using System.Collections.Generic;
using System.Linq;

namespace Swift_Blade
{
    [Serializable]
    public struct ColorStat
    {
        public ColorType colorType;
        public int colorValue;
    }

    public class PlayerStatCompo : StatComponent, IEntityComponent
    {
        public List<ColorStat>        defaultColorStat = new List<ColorStat>();
        public static List<ColorStat> colorStats = new List<ColorStat>();

        private static bool InitOnce = false;

        public event Action ColorChangedAction;

        public void EntityComponentAwake(Entity entity)
        {
            if(InitOnce == false)
                colorStats = defaultColorStat;

            Initialize();
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

            //hmm...no way..
            foreach (var stat in _stats)
            {
                foreach (ColorStat colorStat in colorStats)
                {
                    if (ColorUtils.GetCotainColors(colorStat.colorType).Contains(stat.colorType))
                        stat.ColorValue += colorStat.colorValue;
                }
            }

#if UNITY_EDITOR

            foreach(StatSO stat in _stats)
                stat.dbgValue = stat.Value;

#endif
        }

        public void IncreaseColorValue(ColorType colorType, int increaseAmount)
        {
            var colorStat = GetColorStat(colorType);

            colorStat.colorValue += increaseAmount;
            ColorChange();
        }

        public void DecreaseColorValue(ColorType colorType, int decreaseAmount)
        {
            var colorStat = GetColorStat(colorType);

            colorStat.colorValue -= decreaseAmount;
            ColorChange();
        }

        private void ColorChange()
        {
            UpdateColorValueToStat();
            ColorChangedAction?.Invoke();
        }

        public int GetColorStatValue(ColorType colorType) => GetColorStat(colorType).colorValue;
        public ColorStat GetColorStat(ColorType colorType) => colorStats.FirstOrDefault(stat => stat.colorType == colorType);
    }
}
