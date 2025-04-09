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
        [SerializeField] private int            upgradePercent = 100;
        [SerializeField] private int            percentDecreasePer;

        private PlayerStatCompo _statCompo;
        private int             recordedIncreasedAmount;

        private void Start()
        {
            _statCompo = Player.Instance.GetEntityComponent<PlayerStatCompo>();

            if (_statCompo == null)
            {
                Debug.Log("PlayerStatCompo is null, so ColorRecorder can't work", transform);
                return;
            }

            int colorStatValue = _statCompo.GetColorStatValue(colorType);
            colorSettingUI.SetStatInfoUI(colorStatValue, upgradePercent);
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

            if (randomPercent <= upgradePercent)
            {
                PopupManager.Instance.LogMessage("[ 성공 ]");

                _statCompo.IncreaseColorValue(colorType, 1);
                recordedIncreasedAmount += 1; //Record success count
                OnColorChanged?.Invoke();

                int colorStatValue = _statCompo.GetColorStatValue(colorType);
                colorSettingUI.SetStatInfoUI(colorStatValue, upgradePercent);

                // min is 5, max is 100
                upgradePercent = Mathf.Clamp(
                    upgradePercent - percentDecreasePer, 
                    MIN_UPGRADE_PERCENT, 
                    MAX_UPGRADE_PERCENT); 
            }
            else
                PopupManager.Instance.LogMessage("[ 실패 ]");
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

            upgradePercent += percentDecreasePer;

            _statCompo.DecreaseColorValue(colorType, 1);
            recordedIncreasedAmount -= 1;
            OnColorChanged?.Invoke();

            int colorStatValue = _statCompo.GetColorStatValue(colorType);
            colorSettingUI.SetStatInfoUI(colorStatValue, upgradePercent);

            return;
        }

        //Button Event
        public void InitializeStat()
        {
            if (_statCompo == null)
                return;

            if (recordedIncreasedAmount == 0)
                return;

            _statCompo.DecreaseColorValue(colorType, recordedIncreasedAmount);
            OnColorChanged?.Invoke();

            Player.level.StatPoint += recordedIncreasedAmount;
            recordedIncreasedAmount = 0;

            colorSettingUI.SetStatInfoUI(_statCompo.GetColorStatValue(colorType), upgradePercent);
        }
    }

}
