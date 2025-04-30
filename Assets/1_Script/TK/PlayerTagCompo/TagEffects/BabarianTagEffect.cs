using Swift_Blade.Combat.Caster;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class BabarianTagEffect : TagEffectBase
    {
        private PlayerStatCompo statCompo;

        public override void Initialize(Player player)
        {
            base.Initialize(player);
            statCompo = player.GetEntityComponent<PlayerStatCompo>();
        }
        protected override void TagEnableEffect(int tagCount)
        {
            //statCompo.AddModifier(StatType.DAMAGE, name, )
        }
        protected override void TagDisableEffect()
        {

        }
        private void OnTagEffect(int percent)
        {
            //_currentPercent = percent;
            //_playerDmgCaster.OnHitDamagable += HandleStun;
        }
        private void HandleStun(HashSet<IHealth> damagables)
        {
            //if (UnityEngine.Random.Range(0, 101) > _currentPercent)
            //    return;
            //
            //foreach(var health in damagables)
            //{
            //    ActionData stunData = new ActionData(
            //        new Vector3(10000, 10000, 10000),
            //        new Vector3(10000, 10000, 10000),
            //        0f, 
            //        true);
            //
            //    health.TakeDamage(stunData);
            //}
        }
    }
}
