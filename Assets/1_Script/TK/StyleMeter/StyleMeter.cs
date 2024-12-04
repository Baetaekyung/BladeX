using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    public enum StyleMeterScore : short
    {
        NONE = 0,
        D = 1,
        C = 2,
        B = 3,
        A = 4,
        S = 5,
        SS = 6,
        Swift = 7
    }
    
    public class StyleMeter : MonoSingleton<StyleMeter>
    {
        private readonly int _changeMeterUPAnimHash = Animator.StringToHash("ChangeMeterUP");
        private readonly int _changeMeterDOWNAnimHash = Animator.StringToHash("ChangeMeterDOWN");
        private Animator _animator;

        [SerializeField] private Image _swiftIcon;
        
        private StyleMeterScore _currentMeterScore = StyleMeterScore.NONE;
        public StyleMeterScore CurrentMeterScore => _currentMeterScore;
        
        public static float StyleMeterMultiplier { get; private set; } = 1;

        private int _currentScore = 0;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        
        public void UpgradeMeterScore()
        {
            if (_currentMeterScore == StyleMeterScore.Swift) return;

            _currentScore++;
            _currentMeterScore = (StyleMeterScore)_currentScore;

            float normalizedValue = (float)_currentMeterScore / 7;
            _swiftIcon.fillAmount = normalizedValue;
            
            _animator.SetTrigger(_changeMeterUPAnimHash);
        }

        public void DowngradeMeterScore()
        {
            if (_currentMeterScore == StyleMeterScore.NONE) return;

            _currentScore--;
            _currentMeterScore = (StyleMeterScore)_currentScore;
            _animator.SetTrigger(_changeMeterDOWNAnimHash);
            
            float normalizedValue = (float)_currentMeterScore / 7;
            _swiftIcon.fillAmount = normalizedValue;
        }
    }
}
