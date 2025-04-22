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
                Append("+").
                Append(colorValue.ToString()).
                Append("\n").Append("\n").Append("\n").
                Append(upgradePercent).
                Append("%");
            
            colorInfoText.text = _sb.ToString();
        }
    }
}
