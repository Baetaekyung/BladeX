using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    public enum StyleMeterScore
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
    
    public class StyleMeter : MonoBehaviour
    {
        private readonly int _changeMeterUPAnimHash = Animator.StringToHash("ChangeMeterUP");
        private readonly int _changeMeterDOWNAnimHash = Animator.StringToHash("ChangeMeterDOWN");
        private Animator _animator;

        [SerializeField] private Image _swiftIcon;
        
        private static StyleMeterScore _currentMeterScore = StyleMeterScore.D;
        public static StyleMeterScore CurrentMeterScore => _currentMeterScore;
        [SerializeField] private float _styleMeterSpeedAdder = 0.08f;
        public static float StyleMeterMultiplier { get; private set; } = 1;

        private int _currentScore = 0;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                UpgradeMeterScore();
            }
            else if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                DowngradeMeterScore();
            }
        }
        
        public void UpgradeMeterScore()
        {
            if (_currentMeterScore == StyleMeterScore.Swift) return;

            _currentScore++;
            _currentMeterScore = (StyleMeterScore)_currentScore;
            StyleMeterMultiplier += _styleMeterSpeedAdder;

            float normalizedValue = (float)_currentMeterScore / 7;
            _swiftIcon.fillAmount = normalizedValue;
            
            _animator.SetTrigger(_changeMeterUPAnimHash);
        }

        public void DowngradeMeterScore()
        {
            if (_currentMeterScore == StyleMeterScore.NONE) return;

            _currentScore--;
            StyleMeterMultiplier -= _styleMeterSpeedAdder;
            _currentMeterScore = (StyleMeterScore)_currentScore;
            _animator.SetTrigger(_changeMeterDOWNAnimHash);
            
            float normalizedValue = (float)_currentMeterScore / 7;
            _swiftIcon.fillAmount = normalizedValue;
        }
    }
}
