using System;
using UnityEditor;
using UnityEngine;

namespace Swift_Blade
{
    //Object�� ���� �����ϴ� Equipment
    public abstract class BaseEquipment : ItemObject, IInteractable
    {
        [SerializeField] protected EquipmentData equipData;
        protected PlayerStatCompo _playerStat;
        protected PlayerInventory Inventory => InventoryManager.Instance.Inventory;
        
        //������ ������ ȿ��
        public virtual void OnEquipment()
        {
            HandleStatAdder();
            EquipmentChannelSO channel = equipData.EventChannel;
            if(channel != null)
                channel.OnEquipped += ItemEffect;
            
            Debug.Log(equipData.name + " ������ ����");
        }
        
        //������ ������ ȿ��
        public virtual void OffEquipment()
        {
            HandleStatRemover();
            EquipmentChannelSO channel = equipData.EventChannel;
            if (channel != null)
                channel.OnEquipped -= ItemEffect;

            Debug.Log(equipData.name + " ������ ����");
        }
        
        public void HandleStatAdder()
        {
            //�̰� �� ĳ���ؼ� ���� �ڲ� null�� �Ǵ� ����;;
            _playerStat = FindFirstObjectByType<PlayerStatCompo>();
            
            if (_playerStat == null)
            {
                Debug.LogWarning("Player Stat Component is missing, " +
                                 "add <b>PlayerStatCompo</b> to player");
                return;
            }

            if (equipData.statModifier.Count == 0)
                return;
            
            foreach (var stat in equipData.statModifier)
            {
                //Key�� StatType, Value�� ModifyValue
                _playerStat.AddModifier(stat.Key, equipData.itemSerialCode, stat.Value);
                
                Debug.Log($"{stat.Key.ToString()}�� {stat.Value}��ŭ �����Ͽ���! " +
                          $"SerialNum: {equipData.itemSerialCode}");
            }
        }

        public void HandleStatRemover()
        {
            //�̰� �� ĳ���ؼ� ���� �ڲ� null�� �Ǵ� ����;;
            _playerStat = FindFirstObjectByType<PlayerStatCompo>();
            
            if (_playerStat == null)
            {
                Debug.LogWarning("Player Stat Component is missing, " +
                                 "add <b>PlayerStatCompo</b> to player");
                return;
            }

            if (equipData.statModifier.Count == 0)
                return;
            
            foreach (var stat in equipData.statModifier)
            {
                //Key�� StatType
                _playerStat.RemoveModifier(stat.Key, equipData.itemSerialCode);
                
                Debug.Log($"{stat.Key.ToString()}�� {stat.Value}��ŭ �����Ͽ���! " +
                          $"SerialNum: {equipData.itemSerialCode}");
            }
        }
        
        public abstract void Interact(); //�����۰��� ��ȣ�ۿ�
    }
}
