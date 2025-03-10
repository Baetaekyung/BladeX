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

        private void Awake()
        {
            levelClearSO.LevelClearEvent += levelClearAction;
        }

        private void Start()
        {
            timer = default_timeValue;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            TextManager();

            if(timer <= 0)
            {
                levelClearAction?.Invoke();
            }
        }

        private void TextManager()
        {
            timerText.text = (int)timer + " " + "/s";
        }
    }
}
