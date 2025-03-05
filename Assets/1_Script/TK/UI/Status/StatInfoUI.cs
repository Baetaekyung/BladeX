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
        [SerializeField] private TextMeshProUGUI statInfoText;
        [SerializeField] private Button upgradeButton;
        
        private readonly StringBuilder _sb = new(1);

        private void OnEnable()
        {
            upgradeButton.onClick.AddListener(UpgradeStat);
            SetStatInfoUI();
        }

        private void OnDisable()
        {
            upgradeButton.onClick.RemoveAllListeners();
        }

        private void UpgradeStat()
        {
            if (Player.level.StatPoint <= 0)
                return;

            //Player.level.StatPoint -= 1; set 할 수 없는..
            stat.BaseValue += stat.increaseAmount;
            
            SetStatInfoUI();
        }
        
        [ContextMenu("set")]
        public void SetStatInfoUI()
        { 
            _sb.Clear();

            _sb.Append(stat.displayName);
            _sb.Append(": ");
            _sb.Append(stat.Value).Append(" / ").Append(stat.MaxValue);

            statInfoText.text = _sb.ToString();
        }
    }
}
