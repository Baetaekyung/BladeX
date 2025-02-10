using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "EquipmentEventArgument", menuName = "SO/EquipmentEvent/Argument")]
    public class EquipmentEventArgSO : ScriptableObject
    {
        public SerializableDictionary<string, float> eventArgs 
            = new SerializableDictionary<string, float>();

        public float GetValue(string name)
        {
            float value = 0f;
            
            if (eventArgs.TryGetValue(name, out value)){}

            if (value == 0f)
            {
                Debug.Log("존재하지 않는 매개변수 이름: " + name);
            }

            return value;
        }
        
        public void TryGetValue(string name, ref float value)
        {
            if (eventArgs.TryGetValue(name, out value)){}
        }
    }
    
    [CreateAssetMenu(fileName = "EquipmentChannelSO", menuName = "SO/EquipmentEvent/EventChannel")]
    public class EquipmentChannelSO : ScriptableObject
    {
        public Action OnEquipped; //매게변수 없이 실행만 할때
        public Action<EquipmentEventArgSO> OnEquippedWithArg; //매게변수가 필요할 때
        [Space]
        public EquipmentEventArgSO equipmentArgSO;

        public void RaiseEvent()
        {
            OnEquipped?.Invoke();
            
            if (equipmentArgSO == null)
            {
                Debug.Log("매게변수가 할당되지 않음");
                return;
            }
            OnEquippedWithArg?.Invoke(equipmentArgSO);
        }
        
        public void SubscribeEvent(Action newEvent)
        {
            OnEquipped += newEvent;
        }

        public void SubscribeEvent(Action<EquipmentEventArgSO> newEvent)
        {
            OnEquippedWithArg += newEvent;
        }

        public void RemoveEvent(Action targetEvent)
        {
            OnEquipped -= targetEvent;
        }

        public void RemoveEvent(Action<EquipmentEventArgSO> targetEvent)
        {
            OnEquippedWithArg -= targetEvent;
        }
    }
}
