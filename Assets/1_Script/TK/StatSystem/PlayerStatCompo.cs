using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Swift_Blade
{
    public class PlayerStatCompo : StatComponent, IEntityComponent
    {
        [SerializeField] private StyleMeter styleMeter;
        //todo why ref here?
        public StyleMeter GetStyleMeter => styleMeter;
        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.K))
        //     {
        //         styleMeter.SuccessHit();
        //         Debug.Log(GetStatValue(StatType.DAMAGE));
        //     }
        // }

        [ContextMenu("Init")]
        public void TestInit()
        {
            Initialize();
        }
        
        public void EntityComponentAwake(Entity entity)
        {
            Initialize();

            styleMeter.PlayerStat = this;
        }
    }
}
