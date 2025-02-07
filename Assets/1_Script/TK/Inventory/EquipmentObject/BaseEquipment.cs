using System;
using UnityEngine;

namespace Swift_Blade
{
    //Object로 씬에 존재하는 Equipment
    public abstract class BaseEquipment : MonoBehaviour, IInteractable
    {
        [SerializeField] protected EquipmentData equipData;
        
        protected PlayerInventory Inventory => InventoryManager.Instance.Inventory;
        protected PlayerStatCompo playerStat;

        protected virtual void Awake()
        {
            playerStat = FindFirstObjectByType<PlayerStatCompo>();
        }

        public abstract void EquipmentEffect(); //아이템 효과

        //아이템 장착시 효과
        public virtual void OnEquipment()
        {
            equipData.HandleStatAdder(playerStat);
            equipData.EventChannel.SubscribeEvent(EquipmentEffect);
            
            Debug.Log(equipData.name + " 아이템 장착");
        }
        
        //아이템 해제시 효과
        public virtual void OffEquipment()
        {
            equipData.HandleStatRemover(playerStat);
            equipData.EventChannel.RemoveEvent(EquipmentEffect);
            
            Debug.Log(equipData.name + " 아이템 해제");
        }
        public abstract void Interact(); //아이템과의 상호작용
    }
}
