
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Swift_Blade.Audio;
using Swift_Blade.Inputs;
using Swift_Blade.UI;
using Unity.AppUI.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class PopupManager : MonoSingleton<PopupManager>
    {
        public SerializableDictionary<PopupType, PopupUI> popups = new();

        [SerializeField] private InfoBoxPopup infoBoxPopup;
        [SerializeField] private Transform popupCanvasTrm;
        [SerializeField] private BaseAudioSO inventoryAudio;

        private List<PopupUI> _popupList = new List<PopupUI>();
        public event Action OnPopUpOpenOrClose;

        private List<InfoBoxPopup> _infoboxList = new List<InfoBoxPopup>();

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

        private void Start()
        {
            InitPopups();
            InputManager.InventoryEvent += InputEventInventory;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputManager.InventoryEvent -= InputEventInventory;
        }
        private void InputEventInventory()
        {
            OpenCloseInventory();
        }
        private void InitPopups()
        {
            foreach (var popupUI in popups.Values)
                popupUI.PopDown();
        }

        private void Update()
        {
            PopDownInput();

            if (Input.GetKeyDown(KeyCode.P))
                PopUp(PopupType.Status);
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

            if(GetRemainPopup(PopupType.Inventory))
            {
                PopDown(PopupType.Inventory);
                return;
            }

            AudioManager.PlayWithInit(inventoryAudio, true);

            PopUp(PopupType.Inventory);
        }

        public void PopUp(PopupType popupType)
        {
            if (_popupList.Contains(popups[popupType])) return;

            _popupList.Add(popups[popupType]);
            if (Player.Instance != null)
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

                if (popup.popupType == PopupType.GameOver)
                    return;

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
            if (popupType == PopupType.GameOver)
                return;

            if (_popupList.Count > 0)
            {
                if (_popupList.Contains(popups[popupType]))
                {
                    popups[popupType].PopDown();
                    _popupList.Remove(popups[popupType]);

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
            if (popup.popupType == PopupType.GameOver)
                return;

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
        
        public void LogMessage(string message, float time = 2f)
        {
            PopupUI popup = GetPopupUI(PopupType.Text);
            
            if (popup == null)
                return;
            
            TextPopup textPopup = popup as TextPopup;

            textPopup.SetText(message);
            DelayPopup(PopupType.Text, 2f, () => PopDown(PopupType.Text));
        }

        public void LogInfoBox(string message, float timer = 1f)
        {
            bool isRemainInfoBox = _infoboxList.Count > 0;

            //if popup remain in screen
            if (isRemainInfoBox)
            {
                while (_infoboxList.Count > 0)
                {
                    InfoBoxPopup info = _infoboxList.First();

                    _infoboxList.Remove(info);

                    //Fast update
                    info.DOKill();
                    info.Popup(0.3f, () => info.PopDown(0.1f, 
                        () => Destroy(info.gameObject)));
                }

                InfoBoxPopup remainInfobox = Instantiate(infoBoxPopup, popupCanvasTrm);
                remainInfobox.transform.SetAsLastSibling();

                remainInfobox.SetInfoBox(message);
                remainInfobox.Popup(() =>
                {
                    remainInfobox.PopDown();
                });

                _infoboxList.Add(remainInfobox);

                return;
            }

            PopupUI popup = GetPopupUI(PopupType.InfoBox);
            InfoBoxPopup infoPopup = popup as InfoBoxPopup;

            _infoboxList.Add(infoPopup);

            infoPopup.SetInfoBox(message);
            infoPopup.transform.SetAsLastSibling();

            infoPopup.DelayPopup(timer, 
                () =>
                {
                    infoPopup.PopDown();
                });
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
