using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Swift_Blade
{
    public class PlayerStatCompo : StatComponent, IEntityComponent, IEntityComponentStart
    {
        private Player _player;
        
        public void EntityComponentAwake(Entity entity)
        {
            Initialize();

            _player = entity as Player;
        }

        public void EntityComponentStart(Entity entity)
        {
            _player.GetEntityComponent<PlayerHealth>().OnDeadEvent.AddListener(Initialize);
        }
    }
}
