using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class RgbUI : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ColorType, TextMeshProUGUI> rgbText = new();

        private PlayerStatCompo _playerStatCompo;

        private void Start()
        {
            _playerStatCompo = Player.Instance.GetEntityComponent<PlayerStatCompo>();

            _playerStatCompo.ColorValueChangedAction += SetUI;
            ColorRecorder.OnColorChanged += SetUI;

            Player.level.StatPoint = 10;
            SetUI();
        }

        private void OnDisable()
        {
            _playerStatCompo.ColorValueChangedAction -= SetUI;
            ColorRecorder.OnColorChanged -= SetUI;
        }

        public void SetUI()
        {
            foreach (var colorType in rgbText.Keys)
            {
                int val = _playerStatCompo.GetColorStatValue(colorType);

                Color color = ColorUtils.GetCustomColor(colorType);
                string colorText = ColorUtils.ColorText(KoreanUtility.GetColorTypeKorean(colorType), color);
                
                rgbText[colorType].text = $"{colorText}: {val}";
            }
        }
    }
}
