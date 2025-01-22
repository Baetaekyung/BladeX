using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SelectItem_UI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI typeText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        public Image iconImage;

        public void SetUI(string title, string type, string description)
        {
            titleText.text = title;
            typeText.text = type;
            descriptionText.text = description;
        }
    }
}
