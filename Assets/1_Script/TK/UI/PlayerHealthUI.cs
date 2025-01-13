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
        [SerializeField] private RectTransform healthUI;
        [SerializeField] private GameObject fullHealthPrefab;
        [SerializeField] private GameObject burnHealthPrefab;

        private List<GameObject> _healthIcons = new List<GameObject>();
        
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

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.I))
        //     {
        //         SetHealthUI(4, 4);
        //     }
        //     else if (Input.GetKeyDown(KeyCode.O))
        //     {
        //         SetHealthUI(5, 3);
        //     }
        // }
    }
}
