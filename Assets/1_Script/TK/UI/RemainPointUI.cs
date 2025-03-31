using System;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class RemainPointUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI remainText;

        private void Start()
        {
            HandleUpdateRemainUI(Player.level);
        }
        
        private void OnEnable()
        {
            Player.LevelStat.OnLevelUp += HandleUpdateRemainUI;
        }
        
        private void OnDisable()
        {
            Player.LevelStat.OnLevelUp -= HandleUpdateRemainUI;
        }
                
        private void HandleUpdateRemainUI(Player.LevelStat levelStat)
        {
            remainText.text = $"남은 스텟포인트: {levelStat.StatPoint.ToString()}";
        }
    }
}
