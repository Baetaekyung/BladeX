using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Swift_Blade
{
    public class PlayerStatCompo : StatComponent, IEntityComponent
    {
        [SerializeField] private StyleMeter styleMeter;
        //todo why ref here? ��� ���� �־ �ϴ� ����;;
        public StyleMeter GetStyleMeter => styleMeter;

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
