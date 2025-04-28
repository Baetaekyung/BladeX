using Swift_Blade.Combat.Caster;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class BabarianTagEffect : TagEffectBase
    {
        private PlayerDamageCaster _playerDmgCaster;

        [SerializeField, Range(0, 100)] private int minEffectPercent;
        [SerializeField, Range(0, 100)] private int middleEffectPercent;
        [SerializeField, Range(0, 100)] private int maxEffectPercent;

        private int _currentPercent;

        public override void Initialize(Player player)
        {
            base.Initialize(player);

            _playerDmgCaster = player.GetEntityComponent<PlayerDamageCaster>();
        }

        public override void EnableTagEffect(int tagCount)
        {
            if (tagCount < minTagCount)
                return;

            if (tagCount >= maxTagCount)
                OnTagEffect(maxEffectPercent);
            else if (tagCount >= middleTagCount)
                OnTagEffect(middleEffectPercent);
            else if (tagCount >= minTagCount)
                OnTagEffect(minEffectPercent);
        }

        private void OnTagEffect(int percent)
        {
            _currentPercent = percent;
            _playerDmgCaster.OnHitDamagable += HandleStun;
        }

        private void HandleStun(HashSet<IHealth> damagables)
        {
            if (UnityEngine.Random.Range(0, 101) > _currentPercent)
                return;

            foreach(var health in damagables)
            {
                ActionData stunData = new ActionData(
                    new Vector3(10000, 10000, 10000),
                    new Vector3(10000, 10000, 10000),
                    0f, 
                    true);

                health.TakeDamage(stunData);
            }
        }

        public override void DisableTagEffect(int tagCount)
        {
            if (tagCount < minTagCount)
                return;

            _playerDmgCaster.OnHitDamagable -= HandleStun;
        }

        public override bool IsValidToEnable(int tagCount)
        {
            if (tagCount < minTagCount)
                return false;

            return true;
        }
    }
}
