using UnityEngine;
using System.Text;
using TMPro;

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

            _sb.//Append(colorType.ToString()).
                Append("  ").
                Append(upgradePercent).
                Append("%").
                Append("\n").Append("\n").
                Append(colorValue.ToString());
                
            colorInfoText.text = _sb.ToString();
        }
    }
}
