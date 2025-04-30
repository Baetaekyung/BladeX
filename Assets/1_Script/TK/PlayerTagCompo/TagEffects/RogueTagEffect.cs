using Swift_Blade.Combat.Caster;
using System;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    public class RogueTagEffect : TagEffectBase
    {
        private PlayerDamageCaster _playerDmgCaster;
        private PlayerStatCompo _playerStatCompo;

        private int _currentPercent;

        // °è¼ö´Â 1 ½ºÅÝ¾÷
        // 3ÀÌ¸é 3 ½ºÅÝ¾÷ÀÇ È¿°ú
        [SerializeField] private int minStealAmount = 1;
        [SerializeField] private int middleStealAmount = 2;
        [SerializeField] private int maxStealAmount = 3;

        private int _currentAmount;

        private StringBuilder _sb = new();

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
        //public override void EnableTagEffect(int tagCount)
        //{
        //    if (minTagCount > tagCount)
        //        return;

        //    if (tagCount >= maxTagCount)
        //        OnTagEffect(maxEffectPercent, maxStealAmount);
        //    else if (tagCount >= middleTagCount)
        //        OnTagEffect(middleEffectPercent, middleStealAmount);
        //    else if (tagCount >= minTagCount)
        //        OnTagEffect(minEffectPercent, minStealAmount);
        //}

        //public override void DisableTagEffect(int tagCount)
        //{
        //    _playerDmgCaster.OnCastDamageEvent.RemoveListener(HandleStealStat);
        //}

        private void OnTagEffect(int percent, int amount)
        {
            _currentPercent = percent;
            _currentAmount = amount;

            _playerDmgCaster.OnCastDamageEvent.AddListener(HandleStealStat);
        }

        //Dont need arg
        private void HandleStealStat(ActionData actionData)
        {
            if (Random.Range(0, 101) > _currentPercent)
                return;

            StatType[] types = (StatType[])Enum.GetValues(typeof(StatType));
            StatType randomType = types[Random.Range(0, types.Length)];
            float increaseAmount = _playerStatCompo.GetStat(randomType).increaseAmount;

            _sb.Clear();
            _sb.Append(randomType.ToString()).Append("StealData");

            _playerStatCompo.BuffToStat(randomType,
                _sb.ToString(), 3f,
                _currentAmount * increaseAmount);

            PopupManager
                .Instance
                .LogInfoBox($"StatÀ» ÈÉÃÆ½À´Ï´Ù. ÈÉÄ£ ½ºÅÝ: {KoreanUtility.GetStatTypeToKorean(randomType)}");
        }

    }
}
