using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Swift_Blade
{
    [Serializable]
    public struct ColorStat
    {
        public ColorType colorType;
        public int colorValue;
    }

    public class PlayerStatCompo : StatComponent, IEntityComponent, IEntityComponentStart
    {
        public List<ColorStat> defaultColorStat = new List<ColorStat>();
        public static List<ColorStat> colorStats = new List<ColorStat>();

        private static bool InitOnce = false;

        public void EntityComponentAwake(Entity entity)
        {
            if(InitOnce == false)
            {
                colorStats = defaultColorStat;
            }

            Initialize();
        }

        public void EntityComponentStart(Entity entity)
        {
            
        }

        protected override void Initialize()
        {
            base.Initialize();

            foreach(StatSO stat in _stats)
            {
                foreach(ColorStat colorStat in colorStats)
                {
                    if(ColorUtils.GetCotainColors(colorStat.colorType).Contains(stat.colorType))
                    {
                        //hmm...no way..

                        stat.ColorValue += colorStat.colorValue;
                    }
                }
            }

            foreach(StatSO stat in _stats)
            {
                stat.debugVal = stat.Value;
            }
        }

        public int GetColorStat(ColorType colorType)
        {
            return _stats.FirstOrDefault(stat => stat.colorType == colorType).ColorValue;
        }
    }
}
