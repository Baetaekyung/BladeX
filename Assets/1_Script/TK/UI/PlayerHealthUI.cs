using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Swift_Blade.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        private PlayerHealth _playerHealth;
        [SerializeField] private RectTransform healthUI;
        [SerializeField] private GameObject fullHealthPrefab;
        [SerializeField] private GameObject burnHealthPrefab;

        private List<GameObject> _healthIcons;

        private void Start()
        {
            _healthIcons = new List<GameObject>();

            _playerHealth = FindFirstObjectByType<PlayerHealth>();
            
            if (_playerHealth != null)
            {
                _playerHealth.OnHitEvent.AddListener(HandleSetHealthUI);
                _playerHealth.OnHealthUpdateEvent += SetHealthUI;
                StatInfoUI.OnHealthStatUp += SetHealthUIIfStatUp;
                StatInfoUI.OnHealthStatDown += SetHealthUIIfStatDown;
                
                SetHealthUI(_playerHealth.GetHealthStat.Value, PlayerHealth._currentHealth);
            }
        }

        private void SetHealthUIIfStatDown()
        {
            if (_playerHealth == null)
            {
                Debug.Log("Player health compo is null, PlayerHealthUI.cs line: 35");
                return;
            }
            
            SetHealthUI(_playerHealth.GetHealthStat.Value, --PlayerHealth._currentHealth);
        }

        private void OnDestroy()
        {
            if (_playerHealth != null)
            {
                _playerHealth.OnHitEvent.RemoveListener(HandleSetHealthUI);
                _playerHealth.OnHealthUpdateEvent -= SetHealthUI;
                StatInfoUI.OnHealthStatUp -= SetHealthUIIfStatUp;
                StatInfoUI.OnHealthStatDown -= SetHealthUIIfStatDown;
            }
        }

        private void HandleSetHealthUI(ActionData actionData)
        {
            if (_playerHealth == null)
            {
                Debug.Log("Player health compo is null, PlayerHealthUI.cs line: 35");
                return;
            }
            
            SetHealthUI(_playerHealth.GetHealthStat.Value, _playerHealth.GetCurrentHealth);
        }
        
        private void SetHealthUIIfStatUp()
        {
            if (_playerHealth == null)
            {
                Debug.Log("Player health compo is null, PlayerHealthUI.cs line: 35");
                return;
            }
            
            SetHealthUI(_playerHealth.GetHealthStat.Value, ++PlayerHealth._currentHealth);
        }
        
        public void SetHealthUI(float maxHealth, float currentHealth)
        {
            if (_healthIcons.Count != 0)
            {
                _healthIcons.ForEach(icon => Destroy(icon.gameObject));
                _healthIcons.Clear();
            }
            
            int intMaxHealth = Mathf.RoundToInt(maxHealth);
            int emptyHealth = intMaxHealth - Mathf.RoundToInt(currentHealth);
            int fullHealth = intMaxHealth - emptyHealth;
            
            for (int i = 0; i < fullHealth; i++)
            {
                GameObject icon = Instantiate(fullHealthPrefab, healthUI);
                _healthIcons.Add(icon);
            }

            for (int j = 0; j < emptyHealth; j++)
            {
                GameObject icon = Instantiate(burnHealthPrefab, healthUI);
                icon.transform.DOShakeRotation(0.4f, Vector3.forward * 25f);
                _healthIcons.Add(icon);
            }
        }
    }
}
