using System;
using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "PlayerUIContainer", menuName = "SO/PlayerUIContainerSO")]
    public class PlayerUIContainerSO : ScriptableObject
    {
        public PlayerHealthUI playerHealthUI;
        public StatUI playerStatUI;

        public void SetHealthUI(int currentHealth, int maxHealth)
        {
            if (playerHealthUI == null) return;
            
            playerHealthUI.SetHealthUI(currentHealth, maxHealth);
        }

        public void SetStatUI(StatComponent playerStat)
        {
            if (playerStatUI == null) return;
            
            playerStatUI.SetTargetStat(playerStat);
        }

        public void ShowStatUI(float eraseTime)
        {
            if (playerStatUI == null) return;
            
            playerStatUI.ShowUIDelayUnShow(eraseTime);
        }
    }
}
