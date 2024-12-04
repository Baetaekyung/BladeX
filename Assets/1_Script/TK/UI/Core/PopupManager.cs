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
        }

        public void PopUp(PopupType popupType)
        {
            if (_popupStack.Contains(popups[popupType])) return;
            
            popups[popupType].Popup();
            _popupStack.Push(popups[popupType]);
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
    }
}
