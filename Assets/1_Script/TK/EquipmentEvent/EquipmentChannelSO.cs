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
                Debug.Log("�������� �ʴ� �Ű����� �̸�: " + name);
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
        public Action OnEquipped; //�ŰԺ��� ���� ���ุ �Ҷ�
        public Action<EquipmentEventArgSO> OnEquippedWithArg; //�ŰԺ����� �ʿ��� ��
        [Space]
        public EquipmentEventArgSO equipmentArgSO;

        public void RaiseEvent()
        {
            OnEquipped?.Invoke();
            
            if (equipmentArgSO == null)
            {
                Debug.Log("�ŰԺ����� �Ҵ���� ����");
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
