using TMPro;
using UnityEngine;
using static Swift_Blade.Player;

namespace Swift_Blade
{
    public class StatusUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statPointText;

        private void OnEnable()
        {
            Player.LevelStat.OnLevelUp += HandleInfoChange;

            ColorRecorder.OnColorChanged += HandleInfoChange;
        }

        private void OnDisable()
        {
            Player.LevelStat.OnLevelUp -= HandleInfoChange;

            ColorRecorder.OnColorChanged -= HandleInfoChange;
        }

        private void HandleInfoChange(Player.LevelStat levelStat)
        {
            statPointText.text = $"Stat point: {levelStat.StatPoint}";
        }
        
        public void HandleInfoChange()
        {
            statPointText.text = $"Stat point: {Player.level.StatPoint}";
        }
    }
}
