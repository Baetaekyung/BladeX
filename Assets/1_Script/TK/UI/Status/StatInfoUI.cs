using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class StatInfoUI : MonoBehaviour
    {
        [SerializeField] private StatSO stat;

        [Space(10)] 
        [SerializeField] private Image statIcon;
        [SerializeField] private TextMeshProUGUI statInfoText;
        [SerializeField] private Button upgradeButton;
        
        private readonly StringBuilder _sb = new StringBuilder(1);

        private void OnEnable()
        {
            upgradeButton.onClick.AddListener(UpgradeStat);
        }

        private void OnDisable()
        {
            upgradeButton.onClick.RemoveListener(UpgradeStat);
        }

        private void UpgradeStat()
        {
            stat.BaseValue += 1f; //todo: statSo에 변수로 오르는 양 가져오기
            
            SetStatInfoUI();
        }
        
        [ContextMenu("set")]
        public void SetStatInfoUI()
        {
            //statIcon.sprite = stat.statIcon;
            
            _sb.Clear();

            _sb.Append(stat.displayName);
            _sb.Append(": ");
            _sb.Append(stat.Value).Append(" / ").Append(stat.MaxValue);

            statInfoText.text = _sb.ToString();
        }
    }
}
