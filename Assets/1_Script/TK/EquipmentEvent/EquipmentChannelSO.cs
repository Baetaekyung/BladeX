using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "EquipmentChannelSO", menuName = "SO/EquipmentEvent/EventChannel")]
    public class EquipmentChannelSO : ScriptableObject
    {
        public event Action OnEquipped; 
        
        public void RaiseEvent()
        {
            OnEquipped?.Invoke();
        }
    }
}
