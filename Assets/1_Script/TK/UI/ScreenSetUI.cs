using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ScreenSetUI : MonoBehaviour
    {
        [SerializeField] private Toggle       fullScreenToggle;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown fpsDropdown;
        
        private bool _isFullScreen;

        private void Awake()
        {
            InitializeFullScreen();
            InitializeResolution();
            InitializeFPS();
        }

        private void InitializeFullScreen()
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            _isFullScreen = Screen.fullScreen;
            
            fullScreenToggle.onValueChanged.AddListener(HandleFullScreenChanged);
            fullScreenToggle.isOn = _isFullScreen;
        }
        
        private void InitializeResolution()
        {
            resolutionDropdown.onValueChanged.AddListener(HandleResolutionChanged);
            resolutionDropdown.value = 1;
        }
        
        private void InitializeFPS()
        {
            fpsDropdown.onValueChanged.AddListener(HandleFPSChanged);
            fpsDropdown.value = 1;
        }

        private void HandleFullScreenChanged(bool isOn)
        {
            Screen.fullScreenMode = isOn ?
                FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }
        
        private void HandleResolutionChanged(int index)
        {
            switch (index)
            {
                case 0:
                    Screen.SetResolution(1366, 786, _isFullScreen);
                    break;
                case 1:
                    Screen.SetResolution(1920, 1080, _isFullScreen);
                    break;
                case 2:
                    Screen.SetResolution(2560, 1440, _isFullScreen);
                    break;
                default:
                    throw new ArgumentException("드롭다운의 범위를 벗어난 인덱스");
            }
        }
        
        private void HandleFPSChanged(int index)
        {
            switch (index)
            {
                case 0:
                    Application.targetFrameRate = 30;
                    break;
                case 1:
                    Application.targetFrameRate = 60;
                    break;
                case 2:
                    Application.targetFrameRate = -1; //fps 무제한
                    break;
                default:
                    throw new ArgumentException("드롭다운의 범위를 벗어난 index");
            }
        }
    }
}
