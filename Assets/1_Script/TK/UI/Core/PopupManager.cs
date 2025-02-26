using System;
using System.Collections.Generic;
using System.Linq;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Swift_Blade
{
    public class PopupManager : MonoSingleton<PopupManager>
    {
        public SerializableDictionary<PopupType, PopupUI> popups
            = new SerializableDictionary<PopupType, PopupUI>();
        private List<PopupUI> _popupList = new List<PopupUI>();
        
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
            OpenCloseInventory();
            PopDownInput();

            // if (Keyboard.current.tKey.wasPressedThisFrame
            //     && !DialogueManager.Instance.IsDialogOpen)
            // {
            //     if (GetPopupUI(PopupType.Text) is TextPopup textPopup)
            //     {
            //         textPopup.SetText("Game over...");
            //         DelayPopup(PopupType.Text, 2f, () => textPopup.PopDown());
            //     }
            // }
        }

        private void PopDownInput()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame 
                && !DialogueManager.Instance.IsDialogueOpen)
            {
                PopDown();
            }
        }

        private void OpenCloseInventory()
        {
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                if (GetRemainPopup(PopupType.Inventory) != null)
                {
                    PopDown(PopupType.Inventory);
                }
                else
                {
                    PopUp(PopupType.Inventory);
                }
            }
        }

        public void PopUp(PopupType popupType)
        {
            if (_popupList.Contains(popups[popupType])) return;
            
            _popupList.Add(popups[popupType]);
            popups[popupType].Popup();
        }

        public void DelayPopup(PopupType popupType, float delay)
        {
            if (_popupList.Contains(popups[popupType])) return;
            
            _popupList.Add(popups[popupType]);
            popups[popupType].DelayPopup(delay);
        }

        public void DelayPopup(PopupType popupType, float delay, Action callback)
        {
            if (_popupList.Contains(popups[popupType])) return;
            
            _popupList.Add(popups[popupType]);
            popups[popupType].DelayPopup(delay, callback);
        }
        
        public void PopDown()
        {
            if (_popupList.Count > 0)
            {
                PopupUI popup = _popupList.Last();
                _popupList.RemoveAt(_popupList.Count - 1); 
                popup.PopDown();
            }
            else
            {
                PopUp(PopupType.Option);
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
                }
            }
            else
            {
                PopUp(PopupType.Option);
            }
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

        //for 문2번
        public PopupUI GetRemainPopup(PopupType type)
        {
            return _popupList.Find(x => x.popupType == type);
        }

        public void OpenSettingPopup() => PopUp(PopupType.Setting);
    }
}
