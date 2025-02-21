using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "EquipmentChannelSO", menuName = "SO/EquipmentEvent/EventChannel")]
    public class EquipmentChannelSO : ScriptableObject
    {
        //todo : event
        public Action OnEquipped; //매게변수 없이 실행만 할때

        public void RaiseEvent()
        {
            OnEquipped?.Invoke();
        }
        
        public void SubscribeEvent(Action newEvent)
        {
            OnEquipped += newEvent;
        }

        public void RemoveEvent(Action targetEvent)
        {
            OnEquipped -= targetEvent;
        }
    }
}
