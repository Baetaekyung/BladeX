using System;
using System.Collections.Generic;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Swift_Blade
{
    [MonoSingletonUsage(MonoSingletonFlags.DontDestroyOnLoad)]
    public class PopupManager : MonoSingleton<PopupManager>
    {
        public SerializableDictionary<PopupType, PopupUI> popups
            = new SerializableDictionary<PopupType, PopupUI>();
        private Stack<PopupUI> _popupStack = new Stack<PopupUI>();
        
        private float _delayTime = 3f; //이거 리터럴 임시변수고 SetDelay로 설정해주기

        private void Start()
        {
            InitPopups();
        }

        private void InitPopups()
        {
            foreach (var popupUI in popups.Values)
            {
                popupUI.PopDown();
            }
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                PopDown();
            }

            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                if (GetPopupUI(PopupType.Text) is TextPopup textPopup)
                {
                    textPopup.SetText("Game over...");
                }
                DelayPopup(PopupType.Text, 2f);
            }
        }

        public void PopUp(PopupType popupType)
        {
            if (_popupStack.Contains(popups[popupType])) return;
            
            popups[popupType].Popup();
            _popupStack.Push(popups[popupType]);
        }

        public void DelayPopup(PopupType popupType, float delay)
        {
            if (_popupStack.Contains(popups[popupType])) return;
            
            popups[popupType].DelayPopup(delay);
        }
        
        public void PopDown()
        {
            if (_popupStack.Count > 0)
            {
                PopupUI popup = _popupStack.Pop();
                popup.PopDown();
            }
            else
            {
                PopUp(PopupType.Option);
            }
        }

        public PopupUI GetPopupUI(PopupType type)
        {
            if (popups.ContainsKey(type) is false)
            {
                Debug.Log($"{type}의 팝업이 존재하지 않음.");
                return default;
            }
            
            return popups[type];
        }
        
        public void SetDelay(float delay) => _delayTime = delay;
    }
}
