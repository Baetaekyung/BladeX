using System;
using TMPro;
using UnityEngine;
using static Swift_Blade.Player;

namespace Swift_Blade
{
    public class RemainPointUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI remainText;

        private void Start()
        {
            HandleUpdateRemainUI(level);
        }
        
        private void OnEnable()
        {
            LevelStat.OnLevelUp += HandleUpdateRemainUI;
            ColorRecorder.OnColorChanged += HandleUpdateRemainUI;
        }
        
        private void OnDisable()
        {
            LevelStat.OnLevelUp -= HandleUpdateRemainUI;
            ColorRecorder.OnColorChanged -= HandleUpdateRemainUI;
        }

        private void HandleUpdateRemainUI(LevelStat levelStat)
        {
            remainText.text = $"남은 스텟포인트: {levelStat.StatPoint.ToString()}";
        }

        private void HandleUpdateRemainUI()
        {
            remainText.text = $"남은 스텟포인트: {level.StatPoint.ToString()}";
        }
    }
}
