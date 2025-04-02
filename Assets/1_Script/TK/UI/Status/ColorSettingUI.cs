using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ColorSettingUI : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private TextMeshProUGUI colorInfoText;
        [SerializeField] private ColorType       colorType;

        private readonly StringBuilder _sb = new();
        
        public void SetStatInfoUI(int colorValue, int upgradePercent)
        {
            _sb.Clear();

            _sb.Append(colorType.ToString()).
                Append(": ").
                Append(colorValue.ToString()).
                Append("\t").Append("\t").Append("\t").
                Append("강화 성공 확률: ").
                Append(upgradePercent);

            colorInfoText.text = _sb.ToString();
        }
    }
}
