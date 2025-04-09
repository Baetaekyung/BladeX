using System;
using System.Collections.Generic;
using System.Linq;
using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class PopupManager : MonoSingleton<PopupManager>
    {
        public SerializableDictionary<PopupType, PopupUI> popups = new();

        private List<PopupUI> _popupList = new List<PopupUI>();
        public event Action   OnPopUpOpenOrClose;

        [SerializeField] private float infoRemainTime = 2f;
        private float infoTimer = 0f;
        private bool  infoboxRemain = false;

        public bool IsRemainPopup
        {
            get
            {
                bool isRemain = false;

                for (int i = 0; i < _popupList.Count; i++)
                {
                    if (_popupList[i].popupType != PopupType.InfoBox
                        && _popupList[i].popupType != PopupType.Text)
                    {
                        isRemain = true;
                    }
                }

                return isRemain;
            }
        }

        public bool InfoBoxRemain
        {
            get => infoboxRemain;
            set => infoboxRemain = value;
        }

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
            CheckInfoBox();
        }

        private void CheckInfoBox()
        {
            if (infoboxRemain)
            {
                if (infoTimer < infoRemainTime)
                    infoTimer += Time.unscaledDeltaTime;
                else
                {
                    infoTimer = 0f;
                    infoboxRemain = false;

                    if (GetRemainPopup(PopupType.InfoBox))
                    {
                        PopDown(PopupType.InfoBox);
                    }
                }
            }
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
            if(Player.Instance != null)
                Player.Instance.GetEntityComponent<PlayerMovement>().InputDirection = Vector3.zero;

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

        public void PopDown(PopupUI popup)
        {
            if (_popupList.Count > 0)
            {
                if (popup != null)
                {
                    _popupList.Remove(popup);
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
            PopupUI popup = GetPopupUI(PopupType.Text);
            TextPopup textPopup = popup as TextPopup;

            textPopup.SetText(message);
            DelayPopup(PopupType.Text, 1f, () => PopDown(PopupType.Text));
        }

        public void LogInfoBox(string message)
        {
            //if popup remain in screen
            if (GetRemainPopup(PopupType.InfoBox) != null)
            {
                PopupUI remain = GetRemainPopup(PopupType.InfoBox);
                InfoBoxPopup remainInfobox = remain as InfoBoxPopup;

                remainInfobox.SetInfoBox(message);

                infoTimer = 0f;
                infoboxRemain = true;

                return;
            }

            PopupUI popup = GetPopupUI(PopupType.InfoBox);
            InfoBoxPopup infoPopup = popup as InfoBoxPopup;

            infoPopup.SetInfoBox(message);
            PopUp(PopupType.InfoBox);

            infoTimer = 0f;
            infoboxRemain = true;
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
            return _popupList.FirstOrDefault(x => x.popupType == type);
        }

        public void OpenSettingPopup() => PopUp(PopupType.Setting);
        public void OpenQuitHelpPopup() => PopUp(PopupType.Option); 
    }
}
