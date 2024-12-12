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
        Swift = 6
    }
    
    public class StyleMeter : MonoSingleton<StyleMeter>
    {
        private readonly int _changeMeterUPAnimHash = Animator.StringToHash("ChangeMeterUP");
        private readonly int _changeMeterDOWNAnimHash = Animator.StringToHash("ChangeMeterDOWN");

        [Header("About visual")]
        private Animator _animator;
        [SerializeField] private Image _swiftIcon;
        
        [Header("StyleMeter Score")]
        private StyleMeterScore _currentMeterScore = StyleMeterScore.NONE;
        public StyleMeterScore CurrentMeterScore => _currentMeterScore; //스타일미터 프로퍼티

        [Header("Timer")]
        [SerializeField] private float _downMeterScoreTime = 6f;
        private float _currentTime = 0f;
        private float _meterUpPercent = 0f;
        private int _currentScore = 0; //미터 스코어 정수화

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (_downMeterScoreTime <= _currentTime)
            {
                _currentTime = 0f;
                DowngradeMeterScore();
            }
            else
            {
                _currentTime += Time.deltaTime;
            }
        }

        public void RaiseMeterPercent(float raisePercent)
        {
            _meterUpPercent += raisePercent;
            
            if (_meterUpPercent > 100f)
                UpgradeMeterScore();
        }
        
        public void UpgradeMeterScore()
        {
            if (_currentMeterScore == StyleMeterScore.Swift) return;

            _meterUpPercent = 0f;
            
            _currentScore++;
            _currentMeterScore = (StyleMeterScore)_currentScore;

            float normalizedValue = (float)_currentMeterScore / 6;
            _swiftIcon.fillAmount = normalizedValue;
            
            _animator.SetTrigger(_changeMeterUPAnimHash);
        }

        public void DowngradeMeterScore()
        {
            if (_currentMeterScore == StyleMeterScore.NONE) return;

            _currentScore--;
            _currentMeterScore = (StyleMeterScore)_currentScore;
            _animator.SetTrigger(_changeMeterDOWNAnimHash);
            
            float normalizedValue = (float)_currentMeterScore / 6;
            _swiftIcon.fillAmount = normalizedValue;
        }
    }
}
