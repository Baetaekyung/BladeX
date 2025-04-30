using Swift_Blade.Combat.Caster;
using System;
using UnityEngine;

namespace Swift_Blade
{
    public class KnightTagEffect : TagEffectBase
    {
        private PlayerDamageCaster _playerDmgCaster;
        private PlayerStatCompo    _playerStatCompo;

        [SerializeField] private int minSetEffectPercent;
        [SerializeField] private int middleSetEffectPercent;
        [SerializeField] private int maxSetEffectPercent;

        private int _currentPercent;

        public override void Initialize(Player player)
        {
            base.Initialize(player);

            _playerDmgCaster = player.GetEntityComponent<PlayerDamageCaster>();
            _playerStatCompo = player.GetEntityComponent<PlayerStatCompo>();
        }
        protected override void TagEnableEffect(int tagCount)
        {
            throw new NotImplementedException();
        }
        protected override void TagDisableEffect()
        {
            throw new NotImplementedException();
        }
        //public override void DisableTagEffect(int tagCount)
        //{
        //    if(tagCount < minTagCount)
        //        return;

        //    _playerDmgCaster.OnCastDamageEvent.RemoveListener(HandleBuffShield);
        //}

        //public override void EnableTagEffect(int tagCount)
        //{
        //    if (tagCount < minTagCount)
        //        return;

        //    if (tagCount >= maxTagCount)
        //    {
        //        OnTagEffect(maxSetEffectPercent);
        //    }
        //    else if (tagCount >= middleTagCount)
        //    {
        //        OnTagEffect(middleSetEffectPercent);
        //    }
        //    else if (tagCount >= minTagCount)
        //    {
        //        OnTagEffect(minSetEffectPercent);
        //    }
        //}

        private void OnTagEffect(int percent)
        {
            _currentPercent = percent;

            _playerDmgCaster.OnCastDamageEvent.AddListener(HandleBuffShield);
        }

        //Dont need action data.. hmm...
        private void HandleBuffShield(ActionData actionData)
        {
            //확률에 맞지 않음
            if (UnityEngine.Random.Range(0, 101) > _currentPercent)
                return;

            _playerStatCompo.BuffToStat(StatType.HEALTH, Guid.NewGuid().ToString(), 3f, 1f);
        }
    }
}
