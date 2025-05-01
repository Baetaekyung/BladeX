using UnityEngine;

namespace Swift_Blade
{
    public class HolyTagEffect : TagEffectBase
    {
        [SerializeField] private float _cycleTime = 30f;

        private PlayerStatCompo _stat;

        public override void Initialize(Player player)
        {
            base.Initialize(player);
            _stat = _player.GetEntityComponent<PlayerStatCompo>();
        }

        protected override void TagEnableEffect(int tagCount)
        {
            InvokeRepeating(nameof(Shield), 0f, _cycleTime);
        }

        private void Shield()
        {
            _player.GetSkillController.UseSkill(SkillType.Shield);
            _stat.BuffToStat(StatType.HEALTH, "Holy", _cycleTime, 2f);
        }

        protected override void TagDisableEffect()
        {
            CancelInvoke(nameof(Shield));
        }
    }
}
