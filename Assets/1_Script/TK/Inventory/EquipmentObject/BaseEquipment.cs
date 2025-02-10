using System;
using UnityEngine;

namespace Swift_Blade
{
    //Object�� ���� �����ϴ� Equipment
    public abstract class BaseEquipment : MonoBehaviour, IInteractable
    {
        [SerializeField] protected EquipmentData equipData;
        
        protected PlayerInventory Inventory => InventoryManager.Instance.Inventory;
        protected PlayerStatCompo playerStat;

        protected virtual void Awake()
        {
            playerStat = FindFirstObjectByType<PlayerStatCompo>();
        }

        public abstract void EquipmentEffect(); //������ ȿ��

        //������ ������ ȿ��
        public virtual void OnEquipment()
        {
            equipData.HandleStatAdder(playerStat);
            equipData.EventChannel.SubscribeEvent(EquipmentEffect);
            
            Debug.Log(equipData.name + " ������ ����");
        }
        
        //������ ������ ȿ��
        public virtual void OffEquipment()
        {
            equipData.HandleStatRemover(playerStat);
            equipData.EventChannel.RemoveEvent(EquipmentEffect);
            
            Debug.Log(equipData.name + " ������ ����");
        }
        public abstract void Interact(); //�����۰��� ��ȣ�ۿ�
    }
}
