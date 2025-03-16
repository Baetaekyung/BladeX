using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class StatInfoUI : MonoBehaviour
    {
        public static int UsedStatPoint = 0;
        public static event Action OnStatChanged;

        private static Dictionary<StatSO, int> _statRecords = new Dictionary<StatSO, int>();
        [SerializeField] private StatSO stat;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI statInfoText;
        [SerializeField] private Button upgradeButton;
        
        private readonly StringBuilder _sb = new(1);

        private void OnEnable()
        {
            OnStatChanged += SetStatInfoUI;
            upgradeButton.onClick.AddListener(UpgradeStat);
            
            SetStatInfoUI();
        }

        private void OnDisable()
        {
            OnStatChanged -= SetStatInfoUI;
            upgradeButton.onClick.RemoveAllListeners();
        }

        private void UpgradeStat()
        {
            if (Player.level.StatPoint <= 0)
            {
                Debug.Log("스텟포인트가 없다.");
            }

            if (Mathf.Approximately(stat.BaseValue, stat.MaxValue))
            {
                Debug.Log("스텟이 최대이다.");
            }

            Player.level.StatPoint -= 1;
            UsedStatPoint++;
            stat.BaseValue += stat.increaseAmount;

            RecordAddedStat();
            SetStatInfoUI();
            
            OnStatChanged?.Invoke();
        }

        private void RecordAddedStat()
        {
            if (!_statRecords.TryAdd(stat, 1))
                _statRecords[stat] += 1;
        }

        public void InitializeStat()
        {
            InitializeStatPoint();
        }
        
        private static void InitializeStatPoint()
        {
            if (UsedStatPoint == 0)
                return;
            
            Player.level.StatPoint += UsedStatPoint;
            foreach (var statRecord in _statRecords)
            {
                statRecord.Key.BaseValue -= statRecord.Value * statRecord.Key.increaseAmount;
            }

            UsedStatPoint = 0;
            _statRecords.Clear();
            OnStatChanged?.Invoke();
        }
        
        private void SetStatInfoUI()
        { 
            _sb.Clear();
            
            _sb.Append(stat.displayName);
            _sb.Append(": ");

            if (stat.statType != StatType.HEALTH)
            {
                _sb.Append((Mathf.Clamp(stat.BaseValue, stat.MinValue, stat.MaxValue)).ToString("0.00"))
                    .Append(" / ").Append(stat.MaxValue);
            }
            else
            {
                _sb.Append((Mathf.Clamp(Mathf.RoundToInt(stat.BaseValue), stat.MinValue, stat.MaxValue))
                        .ToString("N"))
                        .Append(" / ").Append(stat.MaxValue);
            }

            statInfoText.text = _sb.ToString();
        }
    }
}
