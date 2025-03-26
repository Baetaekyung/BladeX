using UnityEngine;
using DG.Tweening;

namespace Swift_Blade.Level
{
    public enum ChestType //에 따라서 나올 아이템 확률 조작? (예정)
    {
        Bronze,
        Silver,
        Gold
    }

    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform playerTrm;
        
        [SerializeField] private ItemOrb itemOrb;
        [SerializeField] private float shootAngle = -15f;
        [SerializeField] private float shootPower = 5f;
        
        [SerializeField] private ItemTableSO itemTableSO;
        
        [SerializeField] private Transform[] chestVisual;
        
        private Transform chestLid;
        private ChestType chestType;
        private Rigidbody rigidbody;

        private bool isOpen = false;
        
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
                        
            SetRandomChestType();
        }

        private void SetRandomChestType()
        {
            foreach(var t in chestVisual)
            {
                t.gameObject.SetActive(false);
            }
                        
            int n = Random.Range(0, chestVisual.Length);
            chestType = (ChestType)n;
            chestVisual[n].gameObject.SetActive(true);
            chestLid = chestVisual[n].GetChild(0);
            
        }

        private void OpenChest()
        {
            if (isOpen) return;
            
            isOpen = true;
            
            OpenChestAnimations();
            InstItemOrb();
            GetRandomItem();
        }

        private void OpenChestAnimations()
        {
            //rigidbody.isKinematic = true; 

            Vector3 openLidAngle = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);
            chestLid.DORotate(openLidAngle, 0.3f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Fixed);

            rigidbody.DOJump(rigidbody.position + Vector3.up * 0.25f, 0.05f, 1, 0.15f);
            rigidbody.DORotate(rigidbody.rotation.eulerAngles + new Vector3(5, 0, 0), 0.1f);
            
            //DOVirtual.DelayedCall(0.4f, () => rigidbody.isKinematic = false);
        }
        
        private void InstItemOrb()
        {
            Vector3 spawnPos = transform.localPosition + new Vector3(0, 0.2f, 0);
            ItemOrb orb = Instantiate(itemOrb, spawnPos, Quaternion.identity);
            orb.transform.eulerAngles = new Vector3(shootAngle, 0, 0);
            
            Vector3 shootForce = Vector3.up * shootPower + (playerTrm.position - transform.position) * (shootPower / 13);
            orb.GetComponent<Rigidbody>().AddForce(shootForce, ForceMode.Impulse);
        }
        private ItemDataSO GetRandomItem()
        {
            int itemCount = itemTableSO.itemTable.Count;
            int randomIndex = Random.Range(0, itemCount);

            return itemTableSO.itemTable[randomIndex].itemData;
            
            //InventoryManager.Instance.AddItemToEmptySlot(item.itemData);
            //인벤토리SO에 add엠티
        }

        public void Interact()
        {
            //Debug.Log("열려라참깨");
            OpenChest();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                playerTrm = other.transform;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(playerTrm != null && other.CompareTag("Player"))
            {
                playerTrm = null;
            }
        }
    }
}
