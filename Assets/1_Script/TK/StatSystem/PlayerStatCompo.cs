using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Swift_Blade
{
    public class PlayerStatCompo : StatComponent, IEntityComponent
    {
        [SerializeField] private StyleMeter styleMeter;
        public StyleMeter GetStyleMeter => styleMeter;
        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.K))
        //     {
        //         styleMeter.SuccessHit();
        //         Debug.Log(GetStatValue(StatType.DAMAGE));
        //     }
        // }

        public void EntityComponentAwake(Entity entity)
        {
            Initalize();
        }

        public float GetStatValue(StatType statType)
        {
            StatSO stat = GetStatByType(statType);

            double value = stat.statType != StatType.HEALTH ? stat.Value * styleMeter.statMultiplier : stat.Value;

            return (float)Math.Round(value, 2);
        }

        public float GetStatValue(StatSO statSO)
        {
            StatSO stat = GetStat(statSO);

            double value = stat.statType != StatType.HEALTH ? stat.Value * styleMeter.statMultiplier : stat.Value;

            return (float)Math.Round(value, 2);
        }
    }
}
