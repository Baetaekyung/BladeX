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
            //TODO: StatInfoUI.OnStatChanged += HandleInfoChange;
        }

        private void OnDisable()
        {
            Player.LevelStat.OnLevelUp -= HandleInfoChange;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                //TODO: test code delete
                Player.level.StatPoint++;
                HandleInfoChange(Player.level);
            }
        }

        private void HandleInfoChange(Player.LevelStat levelStat)
        {
            foreach (var statText in statPointText)
            {
                statText.text = $"Stat point: {levelStat.StatPoint}";
            }
        }
    }
}
