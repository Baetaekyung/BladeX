using DG.Tweening;
using Swift_Blade.Combat.Health;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        private PlayerHealth _playerHealth;

        [SerializeField] private RectTransform healthUI;
        [SerializeField] private Sprite fullHealthIcon;
        [SerializeField] private Sprite burnHealthIcon;
        [SerializeField] private PlayerHealthIcon[] healthIcons;
        [SerializeField] private Image[] shieldImages;

        private void Start()
        {
            _playerHealth = Player.Instance.GetEntityComponent<PlayerHealth>();

            RectTransform rTrm = transform as RectTransform;
            rTrm.sizeDelta = new Vector2(800f, rTrm.sizeDelta.y);

            if (_playerHealth != null)
            {
                _playerHealth.OnHitEvent.AddListener(HandleSetHealthUI);
                _playerHealth.OnHealthUpdateEvent += SetHealthUI;

                //StatInfoUI.OnHStatUp += SetHealthUIIfStatUp;
                //StatInfoUI.OnHealthStatDown += SetHealthUIIfStatDown;

                Player.Instance.GetEntityComponent<PlayerStatCompo>().OnStatChanged += SetHealthUI;

                SetHealthUI(_playerHealth.GetHealthStat.Value, PlayerHealth.CurrentHealth, _playerHealth.ShieldAmount);
            }
        }

        private void OnDestroy()
        {
            if (_playerHealth != null)
            {
                _playerHealth.OnHitEvent.RemoveListener(HandleSetHealthUI);
                _playerHealth.OnHealthUpdateEvent -= SetHealthUI;

                Player.Instance.GetEntityComponent<PlayerStatCompo>().OnStatChanged -= SetHealthUI;
            }
        }

        private void HandleSetHealthUI(ActionData actionData)
        {
            if (_playerHealth == null)
            {
                Debug.Log("Player health compo is null, PlayerHealthUI.cs line: 35");
                return;
            }

            SetHealthUI(_playerHealth.GetHealthStat.Value, _playerHealth.GetCurrentHealth, _playerHealth.ShieldAmount);
        }

        private void SetHealthUI()
        {
            if (_playerHealth == null)
            {
                Debug.Log("Player health compo is null, PlayerHealthUI.cs line: 35");
                return;
            }

            SetHealthUI(_playerHealth.GetHealthStat.Value, _playerHealth.GetCurrentHealth, _playerHealth.ShieldAmount);
        }

        public void SetHealthUI(float maxHealth, float currentHealth, int shieldAmount)
        {
            //how much health player can have??
            #region Health validation
            if (maxHealth > healthIcons.Length)
                maxHealth = healthIcons.Length;

            if (currentHealth > healthIcons.Length)
                currentHealth = healthIcons.Length;
            #endregion

            InitHealthIcons(maxHealth, currentHealth);
            InitShieldIcons(shieldAmount);
        }

        private void InitShieldIcons(int shieldAmount)
        {
            int i;
            int shieldAmountInt = Mathf.RoundToInt(shieldAmount);

            for (i = 0; i < shieldImages.Length; i++)
                shieldImages[i].gameObject.SetActive(false);

            for (i = 0; i < shieldAmountInt; i++)
            {
                shieldImages[i].gameObject.SetActive(true);

                int capture = i;

                #region Animation part
                shieldImages[i].transform
                    .DOShakeRotation(0.3f, Vector3.forward * 55f)
                    .OnComplete(() => shieldImages[capture].transform.DORotate(Vector3.zero, 0.1f))
                    .SetLink(shieldImages[capture].gameObject, LinkBehaviour.KillOnDestroy);
                #endregion
            }
        }

        private void InitHealthIcons(float maxHealth, float currentHealth)
        {
            int i;
            int activeHealthCount = Mathf.RoundToInt(maxHealth);
            int currentHealthCount = Mathf.RoundToInt(currentHealth);

            //All icons off
            for (i = 0; i < healthIcons.Length; i++)
                healthIcons[i].gameObject.SetActive(false);

            //setting active healthIcons
            for (i = 0; i < healthIcons.Length; i++)
            {
                if (i < activeHealthCount)
                {
                    //Init
                    healthIcons[i].gameObject.SetActive(true);
                    healthIcons[i].SetIcon(burnHealthIcon);

                    #region Animation part

                    int capture = i;

                    healthIcons[i].transform
                        .DOShakeRotation(0.3f, Vector3.forward * 55f)
                        .OnComplete(() => healthIcons[capture].transform.DORotate(Vector3.zero, 0.1f))
                        .SetLink(healthIcons[capture].gameObject, LinkBehaviour.KillOnDestroy);

                    #endregion
                }
                else
                    healthIcons[i].SetIcon(null);
            }

            //setting current healthIcons
            for (i = 0; i < currentHealthCount; i++)
                healthIcons[i].SetIcon(fullHealthIcon);
        }
    }
}
