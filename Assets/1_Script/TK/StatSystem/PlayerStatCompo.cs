using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Swift_Blade
{
    public class PlayerStatCompo : StatComponent, IEntityComponent, IEntityComponentStart
    {
        public void EntityComponentAwake(Entity entity)
        {
            Initialize();
        }

        public void EntityComponentStart(Entity entity)
        {
            
        }
    }
}
