using UnityEngine;

namespace Swift_Blade
{
    public class MutantTagEffect : TagEffectBase
    {
        [SerializeField] private float _moveSpeedModifier = 5f;
        [SerializeField] private float _attackSpeedModifier = 0.7f;
        [SerializeField] private float _damageModifier = 5f;

        private PlayerTagCompo _tag;
        private PlayerStatCompo _stat;

        public override void Initialize(Player player)
        {
            base.Initialize(player);
            
            _tag = _player.GetEntityComponent<PlayerTagCompo>();
            _stat = _player.GetEntityComponent<PlayerStatCompo>();
        }

        protected override void TagEnableEffect(int tagCount)
        {
            _tag.ActiveParticle(EquipmentTag.MUTANT, true);
            
            _stat.AddModifier(StatType.MOVESPEED, "Mutant", _moveSpeedModifier);
            _stat.AddModifier(StatType.ATTACKSPEED, "Mutant", _attackSpeedModifier);
            _stat.AddModifier(StatType.DAMAGE, "Mutant", _damageModifier);
        }

        protected override void TagDisableEffect()
        {
            _tag.ActiveParticle(EquipmentTag.MUTANT, false);

            _stat.RemoveModifier(StatType.MOVESPEED, "Mutant");
            _stat.RemoveModifier(StatType.ATTACKSPEED, "Mutant");
            _stat.RemoveModifier(StatType.DAMAGE, "Mutant");
        }
    }
}
