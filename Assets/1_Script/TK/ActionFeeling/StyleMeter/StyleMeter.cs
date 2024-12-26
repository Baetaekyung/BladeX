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
        private StyleMeterScore _currentMeterPercent = StyleMeterScore.NONE;
        public StyleMeterScore CurrentMeterPercent => _currentMeterPercent; //스타일미터 프로퍼티

        [Header("Timer")]
        [SerializeField] private float _downMeterScoreTime = 6f;
        private float _currentTime = 0f;
        private float _meterUpPercent = 0f;
        private int _currentMeterRank = 0; //미터 스코어 정수화

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            float normalizedValue = (float)_currentMeterPercent / 6;
            _swiftIcon.fillAmount = normalizedValue;

        }

        private void Update()
        {
            if (_downMeterScoreTime <= _currentTime)
                DowngradeMeterRank();
            else
                _currentTime += Time.deltaTime;
        }

        public void RaiseMeterPercent(float raisePercent)
        {
            _meterUpPercent += raisePercent;
            
            if (_meterUpPercent >= 100f)
                UpgradeMeterRank();
        }
        
        public void UpgradeMeterRank()
        {
            if (_currentMeterPercent == StyleMeterScore.Swift) return;

            _meterUpPercent = 0f;
            _currentTime = 0f; //다운그레이드 초기화
            
            _currentMeterRank++;
            _currentMeterPercent = (StyleMeterScore)_currentMeterRank;

            float normalizedValue = (float)_currentMeterPercent / 6;
            _swiftIcon.fillAmount = normalizedValue;
            
            _animator.SetTrigger(_changeMeterUPAnimHash);
        }

        public void DowngradeMeterRank()
        {
            _currentTime = 0f;
            if (_currentMeterPercent == StyleMeterScore.NONE) return;

            _currentMeterRank--;
            _currentMeterPercent = (StyleMeterScore)_currentMeterRank;
            _animator.SetTrigger(_changeMeterDOWNAnimHash);
            
            float normalizedValue = (float)_currentMeterPercent / 6;
            _swiftIcon.fillAmount = normalizedValue;
        }
    }
}
