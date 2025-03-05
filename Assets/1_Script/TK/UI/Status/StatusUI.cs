using System;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class StatusUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] statPointText;

        private void OnEnable()
        {
            Player.LevelStat.OnLevelUp += HandleInfoChange;
        }

        private void OnDisable()
        {
            Player.LevelStat.OnLevelUp -= HandleInfoChange;
        }

        private void HandleInfoChange(Player.LevelStat levelStat)
        {
            foreach (var statText in statPointText)
            {
                statText.text = $"Remain Stat Point: {levelStat.StatPoint}";
            }
        }
    }
}
