using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private PlayerUIContainerSO playerUIContainer;
        
        [SerializeField] private RectTransform healthUI;
        [SerializeField] private GameObject fullHealthPrefab;
        [SerializeField] private GameObject burnHealthPrefab;

        private List<GameObject> _healthIcons = new List<GameObject>();

        private void Awake()
        {
            playerUIContainer.playerHealthUI = this;
        }

        public void SetHealthUI(int maxHealth, int currentHealth)
        {
            if (_healthIcons.Count != 0)
            {
                _healthIcons.ForEach(icon => Destroy(icon.gameObject));
                _healthIcons.Clear();
            }
            
            healthUI.sizeDelta = new Vector2(75 * maxHealth, 100f);
            
            int emptyHealth = maxHealth - currentHealth;

            for (int i = 0; i < maxHealth - emptyHealth; i++)
            {
                GameObject icon = Instantiate(fullHealthPrefab, healthUI);
                _healthIcons.Add(icon);
            }

            for (int j = 0; j < emptyHealth; j++)
            {
                GameObject icon = Instantiate(burnHealthPrefab, healthUI);
                _healthIcons.Add(icon);
            }
        }
    }
}
