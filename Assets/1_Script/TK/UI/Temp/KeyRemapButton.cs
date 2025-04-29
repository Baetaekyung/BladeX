using Swift_Blade.Inputs;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class KeyRemapButton : MonoBehaviour
    {
        [SerializeField] private InputType inputType;
        [SerializeField] private bool isChangable = true;

        private TextMeshProUGUI _keyText;
        private Button _button;
        private InputManager InputM;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _keyText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            InputM = InputManager.Instance;

            string curKey = InputM.GetCurrentKeyByType(inputType);

            InputManager.RebindEndEvent += UpdateKeyText;
            UpdateKeyText();

            _button.onClick.AddListener(HandleKeymap);
        }

        private void HandleKeymap()
        {
            if (isChangable == false)
            {
                PopupManager.Instance.LogMessage("변경할 수 없는 키 입니다.");
                return;
            }

            InputM.Rebind(inputType);
            PopupManager.Instance.LogMessage("등록할 키를 입력하세요");
        }

        private void UpdateKeyText()
        {
            _keyText.text = InputM.GetCurrentKeyByType(inputType);
        }
    }
}
