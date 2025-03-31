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

        public static event Action        OnStatChanged;
        public static event Action<float> OnHealthStatUp;
        public static event Action<float> OnHealthStatDown;

        private static Dictionary<StatSO, int> _statRecords = new Dictionary<StatSO, int>();

        [Space(10)]
        [SerializeField] private StatSO          stat;
        [SerializeField] private TextMeshProUGUI statInfoText;
        [SerializeField] private Button          upgradeButton;
        
        private readonly StringBuilder _sb = new();

        private void OnEnable()
        {
            OnStatChanged += SetStatInfoUI;

            upgradeButton.onClick.AddListener(UpgradeStat);
        }

        private void Start()
        {
            stat = Player.Instance
                .GetEntityComponent<PlayerStatCompo>()
                .GetStat(stat);

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
                PopupManager.Instance.LogMessage("스텟 포인트가 부족하다");
                return;
            }

            if (Mathf.Approximately(stat.ColorValue, stat.MaxValue))
            {
                PopupManager.Instance.LogMessage("더 이상 올릴 수 없다");
                return;
            }

            Player.level.StatPoint -= 1;
            UsedStatPoint++;

            if (stat.statType == StatType.HEALTH)
                OnHealthStatUp?.Invoke(1);
            
            RecordAddedStat();
            SetStatInfoUI();
            
            OnStatChanged?.Invoke();
        }

        private void RecordAddedStat()
        {
            if (!_statRecords.ContainsKey(stat))
                _statRecords.Add(stat, 1);
            else
                _statRecords[stat] += 1;
        }

        //Button Event
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
                //statRecord.Key.ColorValue -= statRecord.Value * statRecord.Key.increaseAmount;

                if (statRecord.Key.statType == StatType.HEALTH)
                {
                    Debug.Log(statRecord.Value * statRecord.Key.increaseAmount);
                    OnHealthStatDown?.Invoke(statRecord.Value * statRecord.Key.increaseAmount);
                }
            }

            UsedStatPoint = 0;
            _statRecords.Clear();
            OnStatChanged?.Invoke();
        }
        
        private void SetStatInfoUI()
        { 
            _sb.Clear();
            
            _sb.Append(stat.displayName)
                .Append(": ");

            if (stat.statType != StatType.HEALTH)
            {
                _sb.Append((Mathf.Clamp(stat.ColorValue, stat.MinValue, stat.MaxValue)).ToString("0.00"))
                    .Append(" / ").Append(stat.MaxValue);
            }
            else
            {
                _sb.Append((Mathf.Clamp(Mathf.RoundToInt(stat.ColorValue), stat.MinValue, stat.MaxValue))
                        .ToString("N"))
                        .Append(" / ").Append(stat.MaxValue);
            }

            statInfoText.text = _sb.ToString();
        }
    }
}
