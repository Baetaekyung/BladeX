using Swift_Blade.Combat.Caster;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class BabarianTagEffect : TagEffectBase
    {
        private PlayerTagCompo _tag;
        private PlayerStatCompo statCompo;

        public override void Initialize(Player player)
        {
            base.Initialize(player);
            _tag = _player.GetEntityComponent<PlayerTagCompo>();
            statCompo = player.GetEntityComponent<PlayerStatCompo>();
        }
        protected override void TagEnableEffect(int tagCount)
        {
            _tag.ActiveParticle(EquipmentTag.BARBARIAN, true);

            StatSO stat = statCompo.GetStat(StatType.DAMAGE);
            stat.SetModifier(name, tagCount);
        }
        protected override void TagDisableEffect()
        {
            _tag.ActiveParticle(EquipmentTag.BARBARIAN, false);

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
