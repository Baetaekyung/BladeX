using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class ColorRecorder : MonoBehaviour
    {
        private const int MIN_UPGRADE_PERCENT = 5;
        private const int MAX_UPGRADE_PERCENT = 100;

        public static event Action OnColorChanged;

        [SerializeField] private ColorSettingUI colorSettingUI;
        [SerializeField] private ColorType      colorType;
        [SerializeField] private int            baseUpgradePercent = 100;
        [SerializeField] private int            percentDecreasePer;

        private int _upgradePercent;

        private static SerializableDictionary<ColorType, int> _increaseAmountDic = new();
        private static SerializableDictionary<ColorType, int> _upgradePercentDic = new();

        private PlayerStatCompo _statCompo;
        private int recordedIncreasedAmount;

        private void Start()
        {
            _statCompo = Player.Instance.GetEntityComponent<PlayerStatCompo>();

            if (_statCompo == null)
            {
                Debug.Log("PlayerStatCompo is null, so ColorRecorder can't work", transform);

                return;
            }

            _upgradePercent = baseUpgradePercent;

            LoadData();
            colorSettingUI.SetStatInfoUI(recordedIncreasedAmount, _upgradePercent);
        }

        private void LoadData()
        {
            if (!_increaseAmountDic.ContainsKey(colorType))
            {
                _increaseAmountDic.Add(colorType, recordedIncreasedAmount);
            }
            else
            {
                recordedIncreasedAmount = _increaseAmountDic[colorType];
            }

            if (!_upgradePercentDic.ContainsKey(colorType))
            {
                _upgradePercentDic.Add(colorType, baseUpgradePercent);
            }
            else
            {
                _upgradePercent = _upgradePercentDic[colorType];
            }
        }

        //Button Event
        public void UpgradeStat()
        {
            if (_statCompo == null)
                return;

            if (Player.level.StatPoint <= 0)
            {
                PopupManager.Instance.LogMessage("스텟 포인트가 부족하다");

                return;
            }

            Player.level.StatPoint -= 1;

            TryUpgrade();
        }

        private void TryUpgrade()
        {
            int randomPercent = UnityEngine.Random.Range(0, 100); // 0 ~ 99

            if (randomPercent <= _upgradePercent)
            {
                PopupManager.Instance.LogMessage("[ 성공 ]");

                _statCompo.IncreaseColorValue(colorType, 1);
                recordedIncreasedAmount += 1; //Record success count

                _increaseAmountDic[colorType] = recordedIncreasedAmount;

                // min is 5, max is 100
                _upgradePercent = Mathf.Clamp(
                    _upgradePercent - percentDecreasePer, 
                    MIN_UPGRADE_PERCENT, 
                    MAX_UPGRADE_PERCENT);
            }
            else
                PopupManager.Instance.LogMessage("[ 실패 ]");

            colorSettingUI.SetStatInfoUI(recordedIncreasedAmount, _upgradePercent);
            OnColorChanged?.Invoke();
        }

        public bool CheckValidToDecrease()
        {
            if (recordedIncreasedAmount == 0)
            {
                PopupManager.Instance.LogMessage($"{colorType.ToString()} 색이 부족합니다");

                return false;
            }

            return true;
        }

        public void DecreaseColor()
        {
            if (_statCompo == null)
                return;

            _upgradePercent += percentDecreasePer;

            _statCompo.DecreaseColorValue(colorType, 1);
            recordedIncreasedAmount -= 1;
            _increaseAmountDic[colorType] = recordedIncreasedAmount;

            OnColorChanged?.Invoke();
            colorSettingUI.SetStatInfoUI(recordedIncreasedAmount, _upgradePercent);
        }

        //Button Event
        public void InitializeStat()
        {
            if (_statCompo == null)
                return;

            if (recordedIncreasedAmount == 0)
                return;

            _statCompo.DecreaseColorValue(colorType, recordedIncreasedAmount);

            Player.level.StatPoint += recordedIncreasedAmount;
            recordedIncreasedAmount = 0;

            _increaseAmountDic[colorType] = recordedIncreasedAmount;
            _upgradePercentDic[colorType] = baseUpgradePercent;

            _upgradePercent = baseUpgradePercent;
            colorSettingUI.SetStatInfoUI(recordedIncreasedAmount, _upgradePercent);
            OnColorChanged?.Invoke();
        }
    }
}
