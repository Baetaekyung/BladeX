using UnityEngine;
using DG.Tweening;

namespace Swift_Blade
{
    public enum ChestType //에 따라서 나올 아이템 확률 조작? (예정)
    {
        Bronze,
        Silver,
        Gold
    }

    public class ChestController : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemOrb itemOrb;
        [SerializeField] private float shootAngle = -15f;
        [SerializeField] private float shootPower = 5f;

        [SerializeField] private ItemTableSO itemTableSO;

        [SerializeField] private Transform[] chestVisual;
        private Transform chestLid;

        private ChestType chestType;

        private void OnEnable()
        {
            SetRandomChestType();
            SetChestLidTrm();
        }

        private void SetRandomChestType()
        {
            int i = 0;
            foreach(Transform t in transform)
            {
                chestVisual[i] = t;
                chestVisual[i++].gameObject.SetActive(false);
            }
            //비주얼배열에 자식상자들넣어주기, 모두 셋엑티브팔스
            
            int n = Random.Range(0, chestVisual.Length - 1);

            if (n == 0)
            {
                chestType = ChestType.Bronze;
            }
            else if (n == 1)
            {
                chestType = ChestType.Silver;
            }
            else if (n == 2)
            {
                chestType = ChestType.Gold;
            }

            chestVisual[n].gameObject.SetActive(true);
        }

        private void SetChestLidTrm()
        {
            if (chestType == ChestType.Bronze) chestLid = chestVisual[0].GetChild(0);
            else if (chestType == ChestType.Silver) chestLid = chestVisual[1].GetChild(0);
            else if (chestType == ChestType.Gold) chestLid = chestVisual[2].GetChild(0);
        }

        public void OpenChest()
        {
            OpenChestLid();
            InstItemOrb();
            GetRandomItem();
        }

        private void OpenChestLid()
        {
            Vector3 openLidAngle = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);
            chestLid.DORotate(openLidAngle, 0.5f);
        }

        private void InstItemOrb()
        {
            Vector3 spawnPos = transform.position + new Vector3(0, 0.2f, 0);
            ItemOrb orb = Instantiate(itemOrb, spawnPos, Quaternion.identity);

            orb.ItemData = GetRandomItem();

            orb.transform.eulerAngles = new Vector3(shootAngle, 0, 0);
            orb.GetComponent<Rigidbody>().AddForce(Vector3.up * shootPower, ForceMode.Impulse);
        }
        private ItemDataSO GetRandomItem()
        {
            int itemCount = itemTableSO.itemTable.Count;
            int radoamIndex = Random.Range(0, itemCount);

            return itemTableSO.itemTable[radoamIndex].itemData;

            //InventoryManager.Instance.AddItemToEmptySlot(item.itemData);
            //인벤토리SO에 add엠티
        }

        public void Interact()
        {
            Debug.Log("열려라참깨");
            OpenChest();
        }
    }
}
