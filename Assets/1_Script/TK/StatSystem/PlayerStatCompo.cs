using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Swift_Blade
{
    public class PlayerStatCompo : StatComponent, IEntityComponent
    {
        [SerializeField] private StyleMeter styleMeter;
        //todo why ref here? 어디서 쓰고 있어서 일단 놔둠;;
        public StyleMeter GetStyleMeter => styleMeter;
        private Player _player;
        
        private void Awake()
        {
            styleMeter.PlayerStat = this;
        }
        public void EntityComponentAwake(Entity entity)
        {
            Initialize();

            _player = entity as Player;
        }

        private void Start()
        {
            _player.GetEntityComponent<PlayerHealth>().OnDeadEvent.AddListener(Initialize);
            _player.GetEntityComponent<PlayerHealth>().OnDeadEvent.AddListener(styleMeter.Init);
        }

        private void OnDestroy()
        {
            _player.GetEntityComponent<PlayerHealth>().OnDeadEvent.RemoveListener(Initialize);
            _player.GetEntityComponent<PlayerHealth>().OnDeadEvent.RemoveListener(styleMeter.Init);
        }
    }
}
