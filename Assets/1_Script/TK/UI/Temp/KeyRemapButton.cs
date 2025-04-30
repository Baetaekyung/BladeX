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
                PopupManager.Instance.LogMessage("������ �� ���� Ű �Դϴ�.");
                return;
            }

            InputM.Rebind(inputType);
            PopupManager.Instance.LogMessage("����� Ű�� �Է��ϼ���");
        }

        private void UpdateKeyText()
        {
            _keyText.text = InputM.GetCurrentKeyByType(inputType);
        }
    }
}
