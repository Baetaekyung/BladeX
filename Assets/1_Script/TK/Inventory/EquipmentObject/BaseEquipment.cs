using System;
using UnityEditor;
using UnityEngine;

namespace Swift_Blade
{
    //Object로 씬에 존재하는 Equipment
    public abstract class BaseEquipment : ItemObject, IInteractable
    {
        [SerializeField] protected EquipmentData equipData;
        protected PlayerStatCompo _playerStat;
        protected PlayerInventory Inventory => InventoryManager.Instance.Inventory;
        
        //아이템 장착시 효과
        public virtual void OnEquipment()
        {
            HandleStatAdder();
            equipData.EventChannel.OnEquipped += ItemEffect;
            
            Debug.Log(equipData.name + " 아이템 장착");
        }
        
        //아이템 해제시 효과
        public virtual void OffEquipment()
        {
            HandleStatRemover();
            equipData.EventChannel.OnEquipped -= ItemEffect;
            
            Debug.Log(equipData.name + " 아이템 해제");
        }
        
        public void HandleStatAdder()
        {
            //이거 왜 캐싱해서 쓰면 자꾸 null이 되는 거지;;
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
                //Key는 StatType, Value는 ModifyValue
                _playerStat.AddModifier(stat.Key, equipData.itemSerialCode, stat.Value);
                
                Debug.Log($"{stat.Key.ToString()}이 {stat.Value}만큼 증가하였다! " +
                          $"SerialNum: {equipData.itemSerialCode}");
            }
        }

        public void HandleStatRemover()
        {
            //이거 왜 캐싱해서 쓰면 자꾸 null이 되는 거지;;
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
                //Key는 StatType
                _playerStat.RemoveModifier(stat.Key, equipData.itemSerialCode);
                
                Debug.Log($"{stat.Key.ToString()}이 {stat.Value}만큼 감소하였다! " +
                          $"SerialNum: {equipData.itemSerialCode}");
            }
        }
        
        public abstract void Interact(); //아이템과의 상호작용
    }
}
