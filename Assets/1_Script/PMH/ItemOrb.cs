using UnityEngine;

namespace Swift_Blade
{
    public class ItemOrb : MonoBehaviour
    {
        [SerializeField] private ItemTableSO itemTables;
        public ItemDataSO ItemData;

        private void Collected()
        {
            int n = Random.Range(0, itemTables.itemTable.Count);
            ItemData = itemTables.itemTable[n].itemData;

            InventoryManager.Instance.AddItemToEmptySlot(ItemData);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("¿‚æ“¥Ÿ! ≥Õ ≥ª≤®æﬂ!");
                Collected();
                Destroy(gameObject);
            }
        }
    }
}
