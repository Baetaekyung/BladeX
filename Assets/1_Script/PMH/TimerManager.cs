using System;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class TimerManager : MonoBehaviour
    {
        [SerializeField] private LevelClearEventSO levelClearSO;
        private Action levelClearAction;

        [SerializeField] private float default_timeValue;

        private float timer = 0;

        [SerializeField] private TMP_Text timerText;

        [SerializeField] private Transform ResultPanel;
        [SerializeField] private TextMeshProUGUI coinValueTxt;

        [SerializeField] private CoinManager coinManager;

        private void Awake()
        {
            levelClearSO.LevelClearEvent += levelClearAction;
        }

        private void Start()
        {
            ResultPanel.gameObject.SetActive(false);

            timer = default_timeValue;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            TextManager();

            if(timer <= 0)
            {
                GameFinished();
            }
        }

        private void TextManager()
        {
            timerText.text = (int)timer + " " + "/s";
        }

        private void GameFinished()
        {
            ResultPanel.gameObject.SetActive(true);

            coinValueTxt.text = "coin : " + coinManager.coinValue;
            levelClearAction?.Invoke();
        }
    }
}
