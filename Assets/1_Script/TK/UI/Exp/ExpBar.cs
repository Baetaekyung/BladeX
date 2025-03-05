using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ExpBar : MonoBehaviour
    {
        [SerializeField] private Image gauge;
        [SerializeField] private TextMeshProUGUI infoText;

        private void Update()
        {
            infoText.text = $"{Player.level.Experience} / 2";
            gauge.fillAmount = Player.level.Experience / 2f;
        }
    }
}
