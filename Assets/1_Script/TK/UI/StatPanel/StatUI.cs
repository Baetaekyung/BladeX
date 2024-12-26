using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class StatUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _strengthText;
        [SerializeField] private TextMeshProUGUI _attackSpeedText;
        [SerializeField] private TextMeshProUGUI _moveSpeedText;
        [SerializeField] private TextMeshProUGUI _steminaText;
        [SerializeField] private TextMeshProUGUI _criticalChanceText;
        [SerializeField] private TextMeshProUGUI _criticalDamageText;
        [SerializeField] private TextMeshProUGUI _parryDurationText;
        
        [SerializeField] private StatComponent _targetStat;
        private StringBuilder _sb = new StringBuilder();

        public void SetTargetStat(StatComponent statCompo)
        {
            _targetStat = statCompo;
        }

        private void Start()
        {
            SetStatText(_targetStat.GetStatByType(StatType.HEALTH), _healthText);
            SetStatText(_targetStat.GetStatByType(StatType.STRENGTH), _strengthText);
            SetStatText(_targetStat.GetStatByType(StatType.ATTACKSPEED), _attackSpeedText, true);
            SetStatText(_targetStat.GetStatByType(StatType.MOVESPEED), _moveSpeedText, true);
            SetStatText(_targetStat.GetStatByType(StatType.STAMINA), _steminaText);
            SetStatText(_targetStat.GetStatByType(StatType.CRITICALPERCENT), _criticalChanceText, true);
            SetStatText(_targetStat.GetStatByType(StatType.CRITICALDAMAGE), _criticalDamageText, true);
            SetStatText(_targetStat.GetStatByType(StatType.PARRYDURATION), _parryDurationText);
        }

        public void SetStatText(StatSO stat, TextMeshProUGUI targetText, bool isPercent = false)
        {
            _sb.Clear();
            if(isPercent)
                _sb.Append(stat.displayName).Append(":  ").Append(stat.Value.ToString()).Append("%");
            else
                _sb.Append(stat.displayName).Append(":  ").Append(stat.Value.ToString());
            
            targetText.text = _sb.ToString();
        }
    }
}
