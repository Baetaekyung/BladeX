using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Swift_Blade.Inputs;
using Swift_Blade.UI;
using Unity.AppUI.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class PopupManager : MonoSingleton<PopupManager>
    {
        public SerializableDictionary<PopupType, PopupUI> popups = new();

        public bool isLayerTextInfo;

        [SerializeField] private InfoBoxPopup infoBoxPopup;
        [SerializeField] private Transform popupCanvasTrm;

        private List<PopupUI> _popupList = new List<PopupUI>();
        public event Action OnPopUpOpenOrClose;
        
        private readonly Queue<string> _infoBoxQueue = new();
        private bool isDisplayingInfoBox = false;
        private WaitForSeconds infoBoxWait;
        private const float INFO_BOX_INTERVAL = 0.16f; 
        private const float INFO_BOX_FADE_OUT_TIME = 0.43f; 
        
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

            infoBoxWait = new WaitForSeconds(INFO_BOX_INTERVAL);
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

        public void LogInfoBox(string message, float timer = INFO_BOX_FADE_OUT_TIME)
        {
            _infoBoxQueue.Enqueue(message);
            
            if (!isDisplayingInfoBox)
                StartCoroutine(ProcessInfoBoxQueue(timer));
        }

        private IEnumerator ProcessInfoBoxQueue(float timer)
        {
            isDisplayingInfoBox = true;

            while (_infoBoxQueue.Count > 0)
            {
                string message = _infoBoxQueue.Dequeue();

                //if popup remain in screen
                if (GetRemainPopup(PopupType.InfoBox) != null)
                {
                    if (!isLayerTextInfo)
                    {
                        PopupUI remain = GetRemainPopup(PopupType.InfoBox);
                        if (remain is InfoBoxPopup infoB)
                            infoB.SetInfoBox("");
                    }

                    InfoBoxPopup remainInfobox = Instantiate(infoBoxPopup, popupCanvasTrm);
                    remainInfobox.transform.SetAsFirstSibling();
                    remainInfobox.SetInfoBox(message);
                    remainInfobox.DelayPopup(INFO_BOX_FADE_OUT_TIME, () => Destroy(remainInfobox.gameObject));
                }
                else
                {
                    PopupUI popup = GetPopupUI(PopupType.InfoBox);
                    if (popup is InfoBoxPopup infoPopup)
                    {
                        infoPopup.SetInfoBox(message);
                        DelayPopup(PopupType.InfoBox, timer, () => PopDown(PopupType.InfoBox));
                    }
                }

                yield return infoBoxWait;
            }
            
            isDisplayingInfoBox = false;
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
