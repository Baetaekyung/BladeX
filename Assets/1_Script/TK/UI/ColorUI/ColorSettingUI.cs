using System;
using UnityEngine;
using System.Text;
using TMPro;

namespace Swift_Blade
{
    public class ColorSettingUI : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private TextMeshProUGUI upgradeCountText;
        [SerializeField] private TextMeshProUGUI upgradePercentText;
        
        
        [SerializeField] private ColorType       colorType;

        private readonly StringBuilder _sb = new();

        public void SetStatInfoUI(int colorValue, int upgradePercent)
        {
            _sb.Clear();
            _sb.Append("+").Append(colorValue.ToString());
            upgradeCountText.text = _sb.ToString();
            
            _sb.Clear();
            _sb.Append(upgradePercent.ToString());
            _sb.Append("%");
            
            upgradePercentText.SetText(_sb);
            
        }
    }
}
