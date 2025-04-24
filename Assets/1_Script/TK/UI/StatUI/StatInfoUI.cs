using Swift_Blade.Combat.Health;
using System.Text;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class StatInfoUI : MonoBehaviour
    {
        private TextMeshProUGUI statInfo;

        private StringBuilder _sb = new();

        private void Awake()
        {
            statInfo = GetComponent<TextMeshProUGUI>();
        }

        public void SetUI(StatSO stat)
        {
            _sb.Clear();

            _sb.Append(KoreanUtility.GetStatTypeToKorean(stat.statType));
            _sb.Append(" - ");
            _sb.Append(stat.Value);

            if(stat.statType == StatType.HEALTH)
                _sb.Append($" + {Player.Instance.GetEntityComponent<PlayerHealth>().ShieldAmount}");

            statInfo.text = _sb.ToString();
        }
    }
}
