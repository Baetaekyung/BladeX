using UnityEngine;
using UnityEngine.EventSystems;

namespace Swift_Blade
{
    public class ColorMixButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private ColorType upgradeColor;

        private ColorMixer _colorMixer;

        private void Start()
        {
            _colorMixer = GetComponentInParent<ColorMixer>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _colorMixer.MixColor(upgradeColor);
        }
    }
}
