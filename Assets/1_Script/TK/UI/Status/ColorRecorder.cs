using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class ColorRecorder : MonoBehaviour
    {
        public static event Action OnColorChanged; //int is colorValue

        [SerializeField] private ColorType colorType;
        [SerializeField] private int upgradePercent = 95;
        [SerializeField] private int percentDecreasePer;
        [SerializeField] private ColorSettingUI colorSettingUI;
        private int increasedAmount;

        private PlayerStatCompo _statCompo;

        private void Start()
        {
            _statCompo = Player.Instance.GetEntityComponent<PlayerStatCompo>();

            colorSettingUI.SetStatInfoUI(_statCompo.GetColorStatValue(colorType), upgradePercent);
        }

        //Button Event
        public void UpgradeStat()
        {
            if (Player.level.StatPoint <= 0)
            {
                PopupManager.Instance.LogMessage("���� ����Ʈ�� �����ϴ�");
                return;
            }

            Player.level.StatPoint -= 1;
            TryUpgrade();
        }

        private void TryUpgrade()
        {
            if (UnityEngine.Random.Range(0, 100) <= upgradePercent)
            {
                _statCompo.IncreaseColorValue(colorType, 1);

                upgradePercent -= percentDecreasePer;
                upgradePercent = Mathf.Clamp(upgradePercent, 5, 95); //��� 5�۴� �Ǿ�߰���? �ƴԸ���

                increasedAmount += 1;

                colorSettingUI.SetStatInfoUI(_statCompo.GetColorStatValue(colorType), upgradePercent);

                PopupManager.Instance.LogMessage("����");
                return;
            }
            PopupManager.Instance.LogMessage("����");
        }

        public void InitializeStat()
        {
            if(increasedAmount == 0)
                return;

            Player.level.StatPoint += increasedAmount;

            _statCompo.DecreaseColorValue(colorType, increasedAmount);

            increasedAmount = 0;
            colorSettingUI.SetStatInfoUI(_statCompo.GetColorStatValue(colorType), upgradePercent);
        }
    }

}
