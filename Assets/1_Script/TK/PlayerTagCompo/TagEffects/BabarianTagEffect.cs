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
            print("do" + tagCount);
            StatSO stat = statCompo.GetStat(StatType.DAMAGE);
            stat.SetModifier(name, tagCount);
        }
        protected override void TagDisableEffect()
        {
            print("no");
            StatSO stat = statCompo.GetStat(StatType.DAMAGE);
            stat.RemoveModifier(name);
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
