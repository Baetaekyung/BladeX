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
        private PlayerHealth _playerHealth;
        [SerializeField] private RectTransform healthUI;
        [SerializeField] private GameObject fullHealthPrefab;
        [SerializeField] private GameObject burnHealthPrefab;

        private List<GameObject> _healthIcons = new List<GameObject>();

        private void Start()
        {
            _playerHealth = FindFirstObjectByType<PlayerHealth>();

            if (_playerHealth != null)
            {
                SetHealthUI(_playerHealth.GetMaxHealth, _playerHealth.GetCurrentHealth);
                
                _playerHealth.OnHitEvent.AddListener(HandleSetHealthUI);
            }
        }

        private void OnDestroy()
        {
            if (_playerHealth != null)
            {
                _playerHealth.OnHitEvent.RemoveListener(HandleSetHealthUI);
            }
        }

        private void HandleSetHealthUI(ActionData actionData)
        {
            if (_playerHealth == null)
            {
                Debug.Log("Player health compo is null, PlayerHealthUI.cs line: 35");
                return;
            }
            
            SetHealthUI(_playerHealth.GetCurrentHealth, _playerHealth.GetCurrentHealth);
        }

        [ContextMenu("Test Setting Health")]
        public void SetHealthUITest()
        {
            SetHealthUI(5, 5);
        }
        
        public void SetHealthUI(float maxHealth, float currentHealth)
        {
            if (_healthIcons.Count != 0)
            {
                _healthIcons.ForEach(icon => Destroy(icon.gameObject));
                _healthIcons.Clear();
            }
            
            healthUI.sizeDelta = new Vector2(75 * maxHealth, 100f);
            
            float emptyHealth = maxHealth - currentHealth;

            for (int i = 0; i < Mathf.RoundToInt(maxHealth - emptyHealth); i++)
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
