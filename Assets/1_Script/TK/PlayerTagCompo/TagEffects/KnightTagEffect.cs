using UnityEngine;

namespace Swift_Blade
{
    public class KnightTagEffect : TagEffectBase
    {
        [SerializeField] private float _healthModifier = 2f;
        [SerializeField] private float _moveSpeedModifier = -1f;
        [SerializeField] private float _attackSpeedModifier = -0.2f;

        private PlayerStatCompo _stat;

        public override void Initialize(Player player)
        {
            base.Initialize(player);

            _stat = _player.GetEntityComponent<PlayerStatCompo>();
        }

        protected override void TagEnableEffect(int tagCount)
        {
            _stat.AddModifier(StatType.HEALTH, this, _healthModifier);
            _stat.AddModifier(StatType.MOVESPEED, this, _moveSpeedModifier);
            _stat.AddModifier(StatType.ATTACKSPEED, this, _attackSpeedModifier);
        }
        
        protected override void TagDisableEffect()
        {
            _stat.RemoveModifier(StatType.HEALTH, this);
            _stat.RemoveModifier(StatType.MOVESPEED, this);
            _stat.RemoveModifier(StatType.ATTACKSPEED, this);
        }
    }
}
