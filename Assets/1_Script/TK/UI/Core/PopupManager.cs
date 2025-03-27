using System;
using System.Collections.Generic;
using System.Linq;
using Swift_Blade.Level;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Swift_Blade
{
    public class PopupManager : MonoSingleton<PopupManager>
    {
        public SerializableDictionary<PopupType, PopupUI> popups = new();

        private List<PopupUI> _popupList = new List<PopupUI>();
        public event Action   OnPopUpOpenOrClose;

        public bool IsRemainPopup => _popupList.Count > 0;

        private void Start()
        {
            InitPopups();
        }

        private void InitPopups()
        {
            foreach (var popupUI in popups.Values)
                popupUI.PopDown();
        }

        private void Update()
        {   
            OpenCloseInventory();

            PopDownInput();
        }

        private void PopDownInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape)
                && !DialogueManager.Instance.IsDialogueOpen)
            {
                PopDown();
            }
        }

        private void OpenCloseInventory()
        {
            if (popups.ContainsKey(PopupType.Inventory) == false)
                return;
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (GetRemainPopup(PopupType.Inventory) != null)
                    PopDown(PopupType.Inventory);
                else
                    PopUp(PopupType.Inventory);
            }
        }

        public void PopUp(PopupType popupType)
        {
            if (_popupList.Contains(popups[popupType])) return;
            
            _popupList.Add(popups[popupType]);

            popups[popupType].Popup();
            popups[popupType].transform.SetAsLastSibling();

            OnPopUpOpenOrClose?.Invoke();
        }
        
        public void DelayPopup(PopupType popupType, float delay)
        {
            if (_popupList.Contains(popups[popupType])) return;
            
            _popupList.Add(popups[popupType]);

            popups[popupType].DelayPopup(delay);
            popups[popupType].transform.SetAsLastSibling();

            OnPopUpOpenOrClose?.Invoke();
        }

        public void DelayPopup(PopupType popupType, float delay, Action callback)
        {
            if (_popupList.Contains(popups[popupType])) return;
            
            _popupList.Add(popups[popupType]);

            popups[popupType].DelayPopup(delay, callback);
            popups[popupType].transform.SetAsLastSibling();

            OnPopUpOpenOrClose?.Invoke();
        }
        
        public void PopDown()
        {
            if (_popupList.Count > 0)
            {
                PopupUI popup = _popupList.Last();
                popup.PopDown();

                _popupList.RemoveAt(_popupList.Count - 1);

                OnPopUpOpenOrClose?.Invoke();
            }
            else
            {
                PopUp(PopupType.Option);

                OnPopUpOpenOrClose?.Invoke();
            }
        }

        public void PopDown(PopupType popupType)
        {
            PopupUI popup = null;
            int index = 0;
            
            if (_popupList.Count > 0)
            {
                for (int i = 0; i < _popupList.Count; i++)
                {
                    if (_popupList[i].popupType == popupType)
                    {
                        index = i;
                        popup = _popupList[i];
                        break;
                    }
                }

                if (popup != null)
                {
                    _popupList.RemoveAt(index);
                    popup.PopDown();
                    OnPopUpOpenOrClose?.Invoke();
                }
            }
            else
            {
                PopUp(PopupType.Option);

                OnPopUpOpenOrClose?.Invoke();
            }
        }

        public void LogMessage(string message)
        {
            PopupUI popup = GetPopupUI(UI.PopupType.Text);
            TextPopup textPopup = popup as TextPopup;

            textPopup.SetText(message);
            DelayPopup(PopupType.Text, 1f, () => PopDown(PopupType.Text));
        }

        public void AllPopDown()
        {
            while (_popupList.Count != 0)
                PopDown();
        }
        
        public PopupUI GetPopupUI(PopupType type)
        {
            if (popups.TryGetValue(type, out var popup) == false)
            {
                Debug.Log($"{type}의 팝업이 존재하지 않음.");
                return null;
            }
            
            return popup;
        }

        public PopupUI GetRemainPopup(PopupType type)
        {
            return _popupList.Find(x => x.popupType == type);
        }

        public void OpenSettingPopup() => PopUp(PopupType.Setting);
        public void OpenQuitHelpPopup() => PopUp(PopupType.Option); 
    }
}
