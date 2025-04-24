using Swift_Blade.Combat.Caster;
using Swift_Blade.Combat.Health;
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

        public override void DisableTagEffect(int tagCount)
        {
            if(tagCount < minTagCount)
                return;

            if(tagCount >= maxTagCount)
            {
                _playerDmgCaster.OnCastDamageEvent.RemoveListener(HandleBuffShield);
            }
            else if(tagCount >= middleTagCount)
            {
                _playerDmgCaster.OnCastDamageEvent.RemoveListener(HandleBuffShield);
            }
            else if(tagCount >= minTagCount)
            {
                _playerDmgCaster.OnCastDamageEvent.RemoveListener(HandleBuffShield);
            }
        }

        public override void EnableTagEffect(int tagCount)
        {
            if (tagCount < minTagCount)
                return;

            if (tagCount >= maxTagCount)
            {
                _currentPercent = maxSetEffectPercent;

                _playerDmgCaster.OnCastDamageEvent.AddListener(HandleBuffShield);
            }
            else if (tagCount >= middleTagCount)
            {
                _currentPercent = middleSetEffectPercent;

                _playerDmgCaster.OnCastDamageEvent.AddListener(HandleBuffShield);
            }
            else if (tagCount >= minTagCount)
            {
                _currentPercent = minSetEffectPercent;

                _playerDmgCaster.OnCastDamageEvent.AddListener(HandleBuffShield);
            }
        }

        //Dont need action data.. hmm...
        private void HandleBuffShield(ActionData actionData)
        {
            //확률에 맞지 않음
            if (Random.Range(0, 101) > _currentPercent)
                return;

            _playerStatCompo.BuffToStat(StatType.HEALTH, nameof(KnightTagEffect), 3f, 1f);
        }

        public override bool IsValidToEnable(int tagCount)
        {
            if (tagCount >= minTagCount)
                return true;

            return false;
        }
    }
}
