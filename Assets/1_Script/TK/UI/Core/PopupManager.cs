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
            IKeyInput();
            EscapeKeyInput();

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

        private void EscapeKeyInput()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame 
                && !DialogueManager.Instance.IsDialogueOpen)
            {
                PopDown();
            }
        }

        private void IKeyInput()
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
                PopupUI popup = _popupList.First();//last
                _popupList.Remove(popup);//remove at 
                popup.PopDown();
            }
            else
            {
                PopUp(PopupType.Option);
            }
        }

        public void PopDown(PopupType popupType)
        {
            if (_popupList.Count > 0)
            {
                PopupUI popup = 
                    _popupList.FirstOrDefault(x => x.popupType == popupType);
                _popupList.Remove(popup);//remove at

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
                return null;
            }
            
            return popups[type];
        }

        //for 문2번
        public PopupUI GetRemainPopup(PopupType type)
        {
            if (_popupList.Contains(popups[type]))
            {
                return _popupList.Find(x => x.popupType == type);
            }

            return null;
        }

        public void OpenSettingPopup() => PopUp(PopupType.Setting);
    }
}
