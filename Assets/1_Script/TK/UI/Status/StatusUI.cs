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
            StatInfoUI.OnStatChanged += HandleInfoChange;
            //TODO: StatInfoUI.OnStatChanged += HandleInfoChange;
        }

        private void OnDisable()
        {
            Player.LevelStat.OnLevelUp -= HandleInfoChange;
            StatInfoUI.OnStatChanged -= HandleInfoChange;
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
        
        public void HandleInfoChange()
        {
            foreach (var statText in statPointText)
            {
                statText.text = $"Stat point: {Player.level.StatPoint}";
            }
        }
    }
}
