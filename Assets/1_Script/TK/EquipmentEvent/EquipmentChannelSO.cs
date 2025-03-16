using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "EquipmentChannelSO", menuName = "SO/EquipmentEvent/EventChannel")]
    public class EquipmentChannelSO : ScriptableObject
    {
        public event Action<Player> OnEquipped;

        public void RaiseEvent(Player player)
        {
            OnEquipped?.Invoke(player);
        }
    }
}
