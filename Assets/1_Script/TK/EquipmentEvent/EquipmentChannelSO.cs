using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "EquipmentChannelSO", menuName = "SO/EquipmentEvent/EventChannel")]
    public class EquipmentChannelSO : ScriptableObject
    {
        //todo : event
        public Action OnEquipped; //�ŰԺ��� ���� ���ุ �Ҷ�

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
