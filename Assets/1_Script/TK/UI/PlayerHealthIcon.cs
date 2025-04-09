using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class PlayerHealthIcon : MonoBehaviour
    {
        [SerializeField] private Image healthIcon;

        public void SetIcon(Sprite icon)
        {
            if(icon == null)
            {
                healthIcon.color = Color.clear;
                return;
            }

            healthIcon.color = Color.white;
            healthIcon.sprite = icon;
        }
    }
}
