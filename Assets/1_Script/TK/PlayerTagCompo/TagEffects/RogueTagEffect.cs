using Swift_Blade.Combat.Caster;
using System;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    public class RogueTagEffect : TagEffectBase
    {
        private PlayerStatCompo _stat;
        
        public override void Initialize(Player player)
        {
            base.Initialize(player);
            
            _stat = _player.GetEntityComponent<PlayerStatCompo>();
        }

        protected override void TagEnableEffect(int tagCount)
        {
            
        }

        protected override void TagDisableEffect()
        {
            
        }
    }
}
